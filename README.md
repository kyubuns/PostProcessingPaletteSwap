PostProcessingPaletteSwap
=========================

Color palette swap for Unity PostProcessing

## Installation

- Install PostProcessing from package manager.
- [Download the latest package from here](https://github.com/kyubuns/PostProcessingPaletteSwap/releases).
- Import the unitypackage.

## 既にある素材をIndexColorに変換するスクリプト

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class IndexColorConverter : AssetPostprocessor
    {
        private const string DefaultColorPalettePath = "Assets/References/Game/ColorPalette/Default.png";

        private void OnPostprocessTexture(Texture2D texture)
        {
            if (!assetPath.Contains("Game/OriginalSprites")) return;

            var colorPalette = AssetDatabase.LoadAssetAtPath<Texture2D>(DefaultColorPalettePath);
            var colorToIndexColor = new Dictionary<Color32, Color32>();
            var colorPalettePixels = colorPalette.GetPixels32();
            foreach (var (palette, i) in colorPalettePixels.Select((x, i) => (x, i)))
            {
                var r = (byte) (i * 256 / colorPalettePixels.Length);
                colorToIndexColor[palette] = new Color32(r, r, r, 255);
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
                    throw new Exception($"{Path.GetFileName(assetPath)} has unknown color {texturePixels[i]}");
                }

                texturePixels[i] = colorToIndexColor[texturePixels[i]];
            }
            var newTexture2D = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false);
            newTexture2D.SetPixels32(texturePixels);
            var bytes = newTexture2D.EncodeToPNG();
            File.WriteAllBytes(assetPath.Replace("OriginalSprites", "Sprites"), bytes);

            Debug.Log($"IndexColorConverter: {assetPath}");
        }
    }
}
```

## Requirements

- Unity 2018.4 or higher.

## License

MIT License (see [LICENSE](LICENSE))

