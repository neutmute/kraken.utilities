using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Kraken.Core;
using Kraken.Net;

namespace Kraken.Web
{
    public static class WebLogic
    {
        public static string ClientIdentity
        {
            get
            {
                string username;

                if (HttpContext.Current.User == null)
                {
                    username = NetworkService.GetHostname(RemoteIpAddress);
                }
                else
                {
                    username = HttpContext.Current.User.Identity.Name;
                    if (string.IsNullOrEmpty(username))
                    {
                        username = NetworkService.GetHostname(RemoteIpAddress);    
                    }
                }
                return username;
            }
        }

        public static string RemoteIpAddress
        {
            get
            {
                string ip = HttpContext.Current.Request["REMOTE_ADDR"];
                return ip;
            }
        }

        [CodeCoverageExcluded]
        public static void PushToBrowser(BrowserContentType contentType, string downloadAsFilename, string fileserverFilename)
        {
            PushToBrowser(contentType, downloadAsFilename, File.ReadAllBytes(fileserverFilename));
        }

        [CodeCoverageExcluded]
        public static void PushToBrowser(BrowserContentType contentType, string downloadAsFilename, byte[] content)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.ClearContent();
            response.ClearHeaders();

            response.ContentType = contentType.GetCode();
            response.AddHeader("Content-Disposition", String.Format("attachment; filename=\"{0}\"", downloadAsFilename));

            byte[] fileContents = content;

            if (fileContents != null && fileContents.Length > 0)
            {
                response.BinaryWrite(fileContents);
            }

            response.Flush();
            response.SuppressContent = true;
            response.Close();
        }

        /// <summary>
        /// Triggers a download to the client browser
        /// </summary>
        /// <remarks>
        /// Not possible to cover this in a test
        /// </remarks>
        [CodeCoverageExcluded]
        public static void PushToBrowser(IDownloadableFile file)
        {
            PushToBrowser(file.ContentType, file.DownloadAsFilename, file.GetContent());
        }
    }
}
