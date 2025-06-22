using UnityEngine;

namespace Assets.Scripts
{
    public class RubiksCube : MonoBehaviour
    {
        [SerializeField] private GameObject _piecePrefab;

        public RubiksCube(GameObject piecePrefab)
        {
            this._piecePrefab = piecePrefab;
        }

        private const float Step = 1.04f;

        private void Awake()
        {
            for (var x = -1; x <= 1; x++)
            for (var y = -1; y <= 1; y++)
            for (var z = -1; z <= 1; z++)
            {
                var go = Instantiate(_piecePrefab, transform);
                go.transform.localPosition = new Vector3(x * Step, y * Step, z * Step);

                var piece = go.GetComponent<CubePiece>();
                piece.Index = new Vector3Int(x + 1, y + 1, z + 1);
            }
        }
    }
}