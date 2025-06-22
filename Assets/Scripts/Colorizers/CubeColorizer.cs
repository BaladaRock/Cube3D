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