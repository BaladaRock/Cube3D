using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class CubeHelpers
    {
        // helpers
        public static Vector3 AxisVector(Axis a) =>
            a switch
            {
                Axis.X => Vector3.right,
                Axis.Y => Vector3.up,
                _ => Vector3.forward
            };

        public static Vector3 LayerCenter(Axis a, int l, float step)
        {
            var off = (l - 1) * step;

            return a switch
            {
                Axis.X => new Vector3(off, 0, 0),
                Axis.Y => new Vector3(0, off, 0),
                _ => new Vector3(0, 0, off)
            };
        }

    }
}
