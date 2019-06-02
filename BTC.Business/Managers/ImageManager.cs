using BTC.Common.Constants;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BTC.Business.Managers
{
    public class ImageManager
    {

        public ImageManager()
        {

        }
        public string SaveProfileImageOrigin(HttpPostedFile file)
        {
            string newFileName = Guid.NewGuid().ToString().Replace("-", "");
            string ext = System.IO.Path.GetExtension(file.FileName);
            newFileName = newFileName + ".png";
            string savedBaseFilePath = Path.Combine(@"\Images\_userfiles\profile\origin\", newFileName);
            file.SaveAs(savedBaseFilePath);
            return savedBaseFilePath;
        }

        public Task SavePostMainPage(HttpPostedFileBase file, string savedBaseFilePath)
        {
            return Task.Run(() =>
           {
               byte[] imgByteArr = new BinaryReader(file.InputStream)
                     .ReadBytes((int)file.InputStream.Length);
               Image new_img = ByteArrayToImage(imgByteArr);
               new_img.Save(savedBaseFilePath);
               var medium_img = ResiveImage(new_img, 800, 400);
               savedBaseFilePath = savedBaseFilePath.Replace("large", "medium");
               medium_img.Save(savedBaseFilePath);
               var small_img = ResiveImage(new_img, 500, 200);
               savedBaseFilePath = savedBaseFilePath.Replace("medium", "small");
               medium_img.Save(savedBaseFilePath);

           });

        }

        public bool CheckImageType(string type)
        {
            switch (type)
            {
                case ".jpg":
                    return true;
                case ".jpeg":
                    return true;
                case ".png":
                    return true;
                default:
                    return false;
            }
        }
        public Image ResiveImage(Image image, int maxHeight, int newWidth)
        {
            if (image.Width <= newWidth) newWidth = image.Width;

            var newHeight = image.Height * newWidth / image.Width;
            if (newHeight > maxHeight)
            {
                // Resize with height instead  
                newWidth = image.Width * maxHeight / image.Height;
                newHeight = maxHeight;
            }

            var res = new Bitmap(newWidth, newHeight);

            using (var graphic = Graphics.FromImage(res))
            {
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.CompositingQuality = CompositingQuality.HighQuality;
                graphic.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            return res;
        }

        public Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        public Task RemoveOldPostImages(string file_name)
        {

            return Task.Run(() =>
            {
                string file_path = Path.Combine(@"\Images\_post\large\", file_name);
                if (File.Exists(file_path))
                {
                    File.Delete(file_path);
                }

                file_path = file_path.Replace("large", "medium");

                if (File.Exists(file_path))
                {
                    File.Delete(file_path);
                }

                file_path = file_path.Replace("medium", "small");

                if (File.Exists(file_path))
                {
                    File.Delete(file_path);
                }


            });

        }
    }
}
