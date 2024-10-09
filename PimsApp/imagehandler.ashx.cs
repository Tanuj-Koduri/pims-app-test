using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PimsApp
{
    /// <summary>
    /// Summary description for imagehandler
    /// </summary>
    public class imagehandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string imageName = context.Request.QueryString["imageName"];
            //string filePath = $@"C:\Users\SaiKiranKuchana\Desktop\UploadImages\{imageName}";
            string basePath = System.Configuration.ConfigurationManager.AppSettings["ImagePath"];
            string filePath = $@"{basePath}\{imageName}";

            if (System.IO.File.Exists(filePath))
            {
                string extension = System.IO.Path.GetExtension(filePath).ToLower();
                string contentType = GetContentType(extension);

                if (contentType != null)
                {
                    context.Response.ContentType = contentType;
                    context.Response.WriteFile(filePath);
                }
                else
                {
                    context.Response.StatusCode = 415; // Unsupported Media Type
                    context.Response.Write("Unsupported image type");
                }
            }
            else
            {
                context.Response.StatusCode = 404; // Not Found
                context.Response.Write("Image not found");
            }
        }
        private string GetContentType(string extension)
        {
            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                case ".bmp":
                    return "image/bmp";
                case ".tiff":
                case ".tif":
                    return "image/tiff";
                case ".ico":
                    return "image/x-icon";
                case ".svg":
                    return "image/svg+xml";
                case ".jfif":
                    return "image/jfif";
                default:
                    return null; // Unsupported image type
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}