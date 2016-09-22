using System;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Web;
using System.IO;
using System.Drawing.Imaging;
using System.Web.UI;
using System.Drawing;

namespace CommonTools
{
    public static class FileHelper
    {
        public static bool IsImage(string fileName)
        {
            string[] parts = fileName.Split('.');
            if (parts.Length < 2)
                return false;
            string ext = parts[parts.Length - 1];
            switch (ext.ToUpper())
            {
                case "GIF":
                case "JPEG":
                case "JPG":
                case "BMP":
                case "PNG":
                case "ICO":
                    return true;
                default:
                    return false;
            }
        }
        public static string GetFileName(string path)
        {
            string[] parts = path.Split('\\', '/');
            if (parts.Length == 0)
                return string.Empty;
            else
                return parts[parts.Length - 1];
        }
        private static ImageFormat GetImageFormate(string ext)
        {
            switch (ext.ToUpper())
            {
                case ".GIF":
                    return ImageFormat.Gif;
                case ".JPEG":
                case ".JPG":
                    return ImageFormat.Jpeg;
                case ".BMP":
                    return ImageFormat.Bmp;
                case ".PNG":
                    return ImageFormat.Png;
                case ".ICO":
                    return ImageFormat.Icon;
                default:
                    throw new Exception("无效的图片格式！");
            }
        }
        public static bool UploadImage(HttpPostedFileBase fu, string localPath, string inname, out string outname)
        {
            outname = inname;

            if (fu.ContentType.ToLower().IndexOf("image", StringComparison.Ordinal) >= 0)
            {
                var lastid = fu.FileName.LastIndexOf('.');
                outname = string.IsNullOrEmpty(inname) ? Guid.NewGuid().ToString().Replace("-", "") : inname.Substring(0, lastid);

                var ext = fu.FileName.Substring(lastid, fu.FileName.Length - lastid);
                var imgFormat = GetImageFormate(ext);
                outname = outname + ext;
                var oripath = localPath;
                if (!Directory.Exists(oripath))
                {
                    Directory.CreateDirectory(oripath);
                }
                try
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(fu.InputStream);
                    oripath = oripath.EndsWith("/") ? oripath : oripath + "/";
                    img.Save(oripath + outname, imgFormat);
                    img.Dispose();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;

        }

        public static bool UploadImage(FileUpload fu, string localFullPath, string localThumbnailPath, int width, int height, string inname, out string outname)
        {
            Page page = HttpContext.Current.Handler as Page;
            ClientScriptManager csm = page.ClientScript;
            outname = inname;

            int lastid = fu.FileName.LastIndexOf('.');
            if (fu.HasFile)
            {
                if (fu.PostedFile.ContentType.ToLower().IndexOf("image") >= 0)
                {
                    if (string.IsNullOrEmpty(inname))
                    {
                        outname = Guid.NewGuid().ToString().Replace("-", "");
                    }
                    else
                    {
                        //outname = inname.Split('.')[0];
                        outname = inname.Substring(0, lastid);
                    }
                    string ext = fu.FileName.Substring(lastid, fu.FileName.Length - lastid);//"." + fu.FileName.Split('.')[1];
                    System.Drawing.Imaging.ImageFormat imgFormat = GetImageFormate(ext);
                    outname = outname + ext;
                    string oripath = localFullPath;
                    string minpath = string.Empty;
                    if (!Directory.Exists(oripath))
                    {
                        Directory.CreateDirectory(oripath);
                    }
                    //如果有缩略图则删除
                    var thedir = new DirectoryInfo(oripath);
                    foreach (var subdir in thedir.GetDirectories())
                    {
                        foreach (var subf in subdir.GetFiles())
                        {
                            if (subf.Name.IndexOf(outname.Split('.')[0], System.StringComparison.Ordinal) >= 0)
                            {
                                subf.Delete();
                                goto B;
                            }
                        }
                    }
                B:
                    try
                    {
                        System.Drawing.Image img = System.Drawing.Image.FromStream(fu.PostedFile.InputStream);
                        oripath = oripath.EndsWith("/") ? oripath : oripath + "/";
                        img.Save(oripath + outname, imgFormat);
                        minpath = localThumbnailPath;
                        if (!Directory.Exists(minpath))
                        {
                            Directory.CreateDirectory(minpath);
                        }
                        float heightMultipier = (float)height / (float)img.Height;
                        float widthMultipier = (float)width / (float)img.Width;
                        if (heightMultipier > 1) heightMultipier = 1;
                        if (widthMultipier > 1) widthMultipier = 1;
                        float sizeMultiplier = widthMultipier < heightMultipier ? widthMultipier : heightMultipier;
                        var tWidth = (int)(img.Width * sizeMultiplier);
                        var tHeight = (int)(img.Height * sizeMultiplier);
                        img = img.GetThumbnailImage((int)(img.Width * sizeMultiplier), (int)(img.Height * sizeMultiplier), null, System.IntPtr.Zero);
                        minpath = minpath.EndsWith("/") ? minpath : minpath + "/";
                        img.Save(minpath + outname, imgFormat);
                        img.Dispose();
                        if (!string.IsNullOrEmpty(inname))
                        {
                            if (ext != inname.Split('.')[1])
                            {
                                DeleteFile(localFullPath + inname);
                                DeleteFile(localThumbnailPath + inname);
                            }
                        }
                        return true;
                    }
                    catch
                    {
                        csm.RegisterStartupScript(page.GetType(), "jsz", "<script>alert('上传失败,请稍后再试！')</script>");
                        return false;
                    }
                }
                else
                {
                    csm.RegisterStartupScript(page.GetType(), "jscdc", "<script>alert('上传图片的格式不正确！')</script>");
                    return false;
                }
            }
            else
            {
                //csm.RegisterStartupScript(page.GetType(), "jszz", "<script>alert('请浏览要上传的图片！')</script>");
                return false;
            }
        }

        public static string GetThumbImage(string localSavePath, Stream stream, int maxWidth, int maxHeight, string name)
        {
            return GetThumbImage(localSavePath, System.Drawing.Image.FromStream(stream), maxWidth, maxHeight, name);
        }
        public static string GetThumbImage(string localSavePath, string localFilepath, int maxWidth, int maxHeight, string name)
        {
            return GetThumbImage(localSavePath, System.Drawing.Image.FromFile(localFilepath), maxWidth, maxHeight, name);
        }


        public static string GetThumbImage(string localSavePath, System.Drawing.Image img, int maxWidth, int maxHeight, string name)
        {
            int tWidth;
            int tHeight;
            if (!(maxHeight > img.Height && maxWidth > img.Width))
            {
                float heightMultipier = (float)maxHeight / (float)img.Height;
                float widthMultipier = (float)maxWidth / (float)img.Width;
                if (heightMultipier > 1) heightMultipier = 1;
                if (widthMultipier > 1) widthMultipier = 1;
                float sizeMultiplier = widthMultipier < heightMultipier ? widthMultipier : heightMultipier;
                tWidth = (int)(img.Width * sizeMultiplier);
                tHeight = (int)(img.Height * sizeMultiplier);
            }
            else
            {
                tWidth = img.Width;
                tHeight = img.Height;
            }
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(tWidth, tHeight);

            //新建一个画板
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(img, new Rectangle(0, 0, tWidth, tHeight),
                new Rectangle(0, 0, img.Width, img.Height),
                GraphicsUnit.Pixel);
            try
            {
                //以jpg格式保存缩略图
                if (!Directory.Exists(localSavePath))
                {
                    Directory.CreateDirectory(localSavePath);
                }
                bitmap.Save(localSavePath + name, ImageFormat.Jpeg);
                return name;
            }
            finally
            {
                img.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
            //img = img.GetThumbnailImage((int)(img.Width * SizeMultiplier), (int)(img.Height * SizeMultiplier), null, System.IntPtr.Zero);

        }

        public static void DeleteFile(string localPath)
        {
            if (File.Exists(localPath))
            {
                File.Delete(localPath);
            }
        }


        public static bool UploadFile(FileUpload fu, string servFolderPath, bool checkFile, string inName, out string outName)
        {
            if (fu.HasFile)
            {
                if (string.IsNullOrEmpty(inName))
                {
                    outName = Guid.NewGuid().ToString().Replace("-", "");
                }
                else
                {
                    outName = inName.Split('.')[0];
                }
                string[] parts = fu.FileName.Split('.');
                outName = outName + "." + parts[parts.Length - 1];

            }
            else
            {
                outName = inName;
                if (checkFile)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            //
            string path = HttpContext.Current.Server.MapPath(servFolderPath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            try
            {
                fu.PostedFile.SaveAs(path + outName);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool UploadFile(FileUpload fu, string servFolderPath, bool checkFile, string inName, out string outName, out int fileSize)
        {
            fileSize = 0;
            if (fu.HasFile)
            {
                if (string.IsNullOrEmpty(inName))
                {
                    outName = Guid.NewGuid().ToString().Replace("-", "");
                }
                else
                {
                    outName = inName.Split('.')[0];
                }
                string[] parts = fu.FileName.Split('.');
                outName = outName + "." + parts[parts.Length - 1];

            }
            else
            {
                outName = inName;
                if (checkFile)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            //
            string path = HttpContext.Current.Server.MapPath(servFolderPath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            try
            {
                fu.PostedFile.SaveAs(path + outName);
                var fi = new FileInfo(path + outName);
                fileSize = int.Parse(fi.Length.ToString(CultureInfo.InvariantCulture));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
