using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SignalRChat.Models.Data
{
    public class ImageFunc
    {
        public static string AvatarPath = @"~/Uploads/Avatar/";
        public static string MessageImagePath = @"~/Uploads/MessageImage/";
        // There are two type of image one is Message's image and second is User avatar
        // We have Upload folder so
        public static string AddTextToFileName(string s, string txt)
        {
            int idx = -1;
            for (int i = s.Length - 1; i >= 0; i--)
            {
                if (s[i] == '/') break;
                if (s[i] == '.')
                {
                    idx = i;
                    break;
                }
            }
            if (idx == -1)
            {
                return s + txt;
            }
            else
            {
                return s.Substring(0, idx) + txt + s.Substring(idx, s.Length - idx);
            }
        }
        public static string SaveAvatar(string FileName, string FileType, string FileContent)
        {
            if (String.IsNullOrEmpty(FileName) || String.IsNullOrEmpty(FileContent))
            {
                return FileName;
            }

            string path = System.Web.Hosting.HostingEnvironment.MapPath(AvatarPath + FileName);

            while (File.Exists(path))
            {
                Random rnd = new Random();
                int num = rnd.Next(100);
                path = AddTextToFileName(path, num.ToString());
                FileName = AddTextToFileName(FileName, num.ToString());
            }

            File.WriteAllBytes(path, Convert.FromBase64String(FileContent));
            return FileName;
        }
        public static void RemoveAvatar(string FileName)
        {
            string path = System.Web.Hosting.HostingEnvironment.MapPath(AvatarPath + FileName);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        public static string EditAvatar(string oldFileName, string newFileName, string FileType, string FileContent)
        {

            if (String.IsNullOrEmpty(newFileName) || String.IsNullOrEmpty(FileContent))
            {
                return newFileName;
            }
            RemoveAvatar(oldFileName);
            return SaveAvatar(newFileName, FileType, FileContent); ;
        }

        public static string SaveMessageImage(string FileName, string FileType, string FileContent)
        {
            if (String.IsNullOrEmpty(FileName) || String.IsNullOrEmpty(FileContent))
            {
                return FileName;
            }

            string path = System.Web.Hosting.HostingEnvironment.MapPath(MessageImagePath + FileName);

            while (File.Exists(path))
            {
                Random rnd = new Random();
                int num = rnd.Next(100);
                path = AddTextToFileName(path, num.ToString());
                FileName = AddTextToFileName(FileName, num.ToString());
            }

            File.WriteAllBytes(path, Convert.FromBase64String(FileContent));
            return FileName;
        }
        public static void RemoveMessageImage(string FileName)
        {
            string path = System.Web.Hosting.HostingEnvironment.MapPath(MessageImagePath + FileName);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        public static string EditIMessageImage(string oldFileName, string newFileName, string FileType, string FileContent)
        {

            if (String.IsNullOrEmpty(newFileName) || String.IsNullOrEmpty(FileContent))
            {
                return newFileName;
            }
            RemoveMessageImage(oldFileName);
            return SaveMessageImage(newFileName, FileType, FileContent); ;
        }

    }
}