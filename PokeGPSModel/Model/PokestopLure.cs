using System.Xml.Serialization;

namespace PokeGPSModel.Model
{
    [XmlRoot]
    public class PokestopLure
    {
        /// <summary>
        /// Gets or sets the expiration date time.
        /// </summary>
        /// <value>
        /// The expiration date time.
        /// </value>
        [XmlElement(typeof(long))]
        public long ExpirationDateTime { get; set; }
    }
}