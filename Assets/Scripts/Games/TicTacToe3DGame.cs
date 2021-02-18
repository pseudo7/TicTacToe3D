using System.Collections.Generic;
using System.Text;
using TicTacToe3D.Pillars;
using TicTacToe3D.Services;
using TicTacToe3D.Shapes;
using TicTacToe3D.StateMachine;
using TicTacToe3D.Utilities;
using UnityEngine;

namespace TicTacToe3D.Games
{
    public class TicTacToe3DGame : GameBase
    {
        //TODO: Add Single Pillar & Its pillar to have high light property as a single unit
        //TODO: Add Delay Highlight (Check if in Mobile it is shown)
        protected internal override GameEnums.ShapeType[,,] GameMatrix { get; set; }
        protected internal override PillarBase[] Pillars { get; set; }

        private bool _stateTransitionPossible;
        private int _currentShapeCount;
        private int _currentMaterialCount;

        protected internal override SwipeRotator CurrentSwipeRotator { get; set; }

        protected override int Dimension { set; get; }

        protected override void SetupGame()
        {
            Dimension = 3;
            GameMatrix = new GameEnums.ShapeType [Dimension, Dimension, Dimension];
            Pillars = new PillarBase[9];
            GameStateMachine = gameObject.AddComponent<TurnBasedStateMachine>();
            GameStateMachine.Initialisation(new NextTurnState(StateAction));
            CurrentSwipeRotator = FindObjectOfType<SwipeRotator>();
        }

        protected override IStateMachine GameStateMachine { get; set; }

        protected internal override void HighLightPillar(int pillarIndex)
        {
            var count = Pillars.Length;
            while (count-- > 0)
            {
                Pillars[count]
                    .SetMaterial(Bootstrap.BootstrapInstance.GetService<ResourcesService>().LightWoodMatFaded);
                Pillars[count].SetSegmentMaterial(true);
            }

            Pillars[pillarIndex].SetMaterial(Bootstrap.BootstrapInstance.GetService<ResourcesService>().LightWoodMat);
            Pillars[pillarIndex].SetSegmentMaterial(false);
        }

        protected internal override void UnHighLightPillar(int pillarIndex)
        {
            var count = Pillars.Length;
            while (count-- > 0)
            {
                Pillars[count].SetMaterial(Bootstrap.BootstrapInstance.GetService<ResourcesService>().LightWoodMat);
                Pillars[count].SetSegmentMaterial(false);
            }
        }

        protected internal override void AddShape(int pillarIndex)
        {
            if (!_stateTransitionPossible)
            {
                Debug.Log($"State Transition Not Possible".ToColoredString(Color.red));
                return;
            }

            var pillar = Pillars[pillarIndex];
            var targetSegment = pillar.PillarSegments
                .Find(x => !x.CurrentShape);
            if (!targetSegment)
            {
                Debug.LogError($"Segments are filled for Pillar: {pillarIndex}");
                return;
            }

            var shapeType = (_currentShapeCount++ & 1) != 0 ? GameEnums.ShapeType.Cross : GameEnums.ShapeType.Zero;
            ShapeBase shapePrefab = null;
            switch (shapeType)
            {
                case GameEnums.ShapeType.Cross:
                    shapePrefab = Bootstrap.BootstrapInstance.GetService<ResourcesService>().CrossPrefab;
                    break;
                case GameEnums.ShapeType.Zero:
                    shapePrefab = Bootstrap.BootstrapInstance.GetService<ResourcesService>().SpherePrefab;
                    break;
            }

            var spawnedShape = Instantiate(shapePrefab, pillar.SegmentSpawnPosition, pillar.SegmentSpawnRotation);
            spawnedShape.transform.SetParent(pillar.transform, true);
            spawnedShape.SetMaterial((_currentMaterialCount++ & 1) != 0
                ? Bootstrap.BootstrapInstance.GetService<ResourcesService>().DarkWoodMat
                : Bootstrap.BootstrapInstance.GetService<ResourcesService>().LightWoodMat);

            targetSegment.CurrentShape = spawnedShape;
            GameMatrix[pillar.PillarRowIndex, pillar.PillarColumnIndex, targetSegment.SegmentIndex] =
                shapeType;
            PrintGrid();
            _stateTransitionPossible = false;
            targetSegment.MoveShapeToSegment(() => GameStateMachine.UpdateState(new NextTurnState(StateAction)));
            CheckForFinish();
        }

        protected override bool CheckForFinish()
        {
            List<(int, int, int)> solution;
            if (CheckForPillars(out solution))
            {
                solution.ForEach(tuple => Debug.Log(tuple.ToString().ToColoredString(Color.green)));
                return true;
            }

            if (CheckForHLevels(out solution))
            {
                solution.ForEach(tuple => Debug.Log(tuple.ToString().ToColoredString(Color.cyan)));
                return true;
            }

            if (CheckForVLevels(out solution))
            {
                solution.ForEach(tuple => Debug.Log(tuple.ToString().ToColoredString(Color.yellow)));
                return true;
            }

            if (CheckForForwardDiagonals(out solution))
            {
                solution.ForEach(tuple => Debug.Log(tuple.ToString().ToColoredString(Color.red)));
                return true;
            }

            if (CheckForReverseDiagonals(out solution))
            {
                solution.ForEach(tuple => Debug.Log(tuple.ToString().ToColoredString(Color.magenta)));
                return true;
            }

            if (CheckForForwardDiagonalDiagonals(out solution))
            {
                solution.ForEach(tuple => Debug.Log(tuple.ToString().ToColoredString(Color.white)));
                return true;
            }

            if (CheckForReverseDiagonalDiagonals(out solution))
            {
                solution.ForEach(tuple => Debug.Log(tuple.ToString().ToColoredString(Color.white)));
                return true;
            }

            return false;
        }

        private bool CheckForPillars(out List<(int, int, int)> solution)
        {
            solution = new List<(int, int, int)>(Dimension);
            int addition;
            for (var i = 0; i < Dimension; i++)
            {
                for (var j = 0; j < Dimension; j++)
                {
                    addition = 1;
                    solution.Clear();
                    for (var k = 0; k < Dimension; k++)
                    {
                        addition *= (int) GameMatrix[i, j, k];
                        solution.Add((i, j, k));
                    }

                    /*
                     * 0: One or more place is not set
                     * 8: All are Cross
                     * 27 All are Zero
                     */
                    if (addition == 8 || addition == 27)
                        return true;
                    Debug.LogWarning($"Pillar Addition Count: {addition}");
                }
            }

            return false;
        }

        private bool CheckForHLevels(out List<(int, int, int)> solution)
        {
            solution = new List<(int, int, int)>(Dimension);
            int addition;
            for (var i = 0; i < Dimension; i++)
            {
                for (var j = 0; j < Dimension; j++)
                {
                    addition = 1;
                    solution.Clear();
                    for (var k = 0; k < Dimension; k++)
                    {
                        addition *= (int) GameMatrix[i, k, j];
                        solution.Add((i, k, j));
                    }

                    /*
                     * 0: One or more place is not set
                     * 8: All are Cross
                     * 27 All are Zero
                     */
                    if (addition == 8 || addition == 27)
                        return true;
                    Debug.LogWarning($"H Level Addition Count: {addition}");
                }
            }

            return false;
        }

        private bool CheckForVLevels(out List<(int, int, int)> solution)
        {
            solution = new List<(int, int, int)>(Dimension);
            int addition;
            for (var i = 0; i < Dimension; i++)
            {
                for (var j = 0; j < Dimension; j++)
                {
                    addition = 1;
                    solution.Clear();
                    for (var k = 0; k < Dimension; k++)
                    {
                        addition *= (int) GameMatrix[k, i, j];
                        solution.Add((k, i, j));
                    }

                    /*
                     * 0: One or more place is not set
                     * 8: All are Cross
                     * 27 All are Zero
                     */
                    if (addition == 8 || addition == 27)
                        return true;
                    Debug.LogWarning($"V Level Addition Count: {addition}");
                }
            }

            return false;
        }

        private bool CheckForReverseDiagonals(out List<(int, int, int)> solution)
        {
            solution = new List<(int, int, int)>(Dimension);
            int addition;
            for (var i = 0; i < Dimension; i++)
            {
                addition = 1;
                solution.Clear();
                for (var j = 0; j < Dimension; j++)
                {
                    addition *= (int) GameMatrix[i, j, j];
                    solution.Add((i, j, j));

                    /*
                     * 0: One or more place is not set
                     * 8: All are Cross
                     * 27 All are Zero
                     */
                    if (addition == 8 || addition == 27)
                        return true;
                    Debug.LogWarning($"R Diagonal Addition Count: {addition}");
                }

                addition = 1;
                solution.Clear();
                for (var j = 0; j < Dimension; j++)
                {
                    addition *= (int) GameMatrix[j, i, j];
                    solution.Add((j, i, j));

                    /*
                     * 0: One or more place is not set
                     * 8: All are Cross
                     * 27 All are Zero
                     */
                    if (addition == 8 || addition == 27)
                        return true;
                    Debug.LogWarning($"R Diagonal Addition Count: {addition}");
                }

                addition = 1;
                solution.Clear();
                for (var j = 0; j < Dimension; j++)
                {
                    addition *= (int) GameMatrix[j, j, i];
                    solution.Add((j, j, i));

                    /*
                     * 0: One or more place is not set
                     * 8: All are Cross
                     * 27 All are Zero
                     */
                    if (addition == 8 || addition == 27)
                        return true;
                    Debug.LogWarning($"R Diagonal Addition Count: {addition}");
                }
            }

            return false;
        }

        private bool CheckForForwardDiagonals(out List<(int, int, int)> solution)
        {
            solution = new List<(int, int, int)>(Dimension);
            int addition;
            for (var i = 0; i < Dimension; i++)
            {
                addition = 1;
                solution.Clear();
                for (var j = 0; j < Dimension; j++)
                {
                    addition *= (int) GameMatrix[i, j, Dimension - j - 1];
                    solution.Add((i, j, Dimension - j - 1));

                    /*
                     * 0: One or more place is not set
                     * 8: All are Cross
                     * 27 All are Zero
                     */
                    if (addition == 8 || addition == 27)
                        return true;
                    Debug.LogWarning($"F Diagonal Addition Count: {addition}");
                }

                addition = 1;
                solution.Clear();
                for (var j = 0; j < Dimension; j++)
                {
                    addition *= (int) GameMatrix[j, i, Dimension - j - 1];
                    solution.Add((j, i, Dimension - j - 1));

                    /*
                     * 0: One or more place is not set
                     * 8: All are Cross
                     * 27 All are Zero
                     */
                    if (addition == 8 || addition == 27)
                        return true;
                    Debug.LogWarning($"F Diagonal Addition Count: {addition}");
                }

                addition = 1;
                solution.Clear();
                for (var j = 0; j < Dimension; j++)
                {
                    addition *= (int) GameMatrix[j, Dimension - j - 1, i];
                    solution.Add((j, Dimension - j - 1, i));

                    /*
                     * 0: One or more place is not set
                     * 8: All are Cross
                     * 27 All are Zero
                     */
                    if (addition == 8 || addition == 27)
                        return true;
                    Debug.LogWarning($"F Diagonal Addition Count: {addition}");
                }
            }

            return false;
        }

        private bool CheckForForwardDiagonalDiagonals(out List<(int, int, int)> solution)
        {
            solution = new List<(int, int, int)>(Dimension);
            var addition = 1;
            solution.Clear();
            for (var i = 0; i < Dimension; i++)
            {
                addition *= (int) GameMatrix[i, i, Dimension - i - 1];
                solution.Add((i, i, Dimension - i - 1));

                /*
                 * 0: One or more place is not set
                 * 8: All are Cross
                 * 27 All are Zero
                 */
                if (addition == 8 || addition == 27)
                    return true;
                Debug.LogWarning($"FD Diagonal Addition Count: {addition}");
            }

            addition = 1;
            solution.Clear();
            for (var i = 0; i < Dimension; i++)

            {
                addition *= (int) GameMatrix[i, Dimension - i - 1, i];
                solution.Add((i, Dimension - i - 1, i));

                /*
                 * 0: One or more place is not set
                 * 8: All are Cross
                 * 27 All are Zero
                 */
                if (addition == 8 || addition == 27)
                    return true;
                Debug.LogWarning($"FD Diagonal Addition Count: {addition}");
            }

            addition = 1;
            solution.Clear();
            for (var i = 0; i < Dimension; i++)
            {
                addition *= (int) GameMatrix[Dimension - i - 1, i, i];
                solution.Add((Dimension - i - 1, i, i));

                /*
                 * 0: One or more place is not set
                 * 8: All are Cross
                 * 27 All are Zero
                 */
                if (addition == 8 || addition == 27)
                    return true;
                Debug.LogWarning($"FD Diagonal Addition Count: {addition}");
            }

            return false;
        }

        private bool CheckForReverseDiagonalDiagonals(out List<(int, int, int)> solution)
        {
            solution = new List<(int, int, int)>(Dimension);
            var addition = 1;
            for (var i = 0; i < Dimension; i++)
            {
                addition *= (int) GameMatrix[i, i, i];
                solution.Add((i, i, i));

                /*
                 * 0: One or more place is not set
                 * 8: All are Cross
                 * 27 All are Zero
                 */
                if (addition == 8 || addition == 27)
                    return true;
                Debug.LogWarning($"RD Diagonal Addition Count: {addition}");
            }

            return false;
        }

        private void StateAction(IState currentState)
        {
            Debug.Log("State Completed".ToColoredString(Color.green));
            _stateTransitionPossible = currentState.IsStateCompleted = true;
        }

        private void PrintGrid()
        {
            var builder = new StringBuilder();

            var debugType = "* ";
            for (var i = 0;
                i < Dimension;
                i++)
            {
                for (var j = 0; j < Dimension; j++)
                {
                    for (var k = 0; k < Dimension; k++)
                    {
                        switch (GameMatrix[i, j, k])
                        {
                            case GameEnums.ShapeType.None:
                                debugType = "* ";
                                break;
                            case GameEnums.ShapeType.Cross:
                                debugType = "X ";
                                break;
                            case GameEnums.ShapeType.Zero:
                                debugType = "0 ";
                                break;
                        }

                        builder.AppendLine($"[{i}{j}{k}] {debugType}");
                    }

                    // builder.AppendLine();
                }

                builder.AppendLine();
            }

            Debug.Log(builder.ToString().Trim().ToColoredString(Color.white));
        }
    }
}