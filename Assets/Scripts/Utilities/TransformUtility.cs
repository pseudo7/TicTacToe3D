using System;
using System.Collections;
using UnityEngine;

namespace TicTacToe3D.Utilities
{
    public static class TransformUtility
    {
        private static MonoBehaviour _translator;

        public static void MoveToPosition(this Transform objectTransform, Vector3 targetPosition, float duration,
            Action onCompleteAction = null)
        {
            ValidateTranslator();
            _translator.StartCoroutine(MoveRoutine(objectTransform, targetPosition, duration, onCompleteAction));
        }


        private static IEnumerator MoveRoutine(Transform objectTransform, Vector3 targetPosition, float duration,
            Action onCompleteAction)
        {
            var step = (targetPosition - objectTransform.position).magnitude / duration;
            while ((targetPosition - objectTransform.position).sqrMagnitude > .1F)
            {
                objectTransform.position =
                    Vector3.MoveTowards(objectTransform.position, targetPosition, step * Time.deltaTime);
                yield return null;
            }

            objectTransform.position = targetPosition;
            onCompleteAction?.Invoke();
        }

        public static void MoveToPosition(this Transform objectTransform, Transform targetTransform, float duration,
            Action onCompleteAction = null)
        {
            ValidateTranslator();
            _translator.StartCoroutine(MoveRoutine(objectTransform, targetTransform, duration, onCompleteAction));
        }


        private static IEnumerator MoveRoutine(Transform objectTransform, Transform targetTransform, float duration,
            Action onCompleteAction)
        {
            var step = (targetTransform.position - objectTransform.position).magnitude / duration;
            while ((targetTransform.position - objectTransform.position).sqrMagnitude > .1F)
            {
                objectTransform.position =
                    Vector3.MoveTowards(objectTransform.position, targetTransform.position, step * Time.deltaTime);
                yield return null;
            }

            objectTransform.position = targetTransform.position;
            onCompleteAction?.Invoke();
        }

        private static void ValidateTranslator()
        {
            if (!_translator)
                _translator = new GameObject("TransformUtility").AddComponent<Translator>();
        }
    }
}