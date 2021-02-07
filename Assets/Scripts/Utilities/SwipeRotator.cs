using UnityEngine;

namespace TicTacToe3D.Utilities
{
    [DisallowMultipleComponent]
    public class SwipeRotator : MonoBehaviour
    {
        [SerializeField] private GameEnums.AxisType rotateAround;
        [SerializeField] private Rigidbody rotatingBody;
        [SerializeField] [Range(1, 3)] private float stiffness = 2F;

        private float _mouseDelta;
        private Vector3 _rotationAxis;
        private const string MouseXAxis = "Mouse X";
        private const string MouseYAxis = "Mouse Y";

        public void KillInertia() =>
            rotatingBody.angularVelocity = Vector3.zero;

        private void Awake()
        {
            rotatingBody.useGravity = false;
            rotatingBody.angularDrag = stiffness;
            var constraints = RigidbodyConstraints.FreezeAll;
            switch (rotateAround)
            {
                case GameEnums.AxisType.X:
                    constraints &= ~RigidbodyConstraints.FreezeRotationX;
                    _rotationAxis = Vector3.right;
                    break;
                case GameEnums.AxisType.Y:
                    constraints &= ~RigidbodyConstraints.FreezeRotationY;
                    _rotationAxis = Vector3.up;
                    break;
                case GameEnums.AxisType.Z:
                    constraints &= ~RigidbodyConstraints.FreezeRotationZ;
                    _rotationAxis = Vector3.forward;
                    break;
            }

            rotatingBody.constraints = constraints;
        }

        private void OnMouseDrag()
        {
            _mouseDelta = (Input.GetAxis(MouseXAxis) + Input.GetAxis(MouseYAxis)) * -1;
            rotatingBody.AddTorque(_rotationAxis * _mouseDelta, ForceMode.Impulse);
        }
    }
}