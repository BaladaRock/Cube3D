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

//using UnityEngine;

//namespace Assets.Scripts
//{
//    public class CubeGenerator : MonoBehaviour
//    {
//        public GameObject CubePrefab;
//        public float Spacing = 1.05f; // distance between the pieces
//        public CubeColorizer Colorizer;

//        private void Start()
//        {
//            GenerateRubikCube();
//        }

//        private void GenerateRubikCube()
//        {
//            // Iterate through the 3󫢫 grid
//            for (var x = -1; x <= 1; x++)
//            for (var y = -1; y <= 1; y++)
//            for (var z = -1; z <= 1; z++)
//            {
//                // Instantiate the piece
//                var position = new Vector3(x * Spacing, y * Spacing, z * Spacing);
//                var cubePiece = Instantiate(CubePrefab, position, Quaternion.identity, transform);
//                cubePiece.name = $"Piece{x}_{y}_{z}";

//                // Pass grid coordinates to the shader (_PiecePos)
//                var component = cubePiece.GetComponent<MeshRenderer>();
//                if (component != null)
//                {
//                    var block = new MaterialPropertyBlock();
//                    block.SetVector("_PiecePos", new Vector3(x, y, z));
//                    component.SetPropertyBlock(block);
//                    Debug.Log($"Set _PiecePos for {cubePiece.name}: {x},{y},{z}");
//                }

//                // Apply material only on outer layer faces
//                if (Colorizer != null)
//                {
//                    Colorizer.ApplyFaceColors(cubePiece, x, y, z);
//                }
//            }
//        }
//    }
//}