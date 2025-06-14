using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public enum Axis { X, Y, Z }

    public class CubeManager : MonoBehaviour
    {
        public float rotationTime = 0.25f;
        public float step = 1.05f; // this should be identical to the distance between the pieces
        public AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

        Transform pivot;                        
        CubePiece[,,] grid = new CubePiece[3, 3, 3];
        Queue<IEnumerator> moveQueue = new Queue<IEnumerator>();
        bool running;

        void Awake()
        {
            pivot = new GameObject("RotationPivot").transform;
            pivot.SetParent(transform, false); // location (0,0,0)

            // 2D fill in the 2D matrix with CubePiece components
            foreach (CubePiece c in GetComponentsInChildren<CubePiece>())
            {
                Vector3Int idx = c.GridPos;
                c.Index = idx;
                grid[idx.x, idx.y, idx.z] = c;
            }
        }

        void Update() //animations queue
        {
            if (!running && moveQueue.Count > 0)
                StartCoroutine(moveQueue.Dequeue());
        }

        // public interface
        public void EnqueueRotation(Axis axis, int layerIdx, bool clockwise)
        {
            Debug.Log($"QUEUE  axis={axis} layer={layerIdx} cw={clockwise}");
            moveQueue.Enqueue(RotateRoutine(axis, layerIdx, clockwise));
        }

        // 3️D corutine
        IEnumerator RotateRoutine(Axis axis, int layer, bool cw)
        {
            running = true;

            Debug.Log($"START  axis={axis} layer={layer} cw={cw}");
            // select the cube pieces contained by the layer
            var affected = new List<CubePiece>();
            foreach (CubePiece c in grid)
            {
                if (c == null)
                {
                    continue;
                }
                if ((axis == Axis.X && c.Index.x == layer) ||
                    (axis == Axis.Y && c.Index.y == layer) ||
                    (axis == Axis.Z && c.Index.z == layer))
                {
                    affected.Add(c);
                }
            }

            // pivot fixing
            pivot.localPosition = LayerCenter(axis, layer);
            pivot.localRotation = Quaternion.identity;

            // reparent
            foreach (var cb in affected) cb.transform.SetParent(pivot, true);

            // animation
            Quaternion start = Quaternion.identity;
            Quaternion end = Quaternion.AngleAxis((cw ? -90 : 90), AxisVector(axis));

            float t = 0;
            while (t < 1f)
            {
                t += Time.deltaTime / rotationTime;
                pivot.localRotation = Quaternion.Slerp(start, end, ease.Evaluate(t));
                yield return null;
            }
            pivot.localRotation = end;

            // restore parenting & actualize
            foreach (var cb in affected)
            {
                cb.transform.SetParent(transform, true);
                cb.Index = cb.GridPos;
                grid[cb.Index.x, cb.Index.y, cb.Index.z] = cb;
            }

            running = false;
        }

        // helpers
        Vector3 AxisVector(Axis a) =>
            a == Axis.X ? Vector3.right : a == Axis.Y ? Vector3.up : Vector3.forward;

        Vector3 LayerCenter(Axis a, int l)
        {
            float off = (l - 1) * step;            // −1,0,+1
            return a == Axis.X ? new Vector3(off, 0, 0) :
                a == Axis.Y ? new Vector3(0, off, 0) :
                new Vector3(0, 0, off);
        }
    }
}