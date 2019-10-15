Shader "Hidden/Custom/PaletteSwap"
{
    HLSLINCLUDE

        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
        TEXTURE2D_SAMPLER2D(_PaletteTex, sampler_PaletteTex);
        float _PaletteWidth;

        float4 Frag(VaryingsDefault i) : SV_Target
        {
            float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
            float4 paletteColor = SAMPLE_TEXTURE2D(_PaletteTex, sampler_PaletteTex, float2(color.r * 255 / 256 + 0.5 / _PaletteWidth , 0.5));
            float4 outColor = float4(paletteColor.r, paletteColor.g, paletteColor.b, color.a);
            return outColor;
        }

    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM

                #pragma vertex VertDefault
                #pragma fragment Frag

            ENDHLSL
        }
    }
}
