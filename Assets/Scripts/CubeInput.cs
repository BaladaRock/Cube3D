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

            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition),
                        out RaycastHit hit, 100f))
                {
                    var cubelet = hit.transform.GetComponent<CubePiece>();
                    if (cubelet == null) return;
                    DecideMoveFromHit(cubelet, hit.normal);
                }
            }
        }

        void DecideMoveFromHit(CubePiece c, Vector3 hitNormal)
        {
            hitNormal = transform.InverseTransformDirection(hitNormal);

            if (Mathf.Abs(hitNormal.x) > 0.5f)
            {
                int layer = c.Index.x;
                bool cw = hitNormal.x < 0;             
                manager.EnqueueRotation(Axis.X, layer, cw);
            }
            else if (Mathf.Abs(hitNormal.y) > 0.5f)
            {
                int layer = c.Index.y;
                bool cw = hitNormal.y < 0;
                manager.EnqueueRotation(Axis.Y, layer, cw);
            }
            else
            {
                int layer = c.Index.z;
                bool cw = hitNormal.z < 0;
                manager.EnqueueRotation(Axis.Z, layer, cw);
            }
        }
    }
}
