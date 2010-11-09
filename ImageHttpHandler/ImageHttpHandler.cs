using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

public class ImageHttpHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext ctx)
    {
        HttpRequest req = ctx.Request;
        string path = req.PhysicalPath;

        ImageFormat imageFormat;
        string contentType;
        string extension = Path.GetExtension(path).ToLower();

        switch (extension)
        {
            case ".gif":
                contentType = "image/gif";
                imageFormat = ImageFormat.Gif;
                break;
            case ".jpg":
                contentType = "image/jpeg";
                imageFormat = ImageFormat.Jpeg;
                break;
            case ".jpeg":
                contentType = "image/jpeg";
                imageFormat = ImageFormat.Jpeg;
                break;
            case ".png":
                contentType = "image/png";
                imageFormat = ImageFormat.Png;
                break;
            default:
                throw new NotSupportedException("Unrecognized image type.");
        }

        if (!File.Exists(path))
        {
            ctx.Response.Status = "Image not found";
            ctx.Response.StatusCode = 404;
        }
        else
        {
            ctx.Response.StatusCode = 200;
            ctx.Response.ContentType = contentType;
            if (req.FilePath.Contains("/images/thumbs/") && imageFormat != ImageFormat.Bmp) // 
                try
                {
                    AddWatermark(path, imageFormat, ctx.Response.OutputStream, ctx);
                }
                catch (Exception)
                {
                    ctx.Response.WriteFile(path);
                }
            else
                ctx.Response.WriteFile(path);
        }
    }

    public bool IsReusable { get { return true; } }


    public void AddWatermark(string filename, ImageFormat imageFormat, Stream outputStream, HttpContext ctx)
    {
        Image bitmap = Image.FromFile(filename);
        Font font = new Font("Arial", 13, FontStyle.Bold, GraphicsUnit.Pixel);
        Random rnd = new Random();
        Color color = Color.FromArgb(200, rnd.Next(255), rnd.Next(255), rnd.Next(255)); //Adds a black watermark with a low alpha value (almost transparent).
        Point atPoint = new Point(bitmap.Width / 2 - 40, bitmap.Height / 2 - 7); //The pixel point to draw the watermark at (this example puts it at 100, 100 (x, y)).
        SolidBrush brush = new SolidBrush(color);

        string watermarkText = "voobrazi.by";

        Graphics graphics;
        try
        {
            graphics = Graphics.FromImage(bitmap);
        }
        catch
        {
            Image temp = bitmap;
            bitmap = new Bitmap(bitmap.Width, bitmap.Height);
            graphics = Graphics.FromImage(bitmap);
            graphics.DrawImage(temp, new Rectangle(0, 0, bitmap.Width, bitmap.Height), 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel);
            temp.Dispose();
        }

        graphics.DrawString(watermarkText, font, brush, atPoint);
        graphics.Dispose();

        bitmap.Save(outputStream, imageFormat);
        bitmap.Dispose();
    }

    public MemoryStream BmpToJpeg(Image bmp)
    {
        var qualityEncoder = Encoder.Quality;
        const int quality = 100;
        var ratio = new EncoderParameter(qualityEncoder, quality);
        var codecParams = new EncoderParameters(1);
        codecParams.Param[0] = ratio;
        var jpegCodecInfo = GetEncoder(ImageFormat.Jpeg);

        var ms = new MemoryStream();
        bmp.Save(ms, jpegCodecInfo, codecParams);
        return ms;
    }

    private static ImageCodecInfo GetEncoder(ImageFormat format)
    {
        ImageCodecInfo[] codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders();
        return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
    }
}
