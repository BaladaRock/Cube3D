using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public enum Axis { X, Y, Z }

    public class CubeManager : MonoBehaviour
    {
        public float RotationTime = 0.25f;
        public float Step = 1.05f; // this should be identical to the distance between the pieces
        public AnimationCurve Ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private Transform _pivot;
        private readonly CubePiece[,,] _grid = new CubePiece[3, 3, 3];
        private readonly Queue<IEnumerator> _moveQueue = new Queue<IEnumerator>();
        private bool _running;


        private void Start()
        {
            _pivot = new GameObject("RotationPivot").transform;
            _pivot.SetParent(transform, false); // location (0,0,0)

            // 2D fill in the 2D matrix with CubePiece components
            foreach (var c in GetComponentsInChildren<CubePiece>())
            {
                var idx = c.GridPos;
                c.Index = idx;
                _grid[idx.x, idx.y, idx.z] = c;
            }
        }

        private void Update() //animations queue
        {
            if (!_running && _moveQueue.Count > 0)
                StartCoroutine(_moveQueue.Dequeue());
        }

        // public interface
        public void EnqueueRotation(Axis axis, int layerIdx, bool clockwise)
        {
            Debug.Log($"QUEUE  axis={axis} layer={layerIdx} cw={clockwise}");
            _moveQueue.Enqueue(RotateRoutine(axis, layerIdx, clockwise));
        }

        // 3️D coroutine
        private IEnumerator RotateRoutine(Axis axis, int layer, bool cw)
        {
            _running = true;

            Debug.Log($"START  axis={axis} layer={layer} cw={cw}");
            // select the cube pieces contained by the layer
            var affected = new List<CubePiece>();
            foreach (var c in _grid)
            {
                if (c == null)
                {
                    continue;
                }

                Debug.Log(c.name + "  idx=" + c.Index);

                if ((axis == Axis.X && c.Index.x == layer) ||
                    (axis == Axis.Y && c.Index.y == layer) ||
                    (axis == Axis.Z && c.Index.z == layer))
                {
                    affected.Add(c);
                }

                Debug.Log($"affected = {affected.Count}");
            }

            // pivot fixing
            _pivot.localPosition = LayerCenter(axis, layer);
            _pivot.localRotation = Quaternion.identity;

            // reparent
            foreach (var cb in affected) cb.transform.SetParent(_pivot, true);

            // animation
            var start = Quaternion.identity;
            var end = Quaternion.AngleAxis((cw ? -90 : 90), AxisVector(axis));

            float t = 0;
            while (t < 1f)
            {
                t += Time.deltaTime / RotationTime;
                _pivot.localRotation = Quaternion.Slerp(start, end, Ease.Evaluate(t));
                yield return null;
            }
            _pivot.localRotation = end;

            // restore parenting & actualize
            foreach (var cb in affected)
            {
                cb.transform.SetParent(transform, true);
                cb.Index = cb.GridPos;
                _grid[cb.Index.x, cb.Index.y, cb.Index.z] = cb;
            }

            _running = false;
        }

        // helpers
        private static Vector3 AxisVector(Axis a) =>
            a switch
            {
                Axis.X => Vector3.right,
                Axis.Y => Vector3.up,
                _ => Vector3.forward
            };

        private Vector3 LayerCenter(Axis a, int l)
        {
            var off = (l - 1) * Step;            // −1,0,+1
            return a == Axis.X ? new Vector3(off, 0, 0) :
                a == Axis.Y ? new Vector3(0, off, 0) :
                new Vector3(0, 0, off);
        }
    }
}