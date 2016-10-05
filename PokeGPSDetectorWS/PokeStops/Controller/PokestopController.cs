using System.Collections.Generic;
using PokeGPSDetectorWS.Pokemons.Helper;
using PokeGPSModel.Model;
using PokemonGo.RocketAPI.GeneratedCode;

namespace PokeGPSDetectorWS.PokeStops.Controller
{
    internal class PokestopController
    {
        /// <summary>
        /// Parses the poke stop object.
        /// </summary>
        /// <param name="pokestop">The pokestop.</param>
        /// <returns></returns>
        public Pokestop ParsePokeStopObject(FortData pokestop, List<PokeGPSModel.Model.Pokemon> pokemons)
        {
            if (pokestop == null)
            {
                return null;
            }
            Pokestop model = new Pokestop
            {
                Latitude = pokestop.Latitude,
                Longitude = pokestop.Longitude,
                PokestopId = 1
            };
            if (pokestop.LureInfo != null)
            {
                pokemons.Add(new PokeGPSModel.Model.Pokemon
                {
                    Id = (int) pokestop.LureInfo.ActivePokemonId,
                    Latitude = pokestop.Latitude,
                    Longitude = pokestop.Longitude,
                    ExpirationDate = pokestop.LureInfo.LureExpiresTimestampMs,
                    Name = PokemonInfo.PokeInfo[(int)pokestop.LureInfo.ActivePokemonId]

                });
                model.PokestopId = 2;
                model.Lure = new PokestopLure
                {
                    ExpirationDateTime = pokestop.LureInfo.LureExpiresTimestampMs
                };
            }
            return model;
        }
    }
}