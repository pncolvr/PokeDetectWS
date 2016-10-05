using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace PokeGPSDatabase.Database.Model
{
    [Table("Pokestop")]
    public partial class PokestopEntity
    {
        public long id { get; set; }

        public int pokestop_id { get; set; }

        [Required]
        public DbGeography coordinates { get; set; }

        public DateTime? expiration_date { get; set; }
    }
}
