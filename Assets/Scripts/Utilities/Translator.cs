using UnityEngine;

namespace TicTacToe3D.Utilities
{
    public class Translator : MonoBehaviour
    {
        private void Awake() =>
            DontDestroyOnLoad(gameObject);
    }
}