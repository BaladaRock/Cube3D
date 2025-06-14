
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

            // use this index to determine if a piece face is external or internal
            // its value varies between [0, 2]
            float3 _PieceIdx; 

            v2f vert (appdata v)
            {
                v2f o;
                o.pos      = UnityObjectToClipPos(v.vertex);
                o.normalOS = normalize(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                const float TH = 0.95;
                float3 n = normalize(i.normalOS);

                // top / bottom
                if ( n.y >  TH && _PieceIdx.y > 1.5) return _YellowColor; // +Y
                if ( n.y < -TH && _PieceIdx.y < 0.5) return _WhiteColor;  // -Y

                // front / back
                if ( n.z < -TH && _PieceIdx.z < 0.5) return _RedColor;     // +Z
                if ( n.z >  TH && _PieceIdx.z > 1.5) return _OrangeColor;  // -Z

                // right / left
                if ( n.x >  TH && _PieceIdx.x > 1.5) return _GreenColor;  // +X
                if ( n.x < -TH && _PieceIdx.x < 0.5) return _BlueColor;   // -X

                return _InteriorColor;                                    // muchii/interior
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
