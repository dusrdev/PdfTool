# PdfTool

This is a very simple portable application that includes 3 tools:

1. Merging multiple pdf files into one
2. Splitting a single pdf file into multiple files, each page in a new file
3. Converting images to pdf files

## Features - Why PdfTool?

* 🚀 **INCREDIBLE PERFORMANCE** - Extreme optimization and multi-threading is baked into the core of the app
* 🔒 **ABSOLUTE SAFETY** - Everything is processed on the client machine and never leaves it, no internet connection required and no metrics are collected
* 🆓 **COMPLETELY FREE** - No license, no ads, no nonsense!
* 🤔 **TOTAL SIMPLICITY** - No menus and certainly no nested ones, no hidden options, no need to use guides or google. it could not be simpler to use

>"An idiot admires complexity, a genius admires simplicity." -Terry A. Davis

## Screenshots and usage

![PdfTool-Screenshot](https://user-images.githubusercontent.com/8972626/232326421-d7808597-fad2-4b22-a399-fab1ce6fe0dd.png)

As you can see here, the application is very simple.

### Merge

* You can select a desired merged file name, if this name is empty the created file will be name `merged.pdf`, if a file by the selected name already exists, the new file will append a `TimeStamp` to the name
* The output file will be saved in the location of the first of the selected files, in order to make sure you know where it is, it is recommended that all of your selected files will be in the same location (directory)
* The order of the files is the new file is the order in which you have selected them, to change the order simply select them in whichever order you want, you can select file by file and add to the selection with `CTRL and +`

### Split

* Will split the pdf file into multiple files, each page of the original pdf file will be a new file
* Each new file will have the name of the original and the corresponding page number
* Unlike `Merge`, here you can only drop a single file

### Convert

* Like `Merge` you can drop multiple images in a batch
* The pdf pages are rendered 1:1, meaning they will have the same resolution and orientation as the input images
* Supported image formats are `.jpeg`, `.jpg`, `.png`, `.tif`, `.tiff`, `.bmp`

## FAQ

Q: I am encountering a bug, what can I do?

A: There are two ways of receiving support for this product:

1. Submit an issue in Github.
2. Contact me via [email](mailto:dusrdev@gmail.com)

Q: I am not running windows, are there versions for other platforms?

A: This application is written and .NET 7 and all the dependencies are .NET CORE 2+ so in theory it is cross platform and should work about the same in MacOS and Linux, but, you'll have to download the project and build the executable to your specific configuration

Q: I want to be able to use it on computers without the need to download .Net runtime dependencies, is it possible?

A: If you are using Windows 10 or above, most likely is you already have all the dependencies installed as most Microsoft apps are built with the same technologies as this. If not, you can download the source code and compile it with `SelfContained` flag if you are an advanced user. Or just send me an [email](mailto:dusrdev@gmail.com), I will compile it and sent you the file

## Installation

1. Download
2. Unzip (If downloaded zipped version)
3. Run
4. Enjoy 😊

## Credits for dependencies

* [PdfSharp.Core](https://github.com/ststeiger/PdfSharpCore) for all pdf related functionality
* [Sharpify](https://github.com/dusrdev/Sharpify) for high performance language extensions
