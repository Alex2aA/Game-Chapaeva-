Shader "Custom/DissolveShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DissolveTex ("Dissolve Texture", 2D) = "white" {}
        _DissolveAmount ("Dissolve Amount", Range(0, 1)) = 0
        _DissolveEdgeWidth ("Edge Width", Range(0, 0.2)) = 0.025
        _DissolveEdgeColor ("Edge Color", Color) = (1, 1, 0, 1)
        _EmissionIntensity ("Emission Intensity", Float) = 2
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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            sampler2D _DissolveTex;
            float4 _MainTex_ST;
            float _DissolveAmount;
            float _DissolveEdgeWidth;
            float4 _DissolveEdgeColor;
            float _EmissionIntensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the main texture
                fixed4 col = tex2D(_MainTex, i.uv);
                
                // Sample the dissolve texture
                float dissolve = tex2D(_DissolveTex, i.uv).r;
                
                // Clip pixels based on dissolve amount
                float clipValue = dissolve - _DissolveAmount;
                clip(clipValue);
                
                // Edge effect
                if (clipValue < _DissolveEdgeWidth)
                {
                    float edgeFactor = clipValue / _DissolveEdgeWidth;
                    float edgeIntensity = pow(edgeFactor, 2);
                    col.rgb = lerp(_DissolveEdgeColor.rgb * _EmissionIntensity, col.rgb, edgeIntensity);
                }
                
                return col;
            }
            ENDCG
        }
    }
}