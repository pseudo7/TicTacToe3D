using UnityEngine;

namespace TicTacToe3D.Services
{
    public abstract class ServiceBase : MonoBehaviour
    {
        protected abstract void RegisterService();

        protected virtual void Awake() =>
            RegisterService();
    }
}