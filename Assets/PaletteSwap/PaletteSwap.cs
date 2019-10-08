using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace PaletteSwapPostProcessing
{
    [Serializable]
    [PostProcess(typeof(PaletteSwapRenderer), PostProcessEvent.BeforeStack, "Custom/PaletteSwap")]
    public sealed class PaletteSwap : PostProcessEffectSettings
    {
        [Tooltip("256px x 1px")]
        public TextureParameter palette = new TextureParameter();
    }

    public sealed class PaletteSwapRenderer : PostProcessEffectRenderer<PaletteSwap>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/PaletteSwap"));
            // ReSharper disable once Unity.PreferAddressByIdToGraphicsParams
            if (settings != null && settings.palette != null)
            {
                sheet.properties.SetTexture("_Palette", settings.palette);
                sheet.properties.SetFloat("_PaletteWidth", settings.palette.value.width);
            }
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
