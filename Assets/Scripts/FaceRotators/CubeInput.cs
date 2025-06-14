using Assets.Scripts.Helpers;
using UnityEngine;

namespace Assets.Scripts
{
    public class CubeInput : MonoBehaviour
    {
        private CubeManager _manager;
        private Camera _cam;

        private void Awake()
        {
            _manager = GetComponent<CubeManager>();
            _cam = Camera.main;
        }

        private void Update()
        {
            // Keyboard controls for rotations

            // HTM FForward, BBack, UUp, DDown, RRight, LLeft
            if (Input.GetKeyDown(KeyCode.F))
                _manager.RotateByWorldDirection(-Camera.main.transform.forward, false);

            if (Input.GetKeyDown(KeyCode.B))
                _manager.RotateByWorldDirection(Camera.main.transform.forward, false);

            if (Input.GetKeyDown(KeyCode.U))
                _manager.RotateByWorldDirection(Camera.main.transform.up, false);

            if (Input.GetKeyDown(KeyCode.D))
                _manager.RotateByWorldDirection(-Camera.main.transform.up, true);

            if (Input.GetKeyDown(KeyCode.R))
                _manager.RotateByWorldDirection(Camera.main.transform.right, false);

            if (Input.GetKeyDown(KeyCode.L))
                _manager.RotateByWorldDirection(-Camera.main.transform.right, false);

            // Arrow keys for rotations
            if (Input.GetKeyDown(KeyCode.RightArrow))
                _manager.RotateByWorldDirection(Camera.main.transform.right, true);

            if (Input.GetKeyDown(KeyCode.LeftArrow))
                _manager.RotateByWorldDirection(-Camera.main.transform.right, true);

            if (Input.GetKeyDown(KeyCode.UpArrow))
                _manager.RotateByWorldDirection(Camera.main.transform.up, true);

            if (Input.GetKeyDown(KeyCode.DownArrow))
                _manager.RotateByWorldDirection(-Camera.main.transform.up, true);

            // Mouse controls for rotations
            if (!Input.GetMouseButtonDown(0)) return;
            if (!Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition),
                    out var hit, 100f)) return;

            var cubePiece = hit.transform.GetComponent<CubePiece>();
            if (cubePiece == null) return;

            DecideMoveFromHit(cubePiece, hit.normal);
        }

        private void DecideMoveFromHit(CubePiece c, Vector3 hitNormal)
        {
            hitNormal = transform.InverseTransformDirection(hitNormal);

            if (Mathf.Abs(hitNormal.x) > 0.5f)
            {
                var layer = c.Index.x;
                var cw = hitNormal.x < 0;             
                _manager.EnqueueRotation(Axis.X, layer, cw);
            }
            else if (Mathf.Abs(hitNormal.y) > 0.5f)
            {
                var layer = c.Index.y;
                var cw = hitNormal.y < 0;
                _manager.EnqueueRotation(Axis.Y, layer, cw);
            }
            else
            {
                var layer = c.Index.z;
                var cw = hitNormal.z < 0;
                _manager.EnqueueRotation(Axis.Z, layer, cw);
            }
        }
    }
}
