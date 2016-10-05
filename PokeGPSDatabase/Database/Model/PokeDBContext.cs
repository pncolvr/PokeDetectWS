using System.Data.Entity;

namespace PokeGPSDatabase.Database.Model
{

    public partial class PokeDBContext : DbContext
    {
        public PokeDBContext()
            : base("name=PokeDBContext")
        {
        }

        public virtual DbSet<GymEntity> Gym { get; set; }
        public virtual DbSet<PokemonEntity> Pokemon { get; set; }
        public virtual DbSet<PokestopEntity> Pokestop { get; set; }
        public virtual DbSet<RequestPokemonEntity> RequestPokemon { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PokemonEntity>()
                .HasMany(e => e.RequestPokemon)
                .WithRequired(e => e.Pokemon)
                .HasForeignKey(e => e.pokemon_id)
                .WillCascadeOnDelete(false);
        }
    }
}
