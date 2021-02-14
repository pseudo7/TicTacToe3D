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
        //TODO: Add Game Finish Logic
        //TODO: Add Delay Highlight (Check if in Mobile it is shown)
        //TODO: Add Single Pillar & Its pillar to have high light property as a single unit
        protected internal override GameEnums.ShapeType[,,] GameMatrix { get; set; }
        protected internal override PillarBase[] Pillars { get; set; }

        private bool _stateTransitionPossible;
        private int _currentShapeCount;
        private int _currentMaterialCount;

        protected internal override SwipeRotator CurrentSwipeRotator { get; set; }

        protected override void SetupGame()
        {
            GameMatrix = new GameEnums.ShapeType [3, 3, 3];
            Pillars = new PillarBase[9];
            GameStateMachine = gameObject.AddComponent<TurnBasedStateMachine>();
            GameStateMachine.Initialisation(new NextTurnState(StateAction));
            CurrentSwipeRotator = FindObjectOfType<SwipeRotator>();
        }

        protected override IStateMachine GameStateMachine { get; set; }

        protected internal override void HighLightPillar(int pillarIndex)
        {
            // CurrentSwipeRotator.KillInertia();
            var count = Pillars.Length;
            while (count-- > 0)
                Pillars[count]
                    .SetMaterial(Bootstrap.BootstrapInstance.GetService<ResourcesService>().LightWoodMatFaded);
            Pillars[pillarIndex].SetMaterial(Bootstrap.BootstrapInstance.GetService<ResourcesService>().LightWoodMat);
        }

        protected internal override void UnHighLightPillar(int pillarIndex)
        {
            var count = Pillars.Length;
            while (count-- > 0)
                Pillars[count].SetMaterial(Bootstrap.BootstrapInstance.GetService<ResourcesService>().LightWoodMat);
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
            if (!CheckForPillars(out var solution)) return false;
            solution.ForEach(tuple => Debug.Log(tuple.ToString().ToColoredString(Color.green)));
            // solution.ForEach(x =>
            // {
            //     var pillar = Pillars[x.Item1 * 3 + x.Item2];
            //     pillar.PillarSegments[x.Item3].CurrentShape.gameObject.SetActive(false);
            // });
            return true;
        }

        private bool CheckForPillars(out List<(int, int, int)> solution)
        {
            solution = new List<(int, int, int)>(3);
            int addition;
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    addition = 1;
                    solution.Clear();
                    for (var k = 0; k < 3; k++)
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
                    Debug.LogWarning($"Addition Count: {addition}");
                }
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

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    for (var k = 0; k < 3; k++)
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