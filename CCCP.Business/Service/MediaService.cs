using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCCP.Common;
using CCCP.Business.Model;
using CCCP.ViewModel;
using System.IO;
using System.Net;
using System.Configuration;

namespace CCCP.Business.Service
{
    public class MediaService
    {
        public static byte[] ResizeImage(string srcPath, string fileType)
        {
            try
            {
                // 1 to 31, smaller means better -> compressed image size will be larger
                string quality_level = "16";
                string outputFile = srcPath.Replace(fileType, "_s.jpg");

                DateTime start = DateTime.Now;

                string html = string.Empty;

                if (System.IO.File.Exists(outputFile))
                {
                    System.IO.File.Delete(outputFile);
                }

                string cmd = " -i \"" + srcPath + "\" -q:v " + quality_level + " -vf scale=400:-1 \"" + outputFile + "\"";
                ConvertNow(cmd);

                if (!System.IO.File.Exists(outputFile))
                {
                    throw new Exception("no output file");
                }

                byte[] ret = System.IO.File.ReadAllBytes(outputFile);

                if (System.IO.File.Exists(outputFile))
                {
                    System.IO.File.Delete(outputFile);
                }

                return ret;
            }
            catch (Exception ex)
            {
                return new byte[] {};
            }
        }

        public static byte[] CompressVideo(string srcPath, string fileType)
        {
            try
            {
                string vbitrate = "555";
                string abitrate = "128";
                string outputFile = srcPath.Replace(fileType, "_c.mp4"); ;

                DateTime start = DateTime.Now;

                string html = string.Empty;

                if (System.IO.File.Exists(outputFile))
                {
                    System.IO.File.Delete(outputFile);
                }

                string cmd = "-i \"" + srcPath + "\" -acodec copy -vcodec libx264 -b:v " + vbitrate + "k -b:a " + abitrate + "k \"" + outputFile + "\"";
                ConvertNow(cmd);

                if (!System.IO.File.Exists(outputFile))
                {
                    throw new Exception("no output file ");
                }
                
                byte[] ret = System.IO.File.ReadAllBytes(outputFile);

                if (System.IO.File.Exists(outputFile))
                {
                    System.IO.File.Delete(outputFile);
                }

                return ret;
            }
            catch (Exception ex)
            {
                return new byte[] {};
            }
        }

        private static void ConvertNow(string cmd)
        {
            string exepath;
            //string AppPath = Request.PhysicalApplicationPath;
            string AppPath = (ConfigurationManager.AppSettings["ffmpegBin"] != null) ? ConfigurationManager.AppSettings["ffmpegBin"].ToString() : @"D:\CEM_Web_POC\ffmpeg-20151205-git-a16243a-win64-static\bin\";
            //Get the application path
            exepath = AppPath + "ffmpeg.exe";
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = exepath;
            //Path of exe that will be executed, only for "filebuffer" it will be "flvtool2.exe"
            proc.StartInfo.Arguments = cmd;
            //The command which will be executed
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.RedirectStandardOutput = false;
            proc.Start();

            while (proc.HasExited == false)
            {

            }

            proc.Close();
        }
    }
}
