using ADA.Site.App_Start;
using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADA.Site.Helpers
{
    public enum GroupeMime
    {
        Video,
        Image,
        Pdf,
        Audio,
        Downloadable
    }

    public static class GroupeMimeHelper
    {
        private static List<string> _videoMime = new List<string>() { "video/mp4", "video/ogg", "video/webm" };
        private static List<string> _audioMime = new List<string>() { "audio/mpeg", "audio/ogg", "audio/wav" };
        private static List<string> _imageMime = new List<string>() { "image/gif", "image/jpeg", "image/png", "image/tiff" };
        private static List<string> _pdfMime = new List<string>() { "application/pdf" };

        public static GroupeMime GetGroupeMime(string fileName)
        {
            ILogger logger = WindsorActivator.bootstrapper.Container.Resolve<ILogger>();

            if (System.IO.Path.GetExtension(fileName) == ".mp4") return GroupeMime.Video;

            if (_videoMime.Contains(System.Web.MimeMapping.GetMimeMapping(fileName))) return GroupeMime.Video;
            if (_audioMime.Contains(System.Web.MimeMapping.GetMimeMapping(fileName))) return GroupeMime.Audio;
            if (_imageMime.Contains(System.Web.MimeMapping.GetMimeMapping(fileName))) return GroupeMime.Image;
            if (_pdfMime.Contains(System.Web.MimeMapping.GetMimeMapping(fileName))) return GroupeMime.Pdf;

            return GroupeMime.Downloadable;
        }
    }
}
