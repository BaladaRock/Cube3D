using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(MeshRenderer))]
    public class CubeColorizer : MonoBehaviour
    {
        private static readonly int _pieceIdx = Shader.PropertyToID("_PieceIdx");

        private CubePiece _piece;
        private MeshRenderer _rend;

        private void Start()  
        {
            _piece = GetComponent<CubePiece>();
            _rend = GetComponent<MeshRenderer>();
            Apply();
        }

        public void Apply()
        {
            var block = new MaterialPropertyBlock();
            if (_piece == null)
            {
                return;
            }

            block.SetVector(_pieceIdx, new Vector4(_piece.Index.x, _piece.Index.y, _piece.Index.z, 0));
            GetComponent<MeshRenderer>().SetPropertyBlock(block);
        }
    }
}



//public Material FaceShaderMaterial;

//public void ApplyFaceColors(GameObject piece, int x, int y, int z)
//{
//    var component = piece.GetComponent<MeshRenderer>();
//    if (component == null || FaceShaderMaterial == null)
//    {
//        return;
//    }

//    // For interior faces, we don't apply the material
//    var isOnOuterLayer = Mathf.Abs(x) == 1 || Mathf.Abs(y) == 1 || Mathf.Abs(z) == 1;
//    if (!isOnOuterLayer)
//    {
//        Debug.Log($"Inner piece: {piece.name} at ({x},{y},{z})");

//        return;
//    }

//    // Use the same material for all faces
//    var mats = new Material[6];
//    for (var i = 0; i < 6; i++)
//        mats[i] = FaceShaderMaterial;

//    component.materials = mats;
//}