using UnityEngine;

namespace Assets.Scripts
{
    public class CubeRotator : MonoBehaviour
    {
        private Vector3 _lastMousePosition;
        public float RotationSpeed = 5f;

        private void OnMouseDown()
        {
            _lastMousePosition = Input.mousePosition;
        }

        private void OnMouseDrag()
        {
            var delta = Input.mousePosition - _lastMousePosition;

            var rotX = delta.y * RotationSpeed;
            var rotY = -delta.x * RotationSpeed;

            if (!Input.GetKey(KeyCode.LeftAlt))
            {
                return;
            }

            if (transform.parent != null)
            {
                transform.parent.Rotate(Vector3.up, rotY, Space.World);
                transform.parent.Rotate(Vector3.right, rotX, Space.World);
            }

            _lastMousePosition = Input.mousePosition;
        }
    }
}