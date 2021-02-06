using System.Collections;
using UnityEngine;

namespace TicTacToe3D.Utilities
{
    public static class TransformUtility
    {
        private static MonoBehaviour _translator;

        public static void MoveToPosition(this Transform objectTransform, Vector3 targetPosition, float duration)
        {
            ValidateTranslator();
            _translator.StartCoroutine(MoveRoutine(objectTransform, targetPosition, duration));
        }

        private static IEnumerator MoveRoutine(Transform objectTransform, Vector3 targetPosition, float duration)
        {
            var step = (targetPosition - objectTransform.position).magnitude / duration;
            while ((targetPosition - objectTransform.position).sqrMagnitude > .1F)
            {
                objectTransform.position =
                    Vector3.MoveTowards(objectTransform.position, targetPosition, step * Time.deltaTime);
                yield return null;
            }

            objectTransform.position = targetPosition;
        }

        private static void ValidateTranslator()
        {
            if (!_translator)
                _translator = new GameObject("TransformUtility").AddComponent<Translator>();
        }
    }
}