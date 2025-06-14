using UnityEngine;

namespace Assets.Scripts
{
    public class FaceDragInput : MonoBehaviour
    {
        private Camera _cam;
        private Vector2 _dragStart;
        private Vector3 _hitNormalOs; // normal in Object Space
        private CubeManager _manager;
        private CubePiece _piece;

        private void Awake()
        {
            _cam = Camera.main;
            _manager = GetComponentInParent<CubeManager>();
            _piece = GetComponent<CubePiece>();
        }

        private void OnMouseDown()
        {
            _dragStart = Input.mousePosition;

            // save the normal of the touched face
            if (Physics.Raycast(_cam.ScreenPointToRay(_dragStart), out var hit))
                _hitNormalOs = transform.parent.InverseTransformDirection(hit.normal).RoundAxis();
        }

        private void OnMouseUp()
        {
            var delta = (Vector2)Input.mousePosition - _dragStart;

            if (delta.magnitude < 10f) return; // ignore short clicks

            var horiz = Mathf.Abs(delta.x) > Mathf.Abs(delta.y);

            Axis axis;
            int layer;
            bool cw;

            // the normal will decide the plane of the movement
            if (horiz) // drag left -> right
            {
                if (Mathf.Abs(_hitNormalOs.y) > 0.5f)
                {
                    axis = Axis.Y;
                    layer = _piece.Index.y;
                    cw = delta.x < 0;
                }
                else
                {
                    axis = Axis.Z;
                    layer = _piece.Index.z;
                    cw = delta.x > 0;
                } // F/B
            }
            else // drag up -> down
            {
                if (Mathf.Abs(_hitNormalOs.x) > 0.5f)
                {
                    axis = Axis.X;
                    layer = _piece.Index.x;
                    cw = delta.y > 0;
                }
                else
                {
                    axis = Axis.Z;
                    layer = _piece.Index.z;
                    cw = delta.y < 0;
                } // F/B
            }

            Debug.Log($"ROTATE axis={axis} layer={layer} cw={cw}");
            _manager.EnqueueRotation(axis, layer, cw);
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