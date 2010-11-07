//------------------------------------------------------------------------------
// The contents of this file are subject to the nopCommerce Public License Version 1.0 ("License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at  http://www.nopCommerce.com/License.aspx. 
// 
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. 
// See the License for the specific language governing rights and limitations under the License.
// 
// The Original Code is nopCommerce.
// The Initial Developer of the Original Code is NopSolutions.
// All Rights Reserved.
// 
// Contributor(s): _______. 
//------------------------------------------------------------------------------
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using NopSolutions.NopCommerce.DataAccess.Media;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.BusinessLogic.Media
{
    /// <summary>
    /// Picture manager
    /// </summary>
    public static partial class PictureManager
    {
        #region Fields
        private static object s_lock;

        private static ImageCodecInfo _jpegCodec;
        #endregion

        #region Ctor
        static PictureManager()
        {
            s_lock = new object();
        }
        #endregion

        #region Utilities
        private static PictureCollection DBMapping(DBPictureCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            PictureCollection collection = new PictureCollection();
            foreach (DBPicture dbItem in dbCollection)
            {
                Picture item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static Picture DBMapping(DBPicture dbItem)
        {
            if (dbItem == null)
                return null;

            Picture item = new Picture();
            item.PictureID = dbItem.PictureID;
            item.PictureBinary = dbItem.PictureBinary;
            item.Extension = dbItem.Extension;
            item.IsNew = dbItem.IsNew;

            return item;
        }
        
        /// <summary>
        /// Returns the first ImageCodeInfo instance with the specified mime type. Some people try to get the ImageCodeInfo instance by index - sounds rather fragile to me.
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        private static ImageCodecInfo getImageCodeInfo(string mimeType)
        {
            ImageCodecInfo[] info = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in info)
                if (ici.MimeType.Equals(mimeType, StringComparison.OrdinalIgnoreCase)) 
                    return ici;
            return null;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get a picture URL
        /// </summary>
        /// <param name="imageId">Picture identifier</param>
        /// <returns>Picture URL</returns>
        public static string GetPictureUrl(int imageId)
        {
            Picture picture = GetPictureByID(imageId);
            return GetPictureUrl(picture);
        }

        /// <summary>
        /// Get a picture URL
        /// </summary>
        /// <param name="picture">Picture instance</param>
        /// <returns>Picture URL</returns>
        public static string GetPictureUrl(Picture picture)
        {
            return GetPictureUrl(picture, 0);
        }

        /// <summary>
        /// Get a picture URL
        /// </summary>
        /// <param name="imageId">Picture identifier</param>
        /// <param name="TargetSize">The target picture size (longest side)</param>
        /// <returns>Picture URL</returns>
        public static string GetPictureUrl(int imageId, int TargetSize)
        {
            Picture picture = GetPictureByID(imageId);
            return GetPictureUrl(picture, TargetSize);
        }

        /// <summary>
        /// Get a picture URL
        /// </summary>
        /// <param name="picture">Picture instance</param>
        /// <param name="TargetSize">The target picture size (longest side)</param>
        /// <returns>Picture URL</returns>
        public static string GetPictureUrl(Picture picture, int TargetSize)
        {
            return GetPictureUrl(picture, TargetSize, true);
        }

        /// <summary>
        /// Get a picture URL
        /// </summary>
        /// <param name="imageId">Picture identifier</param>
        /// <param name="TargetSize">The target picture size (longest side)</param>
        /// <param name="showDefaultPicture">A value indicating whether the default picture is shown</param>
        /// <returns></returns>
        public static string GetPictureUrl(int imageId, int TargetSize, bool showDefaultPicture)
        {
            Picture picture = GetPictureByID(imageId);
            return GetPictureUrl(picture, TargetSize, showDefaultPicture);
        }

        /// <summary>
        /// Gets the default picture URL
        /// </summary>
        /// <param name="TargetSize">The target picture size (longest side)</param>
        /// <returns></returns>
        public static string GetDefaultPictureUrl(int TargetSize)
        {
            return GetDefaultPictureUrl(PictureTypeEnum.Entity, TargetSize);
        }

        /// <summary>
        /// Gets the default picture URL
        /// </summary>
        /// <param name="DefaultPictureType">Default picture type</param>
        /// <param name="TargetSize">The target picture size (longest side)</param>
        /// <returns></returns>
        public static string GetDefaultPictureUrl(PictureTypeEnum DefaultPictureType, int TargetSize)
        {
            string defaultImageName = string.Empty;
            switch (DefaultPictureType)
            {
                case PictureTypeEnum.Entity:
                    defaultImageName = SettingManager.GetSettingValue("Media.DefaultImageName");
                    break;
                case PictureTypeEnum.Avatar:
                    defaultImageName = SettingManager.GetSettingValue("Media.Customer.DefaultAvatarImageName");
                    break;
                default:
                    defaultImageName = SettingManager.GetSettingValue("Media.DefaultImageName");
                    break;
            }


            string relPath = CommonHelper.GetStoreLocation() + "images/" + defaultImageName;
            if (TargetSize == 0)
                return relPath;
            else
            {
                string filePath = Path.Combine(LocalImagePath, defaultImageName);
                if (File.Exists(filePath))
                {
                    string fname = string.Format("{0}_{1}{2}",
                        Path.GetFileNameWithoutExtension(filePath),
                        TargetSize,
                        Path.GetExtension(filePath));
                    if (!File.Exists(Path.Combine(LocalThumbImagePath, fname)))
                    {
                        Bitmap b = new Bitmap(filePath);

                        Size newSize = CalculateDimensions(b.Size, TargetSize);

                        if (newSize.Width < 1)
                            newSize.Width = 1;
                        if (newSize.Height < 1)
                            newSize.Height = 1;

                        Bitmap newBitMap = new Bitmap(newSize.Width, newSize.Height);
                        Graphics g = Graphics.FromImage(newBitMap);
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                        g.DrawImage(b, 0, 0, newSize.Width, newSize.Height);
                        EncoderParameters ep = new EncoderParameters();
                        ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, PictureManager.ImageQuality);
                        newBitMap.Save(Path.Combine(LocalThumbImagePath, fname), getImageCodeInfo("image/jpeg"), ep);
                        newBitMap.Dispose();
                        b.Dispose();
                    }
                    return CommonHelper.GetStoreLocation() + "images/thumbs/" + fname;
                }
                return relPath;
            }
        }

        /// <summary>
        /// Get a picture URL
        /// </summary>
        /// <param name="picture">Picture instance</param>
        /// <param name="TargetSize">The target picture size (longest side)</param>
        /// <param name="showDefaultPicture">A value indicating whether the default picture is shown</param>
        /// <returns></returns>
        public static string GetPictureUrl(Picture picture, int TargetSize, bool showDefaultPicture)
        {
            string url = string.Empty;
            if (picture == null)
            {
                if (showDefaultPicture)
                    url = GetDefaultPictureUrl(TargetSize);
                return url;
            }

            string[] parts = picture.Extension.Split('/');
            string lastPart = parts[parts.Length - 1];
            switch (lastPart)
            {
                case "pjpeg":
                    lastPart = "jpg";
                    break;
                case "x-png":
                    lastPart = "png";
                    break;
                case "x-icon":
                    lastPart = "ico";
                    break;
            }

            string localFilename = string.Empty;
            if (picture.IsNew)
            {
                string filter = string.Format("{0}*.*", picture.PictureID.ToString("0000000"));
                string[] currentFiles = System.IO.Directory.GetFiles(PictureManager.LocalThumbImagePath, filter);
                foreach (string currentFileName in currentFiles)
                    File.Delete(Path.Combine(PictureManager.LocalThumbImagePath, currentFileName));

                picture = PictureManager.UpdatePicture(picture.PictureID, picture.PictureBinary, picture.Extension, false);
            }
            lock (s_lock)
            {
                if (TargetSize == 0)
                {
                    localFilename = string.Format("{0}.{1}", picture.PictureID.ToString("0000000"), lastPart);
                    if (!File.Exists(Path.Combine(PictureManager.LocalThumbImagePath, localFilename)))
                    {
                        if (!System.IO.Directory.Exists(PictureManager.LocalThumbImagePath))
                        {
                            System.IO.Directory.CreateDirectory(PictureManager.LocalThumbImagePath);
                        }
                        File.WriteAllBytes(Path.Combine(PictureManager.LocalThumbImagePath, localFilename), picture.PictureBinary);
                    }
                }
                else
                {
                    localFilename = string.Format("{0}_{1}.{2}", picture.PictureID.ToString("0000000"), TargetSize, lastPart);
                    if (!File.Exists(Path.Combine(PictureManager.LocalThumbImagePath, localFilename)))
                    {
                        if (!System.IO.Directory.Exists(PictureManager.LocalThumbImagePath))
                        {
                            System.IO.Directory.CreateDirectory(PictureManager.LocalThumbImagePath);
                        }
                        using (MemoryStream stream = new MemoryStream(picture.PictureBinary))
                        {
                            Bitmap b = new Bitmap(stream);

                            Size newSize = CalculateDimensions(b.Size, TargetSize);

                            if (newSize.Width < 1)
                                newSize.Width = 1;
                            if (newSize.Height < 1)
                                newSize.Height = 1;

                            Bitmap newBitMap = new Bitmap(newSize.Width, newSize.Height);
                            Graphics g = Graphics.FromImage(newBitMap);
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                            g.DrawImage(b, 0, 0, newSize.Width, newSize.Height);
                            EncoderParameters ep = new EncoderParameters();
                            ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, PictureManager.ImageQuality);
                            newBitMap.Save(Path.Combine(PictureManager.LocalThumbImagePath, localFilename), getImageCodeInfo("image/jpeg"), ep);
                            newBitMap.Dispose();
                            b.Dispose();
                        }
                    }
                }
            }
            url = CommonHelper.GetStoreLocation() + "images/thumbs/" + localFilename;
            return url;
        }

        /// <summary>
        /// Calculates picture dimensions whilst maintaining aspect
        /// </summary>
        /// <param name="OriginalSize">The original picture size</param>
        /// <param name="TargetSize">The target picture size (longest side)</param>
        /// <returns></returns>
        public static Size CalculateDimensions(Size OriginalSize, int TargetSize)
        {
            Size newSize = new Size();
            if (OriginalSize.Height > OriginalSize.Width) // portrait 
            {
                newSize.Width = (int)(OriginalSize.Width * (float)(TargetSize / (float)OriginalSize.Height));
                newSize.Height = TargetSize;
            }
            else // landscape or square
            {
                newSize.Height = (int)(OriginalSize.Height * (float)(TargetSize / (float)OriginalSize.Width));
                newSize.Width = TargetSize;
            }
            return newSize;
        }

        /// <summary>
        /// Gets a picture
        /// </summary>
        /// <param name="PictureID">Picture identifier</param>
        /// <returns>Picture</returns>
        public static Picture GetPictureByID(int PictureID)
        {
            if (PictureID == 0)
                return null;

            DBPicture dbItem = DBProviderManager<DBPictureProvider>.Provider.GetPictureByID(PictureID);
            Picture picture = DBMapping(dbItem);
            return picture;
        }

        /// <summary>
        /// Deletes a picture
        /// </summary>
        /// <param name="PictureID">Picture identifier</param>
        public static void DeletePicture(int PictureID)
        {
            string filter = string.Format("{0}*.*", PictureID.ToString("0000000"));
            string[] currentFiles = System.IO.Directory.GetFiles(PictureManager.LocalThumbImagePath, filter);
            foreach (string currentFileName in currentFiles)
                File.Delete(Path.Combine(PictureManager.LocalThumbImagePath, currentFileName));

            DBProviderManager<DBPictureProvider>.Provider.DeletePicture(PictureID);
        }

        /// <summary>
        /// Validates input picture dimensions
        /// </summary>
        /// <param name="PictureBinary">PictureBinary</param>
        /// <returns></returns>
        public static byte[] ValidatePicture(byte[] PictureBinary)
        {
            using (MemoryStream stream = new MemoryStream(PictureBinary))
            {
                Bitmap b = new Bitmap(stream);
                int maxSize = SettingManager.GetSettingValueInteger("Media.MaximumImageSize", 1280);

                if ((b.Height > maxSize) || (b.Width > maxSize))
                {
                    Size newSize = CalculateDimensions(b.Size, maxSize);
                    Bitmap newBitMap = new Bitmap(newSize.Width, newSize.Height);
                    Graphics g = Graphics.FromImage(newBitMap);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    g.DrawImage(b, 0, 0, newSize.Width, newSize.Height);

                    MemoryStream m = new MemoryStream();
                    EncoderParameters ep = new EncoderParameters();
                    ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, PictureManager.ImageQuality);
                    newBitMap.Save(m, getImageCodeInfo("image/jpeg"), ep);
                    newBitMap.Dispose();
                    b.Dispose();

                    return m.GetBuffer();
                }
                else
                {
                    b.Dispose();
                    return PictureBinary;
                }
            }
        }

        /// <summary>
        /// Inserts a picture
        /// </summary>
        /// <param name="PictureBinary">The picture binary</param>
        /// <param name="Extension">The picture extension</param>
        /// <param name="IsNew">A value indicating whether the picture is new</param>
        /// <returns>Picture</returns>
        public static Picture InsertPicture(byte[] PictureBinary, string Extension, bool IsNew)
        {
            PictureBinary = ValidatePicture(PictureBinary);
            DBPicture dbItem = DBProviderManager<DBPictureProvider>.Provider.InsertPicture(PictureBinary, Extension, IsNew);
            Picture picture = DBMapping(dbItem);
            return picture;
        }

        /// <summary>
        /// Updates the picture
        /// </summary>
        /// <param name="PictureID">The picture identifier</param>
        /// <param name="PictureBinary">The picture binary</param>
        /// <param name="Extension">The picture extension</param>
        /// <param name="IsNew">A value indicating whether the picture is new</param>
        /// <returns>Picture</returns>
        public static Picture UpdatePicture(int PictureID, byte[] PictureBinary, string Extension, bool IsNew)
        {
            ValidatePicture(PictureBinary);
            DBPicture dbItem = DBProviderManager<DBPictureProvider>.Provider.UpdatePicture(PictureID, PictureBinary, Extension, IsNew);
            Picture picture = DBMapping(dbItem);
            return picture;
        }

        /// <summary>
        /// Gets the picture binary array
        /// </summary>
        /// <param name="fs">File stream</param>
        /// <param name="size">Picture size</param>
        /// <returns>Picture binary array</returns>
        public static byte[] GetPictureBits(Stream fs, int size)
        {
            byte[] img = new byte[size];
            fs.Read(img, 0, size);
            return img;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets an image quality
        /// </summary>
        public static long ImageQuality
        {
            get
            {
                return 100L;
            }
        }

        /// <summary>
        /// Gets a local thumb image path
        /// </summary>
        public static string LocalThumbImagePath
        {
            get
            {
                string path = HttpContext.Current.Request.PhysicalApplicationPath + "images\\thumbs";
                return path;
            }
        }

        /// <summary>
        /// Gets the local image path
        /// </summary>
        public static string LocalImagePath
        {
            get
            {
                string path = HttpContext.Current.Request.PhysicalApplicationPath + "images";
                return path;
            }
        }
        #endregion
    }
}
