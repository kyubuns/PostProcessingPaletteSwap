using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace PostProcessingPaletteSwap.Editor
{
    public class IndexColorConverter : AssetPostprocessor
    {
        private const string DefaultColorPalettePath = "Assets/Plugins/PaletteSwap/Sample/Palettes/a.png";
        private const string OriginalSpritePath = "Assets/Plugins/PaletteSwap/Sample/OriginalSprites";
        private const string ConvertedSpritePath = "Assets/Plugins/PaletteSwap/Sample/ConvertedSprites";

        private void OnPostprocessTexture(Texture2D texture)
        {
            if (!assetPath.Contains(OriginalSpritePath)) return;

            //var colorPalette = AssetDatabase.LoadAssetAtPath<Texture2D>(DefaultColorPalettePath);

            var colorPalette = new Texture2D (0, 0);
            colorPalette.LoadImage(File.ReadAllBytes(DefaultColorPalettePath));

            var colorToIndexColor = new Dictionary<Color32, Color32>();
            var colorPalettePixels = colorPalette.GetPixels32().Take(colorPalette.width).ToArray();
            foreach (var (palette, i) in colorPalettePixels.Select((x, i) => (x, i)))
            {
                var r = (byte) (i * 256 / colorPalettePixels.Length);
                colorToIndexColor[palette] = new Color32(r, r, r, 255);
                // Debug.Log($"{palette} = {colorToIndexColor[palette].r}");
            }

            var texturePixels = texture.GetPixels32();
            for (var i = 0; i < texturePixels.Length; ++i)
            {
                if (texturePixels[i].a == 0)
                {
                    texturePixels[i] = new Color32(0, 0, 0, 0);
                    continue;
                }

                if (!colorToIndexColor.ContainsKey(texturePixels[i]))
                {
                    throw new Exception($"{Path.GetFileName(assetPath)} has unknown color {texturePixels[i]} in ({i / texture.width}, {i % texture.width})");
                }

                var c = colorToIndexColor[texturePixels[i]];
                c.a = texturePixels[i].a;
                texturePixels[i] = c;
            }
            var newTexture2D = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false);
            newTexture2D.SetPixels32(texturePixels);
            var bytes = newTexture2D.EncodeToPNG();
            var savePath = assetPath.Replace(OriginalSpritePath, ConvertedSpritePath);
            File.WriteAllBytes(savePath, bytes);
            AssetDatabase.ImportAsset(savePath);

            Debug.Log($"IndexColorConverter: {assetPath}");
        }
    }
}
