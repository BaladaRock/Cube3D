
Shader "Custom/RubikSixFaceColor"
{
    Properties
    {
        _RedColor      ("Front  (+Z) — Red"   , Color) = (1,0,0,1)
        _BlueColor     ("Left   (-X) — Blue"  , Color) = (0,0,1,1)
        _GreenColor    ("Right  (+X) — Green" , Color) = (0,1,0,1)
        _YellowColor   ("Top    (+Y) — Yellow", Color) = (1,1,0,1)
        _WhiteColor    ("Bottom (-Y) — White" , Color) = (1,1,1,1)
        _OrangeColor   ("Back   (-Z) — Orange", Color) = (1,0.5,0,1)
        _InteriorColor ("Interior faces"      , Color) = (0,0,0,1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos      : SV_POSITION;
                float3 normalOS : TEXCOORD0;
            };

            fixed4 _RedColor;
            fixed4 _BlueColor;
            fixed4 _GreenColor;
            fixed4 _YellowColor;
            fixed4 _WhiteColor;
            fixed4 _OrangeColor;
            fixed4 _InteriorColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos      = UnityObjectToClipPos(v.vertex);
                o.normalOS = normalize(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                const float THRESHOLD = 0.95;

                float3 n = normalize(i.normalOS);

                if (n.y >  THRESHOLD)      return _YellowColor;   // top (+Y)
                else if (n.y < -THRESHOLD) return _WhiteColor;    // bottom (-Y)
                else if (n.z >  THRESHOLD) return _RedColor;      // front (+Z)
                else if (n.z < -THRESHOLD) return _OrangeColor;   // back (-Z)
                else if (n.x >  THRESHOLD) return _GreenColor;    // right (+X)
                else if (n.x < -THRESHOLD) return _BlueColor;     // left  (-X)

                return _InteriorColor;  // anything else (edges / interior)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
