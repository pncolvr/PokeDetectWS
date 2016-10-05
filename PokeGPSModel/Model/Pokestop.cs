using System.Xml.Serialization;

namespace PokeGPSModel.Model
{
    [XmlRoot]
    public class Pokestop
    {
        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>
        /// The latitude.
        /// </value>
        [XmlElement(typeof(double))]
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>
        /// The longitude.
        /// </value>
        [XmlElement(typeof(double))]
        public double Longitude { get; set; }

        /// <summary>
        /// Gets or sets the lure.
        /// </summary>
        /// <value>
        /// The lure.
        /// </value>
        [XmlElement(typeof(PokestopLure))]
        public PokestopLure Lure { get; set; }

        /// <summary>
        /// Gets or sets the pokestop identifier.
        /// </summary>
        /// <value>
        /// The pokestop identifier.
        /// </value>
        [XmlElement(typeof(int))]
        public int PokestopId { get; set; }
    }
}