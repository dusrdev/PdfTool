﻿using System.IO;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PdfTool.Models;
using PdfSharpCore;
using System.Text;

namespace PdfTool.Controller;

/// <summary>
/// Converts images to pdf
/// </summary>
internal sealed class ImageToPdfConverter {
    private readonly AppSettings _settings;

    public ImageToPdfConverter(AppSettings settings) {
        _settings = settings;
        var provider = CodePagesEncodingProvider.Instance;
        Encoding.RegisterProvider(provider);
    }

    /// <summary>
    /// Batch converts images to pdfs and saves them to the same directory as the first file
    /// </summary>
    /// <param name="filePaths"></param>
    /// <returns></returns>
    public async Task<Result> ConvertImagesAsync(string[] filePaths) {
        foreach (string path in filePaths) {
            PdfDocument documents = new PdfDocument();
            documents.Info.Title = Path.GetFileNameWithoutExtension(path);

            PdfPage page = documents.AddPage();
            XImage image = XImage.FromFile(path);
            if (image.PixelWidth > image.PixelHeight) {
                page.Orientation = PageOrientation.Landscape;
            }
            XGraphics gfx = XGraphics.FromPdfPage(page);

            await DrawImage(gfx, image, 0, 0, (int)page.Width, (int)page.Height);

            string resultPath = Path.ChangeExtension(path, ".pdf");

            if (File.Exists(resultPath)) {
                File.Delete(resultPath);
            }

            if (documents.PageCount > 0) documents.Save(resultPath);
        }

        return new Result {
            Success = true,
            Message = "Conversion successful."
        };
    }

    /// <summary>
    /// Draws image to pdf page
    /// </summary>
    /// <param name="gfx"></param>
    /// <param name="image"></param>
    /// <param name="isVertical"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    private Task DrawImage(XGraphics gfx, XImage image, int x, int y, int width, int height) {
        if (!_settings.MaintainAspectRatio) {
            gfx.DrawImage(image, x, y, width, height);
            return Task.CompletedTask;
        }

        var area = width * height;

        var inner = image.PixelWidth / (double)image.PixelHeight * area;
        var Width = Math.Sqrt(inner);
        var Height = Width * image.PixelHeight / image.PixelWidth;
        
        if (Width > width) {
            var ratio = Width / width;
            Width /= ratio;
            Height /= ratio;
        }
        
        if (Height < height) {
            y += (int)((height - Height) / 2);
        }
        
        if (Width < width) {
            x += (int)((width - Width) / 2);
        }
        
        gfx.DrawImage(image, x, y, (int)Width, (int)Height);//scale to A4
        return Task.CompletedTask;
    }
}