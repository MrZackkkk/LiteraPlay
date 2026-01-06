Shader "LiteraPlay/OutlineHover"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (1, 0.8, 0, 1)
        _OutlineThickness ("Outline Thickness", Range(0, 0.1)) = 0.02
        _Hover ("Hover", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            Name "Outline"
            Tags { "LightMode" = "SRPDefaultUnlit" }
            Cull Front
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _OutlineColor;
                float _OutlineThickness;
                float _Hover;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                float3 normalOS = normalize(input.normalOS);
                float3 expandedPosition = input.positionOS.xyz + normalOS * _OutlineThickness;
                output.positionHCS = TransformObjectToHClip(float4(expandedPosition, 1.0));
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                return half4(_OutlineColor.rgb, _OutlineColor.a * _Hover);
            }
            ENDHLSL
        }
    }
}
