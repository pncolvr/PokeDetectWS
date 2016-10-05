using PokeGPSDetectorWS.Pokemons.Helper;
using PokemonGo.RocketAPI.GeneratedCode;
using Pokemon = PokeGPSModel.Model.Pokemon;

namespace PokeGPSDetectorWS.Pokemons.Controller
{
    internal class PokemonController
    {
        /// <summary>
        /// Parses the pokemon object.
        /// </summary>
        /// <param name="pokemon">The pokemon.</param>
        /// <returns>PokemonEntity model</returns>
        public Pokemon ParsePokemonObject(MapPokemon pokemon)
        {
            if (pokemon == null)
            {
                return null;
            }
            Pokemon model = new Pokemon
            {
                ExpirationDate = pokemon.ExpirationTimestampMs,
                Id = (int) pokemon.PokemonId,
                Latitude = pokemon.Latitude,
                Longitude = pokemon.Longitude,
                Name = PokemonInfo.PokeInfo[(int) pokemon.PokemonId]
            };
            return model;
        }
    }
}
