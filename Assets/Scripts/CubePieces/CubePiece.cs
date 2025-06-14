using UnityEngine;

namespace Assets.Scripts
{
    public class CubePiece : MonoBehaviour
    {
        public Vector3Int Index;

        public Vector3Int GridPos =>
            new(Mathf.RoundToInt(transform.localPosition.x) + 1,
                Mathf.RoundToInt(transform.localPosition.y) + 1,
                Mathf.RoundToInt(transform.localPosition.z) + 1);
    }
}