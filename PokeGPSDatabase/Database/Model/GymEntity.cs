using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace PokeGPSDatabase.Database.Model
{
    [Table("Gym")]
    public partial class GymEntity
    {
        public long id { get; set; }

        [Required]
        public DbGeography coordinates { get; set; }

        public int gym_id { get; set; }

        [Required]
        [StringLength(50)]
        public string team_name { get; set; }

        public long prestige { get; set; }
    }
}
