Shader "examples/week 2/shapes"
{
    Properties
    {
        _spaceBlend ("space blend", Range(0,1)) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            #define TAU 6.28318530718

            uniform float _spaceBlend;

            float4 frag (Interpolators i) : SV_Target
            {
                float2 iuv = i.uv*2-1;

                float2 polarUV = float2(atan2(iuv.y, iuv.x), length(iuv));
                polarUV.x = polarUV.x / 6.28 + 0.5;
                float2 uv = lerp(iuv, polarUV, _spaceBlend);

                float shape = 0;
                float shape1 = 0;
                uv*=5;
                float2 gridUV = frac(uv)*2-1;
                float2 gridUV1 = frac(uv-0.5)*2-1;

                // shape = circle(0.5, uv);


                float index = floor(uv.x) * floor(uv.y);
                index=0;
                float index1 = floor(uv.x-0.5) * floor(uv.y-0.5);
                index1=0;
                

                float shaper = (sin(_Time.y+index) + 0.75) * lerp( 1 , 15 , smoothstep( 0 , 2 , sin(_Time.y + index ) + 0.75 ));
                shape = 1 - smoothstep(0.5, 0.55, pow(abs(gridUV.x), shaper) + pow(abs(gridUV.y), shaper));

                float shaper1 = (cos(_Time.y+index1) + 0.75) * lerp(-10 , 10 , smoothstep( 0 , 1 , cos(_Time.y + index1 ) + 0.75));
                shape1 = smoothstep(0.5, 0.55, pow(abs(gridUV1.x), shaper1) + pow(abs(gridUV1.y), shaper1));

                float color1 = max(shape1,shape);
                float color2 = min(shape1,shape);
                return float4(color1, pow(color1,shaper1), color2, 1.0);

                //float2 size = float2(0.02, 0.9);
                //float2 shaper = float2(step(-size.x, uv.x), step(-size.y, uv.y));
                //shaper *= float2(1-step(size.x, uv.x), 1-step(size.y, uv.y));
                //shape = shaper.x * shaper.y;

                // return float4(uv.x, 0, uv.y, 1);
                //return float4(color1.r,color1*shaper,color1*shaper1, 1.0);
            }
            ENDCG
        }
    }
}
