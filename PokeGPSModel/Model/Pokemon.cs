using System;
using System.Xml.Serialization;

namespace PokeGPSModel.Model
{
    [XmlRoot]
    public class Pokemon
    {
        /// <summary>
        /// Gets or sets the expiration date.
        /// </summary>
        /// <value>
        /// The expiration date.
        /// </value>
        [XmlElement(typeof(long))]
        public long ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [XmlElement(typeof(int))]
        public int Id { get; set; }
        

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
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [XmlElement(typeof(String))]
        public String Name { get; set; }        
    }
}
