using UnityEngine;

namespace Assets.Scripts
{
    public class CubeInput : MonoBehaviour
    {
        CubeManager manager;
        Camera cam;

        void Awake()
        {
            manager = GetComponent<CubeManager>();
            cam = Camera.main;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F)) manager.EnqueueRotation(Axis.Z, 2, true); 
            if (Input.GetKeyDown(KeyCode.U)) manager.EnqueueRotation(Axis.Y, 2, true); 
            if (Input.GetKeyDown(KeyCode.R)) manager.EnqueueRotation(Axis.X, 2, false);

            // arrow controls for rotation
            if (Input.GetKeyDown(KeyCode.RightArrow))
                manager.EnqueueRotation(Axis.Z, 2, true);
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                manager.EnqueueRotation(Axis.Z, 2, false);
            if (Input.GetKeyDown(KeyCode.UpArrow))
                manager.EnqueueRotation(Axis.Y, 2, true);
            if (Input.GetKeyDown(KeyCode.DownArrow))
                manager.EnqueueRotation(Axis.Y, 2, false);

            // WASD controls for rotation
            if (Input.GetKeyDown(KeyCode.A))
                manager.EnqueueRotation(Axis.X, 0, false);
            if (Input.GetKeyDown(KeyCode.D))
                manager.EnqueueRotation(Axis.X, 2, true);

            if (!Input.GetMouseButtonDown(0)) return;
            if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition),
                    out var hit, 100f)) return;

            var cubelet = hit.transform.GetComponent<CubePiece>();
            if (cubelet == null) return;

            DecideMoveFromHit(cubelet, hit.normal);
        }

        private void DecideMoveFromHit(CubePiece c, Vector3 hitNormal)
        {
            hitNormal = transform.InverseTransformDirection(hitNormal);

            if (Mathf.Abs(hitNormal.x) > 0.5f)
            {
                var layer = c.Index.x;
                var cw = hitNormal.x < 0;             
                manager.EnqueueRotation(Axis.X, layer, cw);
            }
            else if (Mathf.Abs(hitNormal.y) > 0.5f)
            {
                var layer = c.Index.y;
                var cw = hitNormal.y < 0;
                manager.EnqueueRotation(Axis.Y, layer, cw);
            }
            else
            {
                var layer = c.Index.z;
                var cw = hitNormal.z < 0;
                manager.EnqueueRotation(Axis.Z, layer, cw);
            }
        }
    }
}
