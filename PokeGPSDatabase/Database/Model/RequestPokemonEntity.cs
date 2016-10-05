using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace PokeGPSDatabase.Database.Model
{
    [Table("RequestPokemon")]
    public partial class RequestPokemonEntity
    {
        public long id { get; set; }

        public int pokemon_id { get; set; }

        [Required]
        public DbGeography coordinates { get; set; }

        public DateTime expiration_date { get; set; }

        public virtual PokemonEntity Pokemon { get; set; }
    }
}
