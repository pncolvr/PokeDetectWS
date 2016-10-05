using System;
using System.Xml.Serialization;

namespace PokeGPSDetectorWS.Image.Model
{
    [XmlRoot]
    public class Image
    {
        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        [XmlElement(typeof(String))]
        public String FileName { get; set; }
        /// <summary>
        /// Gets or sets the bytes.
        /// </summary>
        /// <value>
        /// The bytes.
        /// </value>
        [XmlElement(typeof(byte[]))]
        public String Base64 { get; set; }
    }
}
