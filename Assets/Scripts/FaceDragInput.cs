using UnityEngine;

namespace Assets.Scripts
{
    public class FaceDragInput : MonoBehaviour
    {
        private Camera cam;
        private Vector2 dragStart;
        private Vector3 hitNormalOS; // normal în Object Space
        private CubeManager manager;
        private CubePiece piece;

        private void Awake()
        {
            cam = Camera.main;
            manager = GetComponentInParent<CubeManager>();
            piece = GetComponent<CubePiece>();
        }

        private void OnMouseDown()
        {
            dragStart = Input.mousePosition;

            // save the normal of the touched face
            if (Physics.Raycast(cam.ScreenPointToRay(dragStart), out var hit))
                hitNormalOS = transform.parent.InverseTransformDirection(hit.normal).RoundAxis();
        }

        private void OnMouseUp()
        {
            var delta = (Vector2)Input.mousePosition - dragStart;

            if (delta.magnitude < 10f) return; // igonre short clicks

            var horiz = Mathf.Abs(delta.x) > Mathf.Abs(delta.y);

            Axis axis;
            int layer;
            bool cw;

            // normal will decide the plane of the movement
            if (horiz) // drag left -> right
            {
                if (Mathf.Abs(hitNormalOS.y) > 0.5f)
                {
                    axis = Axis.Y;
                    layer = piece.Index.y;
                    cw = delta.x < 0;
                }
                else
                {
                    axis = Axis.Z;
                    layer = piece.Index.z;
                    cw = delta.x > 0;
                } // F/B
            }
            else // drag up -> down
            {
                if (Mathf.Abs(hitNormalOS.x) > 0.5f)
                {
                    axis = Axis.X;
                    layer = piece.Index.x;
                    cw = delta.y > 0;
                }
                else
                {
                    axis = Axis.Z;
                    layer = piece.Index.z;
                    cw = delta.y < 0;
                } // F/B
            }

            manager.EnqueueRotation(axis, layer, cw);
        }
    }

    internal static class VecExt // helper
    {
        public static Vector3 RoundAxis(this Vector3 v)
        {
            if (Mathf.Abs(v.x) > Mathf.Abs(v.y) && Mathf.Abs(v.x) > Mathf.Abs(v.z))
                return new Vector3(Mathf.Sign(v.x), 0, 0);

            return Mathf.Abs(v.y) > Mathf.Abs(v.z)
                ? new Vector3(0, Mathf.Sign(v.y), 0)
                : new Vector3(0, 0, Mathf.Sign(v.z));
        }
    }
}