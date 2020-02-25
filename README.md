PostProcessingPaletteSwap
=========================

![output](https://user-images.githubusercontent.com/961165/75260567-27a32f80-582d-11ea-92e5-1d7b7c249b8e.gif)

Color palette swap for Unity PostProcessing

## Installation

- Install PostProcessing from package manager.
- [Download the latest package from here](https://github.com/kyubuns/PostProcessingPaletteSwap/releases).
- Import the unitypackage.

## Requirements

- Unity 2018.4 or higher.
- PostProcessing package.

## How to use

- Create palette image.
  - Width: 1~256px (recommend 4^x)
  - Height: unspecified
  - FilterMode: Point (no filter)
  - [Examples](https://github.com/kyubuns/PostProcessingPaletteSwap/tree/master/Assets/Plugins/PaletteSwap/Sample/Palettes)
- Import sprite to OriginalSprite directory.
  - Convert image by [the script](https://github.com/kyubuns/PostProcessingPaletteSwap/tree/master/Assets/Plugins/PaletteSwap/Sample/Editor/IndexColorConverter.cs).
  - You need to change a path in the script
- Add "Palette Swap" to a post-process camera profile of your camera.

## License

MIT License (see [LICENSE](LICENSE))

