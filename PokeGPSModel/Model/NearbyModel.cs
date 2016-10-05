using System.Xml.Serialization;

namespace PokeGPSModel.Model
{
    [XmlRoot("NearbyModel")]
    public class NearbyModel
    {
        /// <summary>
        /// The gyms
        /// </summary>
        /// <value>
        /// The gyms.
        /// </value>
        [XmlElement(typeof (Gym[]))]
        public Gym[] Gyms { get; set; }

        /// <summary>
        /// Gets or sets the poke stops.
        /// </summary>
        /// <value>
        /// The poke stops.
        /// </value>
        [XmlElement(typeof(Pokestop[]))]
        public Pokestop[] Pokestops { get; set; }

        /// <summary>
        /// The pokemons
        /// </summary>
        /// <value>
        /// The pokemons.
        /// </value>
        [XmlElement(typeof(Pokemon[]))]
        public Pokemon[] Pokemons { get; set; }
    }
}
