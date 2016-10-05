using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokemonGo.RocketAPI;
using PokemonGo.RocketAPI.GeneratedCode;
using AllEnum;
using System;
using PokeGPSDetectorWS.Gyms.Controller;
using PokeGPSDetectorWS.PokemonGOAPI.Extensions;
using PokeGPSDetectorWS.Pokemons.Controller;
using PokeGPSDetectorWS.PokeStops.Controller;
using PokeGPSModel.Model;

namespace PokeGPSDetectorWS.PokemonGOAPI.Controller
{
    internal class PokemonGOWrapper
    {
        private Client _client;

        /// <summary>
        /// Logins the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public void Login(String username, String password)
        {
            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
            {
                throw new Exception("Username and password must not be null.");
            }
            this.PerformLogin(username, password);
        }

        /// <summary>
        /// Performs the login.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        private void PerformLogin(string username, string password)
        {
            try
            {
                this._client = new Client(new Settings());
                _client.DoPtcLogin(username, password).Wait();
                _client.SetServer().Wait();
            }
            catch (Exception e)
            {
                throw new Exception($"Unable to login. Details: {e.Message}");
            }

        }

        /// <summary>
        /// Gets the nearby data.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns>
        /// Nerby data
        /// </returns>
        public async Task<NearbyModel> GetNearbyData(double latitude, double longitude)
        {
            if (!await this.RequestMove(latitude, longitude))
            {
                throw new Exception("Unable to move");
            }
            try
            {
                var mapObjects = await GetMapObjects();
                if (mapObjects == null)
                {
                    return default(NearbyModel);
                }
                List<PokeGPSModel.Model.Pokemon> pokemons = new List<PokeGPSModel.Model.Pokemon>();
                List<Gym> gyms = mapObjects.MapCells.SelectMany(i => i.Forts).Where(i => i.Type == FortType.Gym).
                    Select(fortData => new GymController().ParseGymObject(fortData)).ToList();
                List<Pokestop> pokestops = mapObjects.MapCells.SelectMany(i => i.Forts).
                    Where(i => i.Type == FortType.Checkpoint && i.CooldownCompleteTimestampMs < DateTime.UtcNow.ToUnixTime()).
                    Select(pokestopData => new PokestopController().ParsePokeStopObject(pokestopData, pokemons)).ToList();
                pokemons.AddRange(mapObjects.MapCells.SelectMany(i => i.CatchablePokemons).
                    Select(pokemonData => new PokemonController().ParsePokemonObject(pokemonData)).ToList());
                return new NearbyModel
                {
                    Gyms = gyms.ToArray(),
                    Pokestops = pokestops.ToArray(),
                    Pokemons = pokemons.ToArray()
                };
            }
            catch (Exception e)
            {
                throw new Exception($"Unable to get pokemon data. Details: {e.Message}");
            }
        }

        /// <summary>
        /// Requests the move.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns>
        /// True if move update, false otherwise
        /// </returns>
        private async Task<bool> RequestMove(double latitude, double longitude)
        {
            try
            {
                return await _client.UpdatePlayerLocation(latitude, longitude) != null;
            }
            catch (Exception e)
            {
                throw new Exception($"Unable to move. Details: {e.Message}");
            }
        }

        /// <summary>
        /// Gets the map objects.
        /// </summary>
        /// <returns>Map objects</returns>
        private async Task<GetMapObjectsResponse> GetMapObjects()
        {
            try
            {
                return await _client.GetMapObjects();
            }
            catch (Exception e)
            {
                throw new Exception($"Unable to get map objects. Details: {e.Message}");
            }
        }        
    }
}