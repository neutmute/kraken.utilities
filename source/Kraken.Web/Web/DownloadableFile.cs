using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kraken.Core;

namespace Kraken.Web
{
    /// <summary>
    /// Encodes the MIME type 
    /// </summary>
    /// <remarks>
    /// http://www.hansenb.pdx.edu/DMKB/dict/tutorials/mime_typ.php
    /// </remarks>
    public enum BrowserContentType
    {
        Unknown = 0,

        [EnumCode("text/csv")]
        Csv,

        [EnumCode("application/pdf")]
        Pdf,

        [EnumCode("application/zip")]
        Zip,

        [EnumCode("application/msword")]
        Word,

        [EnumCode("application/vnd.google-earth.kml+xml")]
        Kml
    }

    public interface IDownloadableFile
    {
        string DownloadAsFilename { get; }

        /// <summary>
        /// The type of content that this report will download as
        /// </summary>
        BrowserContentType ContentType { get; }

        byte[] GetContent();
    }
}
