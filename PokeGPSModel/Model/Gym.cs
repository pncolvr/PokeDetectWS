using System;
using System.Xml.Serialization;

namespace PokeGPSModel.Model
{
    [XmlRoot]
    public class Gym
    {
        /// <summary>
        /// Gets or sets the gym identifier.
        /// </summary>
        /// <value>
        /// The gym identifier.
        /// </value>
        [XmlElement(typeof(int))]
        public int GymId { get; set; }

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
        /// Gets or sets the name of the team.
        /// </summary>
        /// <value>
        /// The name of the team.
        /// </value>
        [XmlElement(typeof(String))]
        public String TeamName { get; set; }

        /// <summary>
        /// Gets or sets the prestige.
        /// </summary>
        /// <value>
        /// The prestige.
        /// </value>
        [XmlElement(typeof(long))]
        public long Prestige { get; set; }
    }
}
