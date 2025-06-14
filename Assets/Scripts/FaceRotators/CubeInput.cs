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
            if (Input.GetKeyDown(KeyCode.F)) _manager.EnqueueRotation(Axis.Z, 2, true); 
            if (Input.GetKeyDown(KeyCode.U)) _manager.EnqueueRotation(Axis.Y, 2, true); 
            if (Input.GetKeyDown(KeyCode.R)) _manager.EnqueueRotation(Axis.X, 2, false);

            // arrow controls for rotation
            if (Input.GetKeyDown(KeyCode.RightArrow))
                _manager.EnqueueRotation(Axis.Z, 2, true);
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                _manager.EnqueueRotation(Axis.Z, 2, false);
            if (Input.GetKeyDown(KeyCode.UpArrow))
                _manager.EnqueueRotation(Axis.Y, 2, true);
            if (Input.GetKeyDown(KeyCode.DownArrow))
                _manager.EnqueueRotation(Axis.Y, 2, false);

            // WASD controls for rotation
            if (Input.GetKeyDown(KeyCode.A))
                _manager.EnqueueRotation(Axis.X, 0, false);
            if (Input.GetKeyDown(KeyCode.D))
                _manager.EnqueueRotation(Axis.X, 2, true);

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
