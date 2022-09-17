# PdfTool

This is a very simple portable application that allows the user to either convert images (pdf or jpeg) to pdf files (while maintaining aspect ratio or resizing), or merging multiple pdf files into one pdf file

## Intro

I have a business client which needed a simple program to merge pdf files and convert images to pdf's without sacrificing privacy as some or all the files are private documents of their clients. There were only two feasible solutions for this, either Adobe Acrobat Premium which is expensive and overkill if only those features are required, or other Open source alternatives, most of which either hide certain features behind paywall, have many options which makes them difficult to use, have a 2002 dated user interface, or all of the above.

This led me to create this application, where safety, ease of use, portability and modern user interface are the foundations.

## Screenshots and usage

![PdfTool Screenshot](https://user-images.githubusercontent.com/8972626/189625466-5d0bb940-e631-42f1-82f7-bf62a3a23506.png)

As you can see here, the application is very simple.

### Left side

There's a big box to which you drag multiple pdf files (together) to merge them.

* You can select a desired merged file name, if this name is empty the created file will be name `merged.pdf`, if a file by the selected name already exists, the new file will append a `TimeStamp` to the name.

* The output file will be saved in the location of the first of the selected files, in order to make sure you know where it is, it is recommended that all of your selected files will be in the same location (directory).

### Right side

There's a bix box to which you drag single or multiple pictures to convert all of them to .pdf's, each file will have a separate .pdf file of the same name, only the extension will be changed.

* You can toggle whether to maintain aspect ratio or not, the default is on. If you maintain aspect ratio, the image will be scaled to fit inside the .pdf page (A4 size) and be centered. If you toggle this option off, the image will be stretched to fill a .pdf page (also A4 size)

* If the image is wider than it is high, the page will automatically be rotated to landscape orientation.
* Supported image formats are `.jpeg`, `.jpg`, `.png`, `.tif`, `.tiff`.

## FAQ

Q: What happens to documents I process with this application? are they uploaded to some server? What is the privacy policy?

A: The application does all the processing on the client computer without any connections to the internet, as a matter of fact, it can work offline just the same. Thus making it completely private and anonymous. There are no data collections whatsoever, no metrics, no user info, no nothing.

Q: I have merged pdf files, but the order is wrong? how can I order them?

A: You can't choose the order, and the application doesn't actually order the files in any way. What happens is the files are merged in the order that they are selected, so all you need to order files a certain way is to select them the way you want before dragging them to the application.

Q: Will I add options?

A: Probably not, this was made as a very simple solution to a specific client problem. If said client will require more features, maybe I will add them.

Q: I am not running windows, are there versions for other platforms?

A: This application is written and .NET 6 and all the dependencies are .NET CORE 2+ so in theory it is cross platform and should work about the same in MacOS and Linux, but, you'll have to download the project and build the executable to your specific configuration.

Q: I want to be able to use it on computers without the need to download .Net runtime dependencies, is it possible?

A: Yes, download the "self-contained" from [Releases](https://github.com/dusrdev/PdfTool/releases), extract it a new folder, and create a shortcut for the executable or launch directly from said folder.

## Installation

This application is made to be extremely portable thus it is compiled to be a single file executable, all you need is to download the latest version from [Releases](https://github.com/dusrdev/PdfTool/releases) and run it, if the client pc has no archive extractor, you can also download the .exe version
