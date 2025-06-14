using UnityEngine;

namespace Assets.Scripts
{
    public class CubeColorizer : MonoBehaviour
    {
        public Material FaceShaderMaterial;

        public void ApplyFaceColors(GameObject piece, int x, int y, int z)
        {
            var component = piece.GetComponent<MeshRenderer>();
            if (component == null || FaceShaderMaterial == null)
            {
                return;
            }

            // For interior faces, we don't apply the material
            var isOnOuterLayer = Mathf.Abs(x) == 1 || Mathf.Abs(y) == 1 || Mathf.Abs(z) == 1;
            if (!isOnOuterLayer)
            {
                Debug.Log($"Inner piece: {piece.name} at ({x},{y},{z})");

                return;
            }

            // Use the same material for all faces
            var mats = new Material[6];
            for (var i = 0; i < 6; i++)
                mats[i] = FaceShaderMaterial;

            component.materials = mats;
        }
    }
}