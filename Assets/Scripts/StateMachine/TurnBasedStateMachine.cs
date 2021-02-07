using System.Collections;
using TicTacToe3D.Utilities;
using UnityEngine;

namespace TicTacToe3D.StateMachine
{
    public class TurnBasedStateMachine : MonoBehaviour, IStateMachine
    {
        private bool _isActive;
        private bool _isStateUpdated;
        private IState _currentState;
        private Coroutine _stateCoroutine;

        public void Initialisation(IState initialState = null)
        {
            _isActive = true;
            _currentState = initialState;
            _stateCoroutine = StartCoroutine(StateMachineRoutine());
            _isStateUpdated = true;
        }

        public void UpdateState(IState nextState)
        {
            if (!_currentState.IsStateCompleted)
            {
                Debug.Log("Previous State Not Completed".ToColoredString(Color.red));
                return;
            }

            _currentState = nextState;
            _isStateUpdated = true;
        }

        public void Finalisation()
        {
            _isActive = false;
            if (_stateCoroutine != null)
                StopCoroutine(_stateCoroutine);
        }

        private IEnumerator StateMachineRoutine()
        {
            while (_isActive)
            {
                Debug.Log("Cycle Started".ToColoredString(Color.cyan));
                yield return new WaitUntil(() => _isStateUpdated);
                _currentState?.StateAction?.Invoke(_currentState);
                _isStateUpdated = false;
                yield return new WaitUntil(() => _currentState.IsStateCompleted);
                Debug.Log("Cycle Completed".ToColoredString(Color.magenta));
            }
        }
    }
}