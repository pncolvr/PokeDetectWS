using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Data.Entity.SqlServer;
using System.Globalization;
using System.Linq;
using PokeGPSDatabase.Database.Model;
using PokeGPSModel.Model;

namespace PokeGPSDatabase.Database.Controller
{
    public class DBController
    {
        /// <summary>
        /// Gets the nearby data.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="radiusMeters">The radius meters.</param>
        /// <returns>Nearby data</returns>
        public NearbyModel GetNearbyData(double latitude, double longitude, int radiusMeters)
        {
            NearbyModel nearbyModel;
            using (PokeDBContext db = new PokeDBContext())
            {
                try
                {
                    List<Gym> gyms = new List<Gym>();
                    List<Pokestop> pokestops = new List<Pokestop>();
                    List<Pokemon> pokemons = new List<Pokemon>();
                    DbGeography point = this.ConvertGPSToGeography(latitude, longitude);
                    DbGeography radius = point.Buffer(radiusMeters * 1000);
                    IQueryable<GymEntity> gymsData = db.Gym.Where(r => SqlSpatialFunctions.Filter(r.coordinates, radius) == true);
                    IQueryable<PokestopEntity> pokestopsData = db.Pokestop.Where(r => SqlSpatialFunctions.Filter(r.coordinates, radius) == true);
                    IQueryable<RequestPokemonEntity> pokemonsData = db.RequestPokemon.Where(r => SqlSpatialFunctions.Filter(r.coordinates, radius) == true &&
                        r.expiration_date > DateTime.Now);
                    this.GetGyms(gyms, gymsData);
                    this.GetPokestops(pokestops, pokestopsData);
                    this.GetPokemons(pokemons, pokemonsData);
                    if (gyms.Count == 0 && pokestops.Count == 0 && pokemons.Count == 0)
                    {
                        return null;
                    }
                    nearbyModel = new NearbyModel
                    {
                        Gyms = gyms.ToArray(),
                        Pokestops = pokestops.ToArray(),
                        Pokemons = pokemons.ToArray()
                    };
                }
                catch (Exception)
                {
                    nearbyModel = null;
                }
            }
            return nearbyModel;
        }

        /// <summary>
        /// Gets the gyms.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="gyms">The gyms.</param>
        private void GetGyms(List<Gym> model, IQueryable<GymEntity> gyms)
        {
            if (!gyms.Any())
            {
                return;
            }
            foreach (GymEntity gym in gyms)
            {
                if (gym == null || model.Any(m => m.Latitude == gym.coordinates.Latitude.Value &&
                                                  m.Longitude == gym.coordinates.Longitude.Value))
                {
                    return;
                }
                model.Add(new Gym
                {
                    GymId = gym.gym_id,
                    Latitude = gym.coordinates.Latitude.Value,
                    Longitude = gym.coordinates.Longitude.Value,
                    Prestige = gym.prestige,
                    TeamName = gym.team_name
                });
            }
        }

        /// <summary>
        /// Gets the pokestops.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="pokestops">The pokestops.</param>
        private void GetPokestops(List<Pokestop> model, IQueryable<PokestopEntity> pokestops)
        {
            if (!pokestops.Any())
            {
                return;
            }
            foreach (PokestopEntity pokestop in pokestops)
            {
                if (pokestop == null || model.Any(m => m.Latitude == pokestop.coordinates.Latitude.Value &&
                                                       m.Longitude == pokestop.coordinates.Longitude.Value))
                {
                    return;
                }
                Pokestop entity = new Pokestop
                {
                    Latitude = pokestop.coordinates.Latitude.Value,
                    Longitude = pokestop.coordinates.Longitude.Value,
                    PokestopId = 1
                };
                if (pokestop.expiration_date != null)
                {
                    if (pokestop.expiration_date > DateTime.Now)
                    {
                        entity.Lure = new PokestopLure
                        {
                            ExpirationDateTime = this.DateTimeToUnixTime(pokestop.expiration_date.Value)
                        };
                        entity.PokestopId = 2;
                    }
                    
                }
                model.Add(entity);
            }
        }

        /// <summary>
        /// Gets the pokemons.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="pokemons">The pokemons.</param>
        private void GetPokemons(List<Pokemon> model, IQueryable<RequestPokemonEntity> pokemons)
        {
            if (!pokemons.Any())
            {
                return;
            }
            foreach (RequestPokemonEntity pokemon in pokemons)
            {
                if (pokemon == null || model.Any(m => m.Id == pokemon.Pokemon.id &&
                                                      m.Latitude == pokemon.coordinates.Latitude.Value &&
                                                      m.Longitude == pokemon.coordinates.Longitude.Value))
                {
                    return;
                }
                if (pokemon.expiration_date > DateTime.Now)
                {
                    model.Add(new Pokemon
                    {
                        ExpirationDate = this.DateTimeToUnixTime(pokemon.expiration_date),
                        Id = pokemon.Pokemon.id,
                        Latitude = pokemon.coordinates.Latitude.Value,
                        Longitude = pokemon.coordinates.Longitude.Value,
                        Name = pokemon.Pokemon.name
                    });
                }
            }
        }

        /// <summary>
        /// Inserts the nearby data.
        /// </summary>
        /// <param name="nearbyModel">The nearby model.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        public void InsertNearbyData(NearbyModel nearbyModel, double latitude, double longitude)
        {
            if (nearbyModel == null)
            {
                return;
            }
            using (PokeDBContext db = new PokeDBContext())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        this.HandleGymEntity(db, nearbyModel.Gyms);
                        this.HandlePokestopEntity(db, nearbyModel.Pokestops);
                        this.HandlePokemonEntity(db, nearbyModel.Pokemons);
                        db.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }

        /// <summary>
        /// Handles the gym entity.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="gyms">The gyms.</param>
        private void HandleGymEntity(PokeDBContext db, Gym[] gyms)
        {
            if (gyms == null)
            {
                return;
            }
            foreach (Gym gym in gyms)
            {
                this.InsertOrUpdateGymEntity(db, gym);
            }
        }

        /// <summary>
        /// Handles the pokestop entity.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="pokestops">The pokestops.</param>
        private void HandlePokestopEntity(PokeDBContext db, Pokestop[] pokestops)
        {
            if (pokestops == null)
            {
                return;
            }
            foreach (Pokestop pokestop in pokestops)
            {
                this.InsertOrUpdatePokestopEntity(db, pokestop);
            }
        }

        /// <summary>
        /// Handles the pokemon entity.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="pokemons">The pokemons.</param>
        private void HandlePokemonEntity(PokeDBContext db, Pokemon[] pokemons)
        {
            if (pokemons == null)
            {
                return;
            }
            foreach (Pokemon pokemon in pokemons)
            {
                this.InsertOrUpdateRequestPokemonEntity(db, pokemon);
            }
        }

        /// <summary>
        /// Gets the gym entity.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="gym">The gym.</param>
        /// <returns>Gym request</returns>
        private GymEntity GetGymEntity(PokeDBContext db, Gym gym)
        {
            DbGeography point = this.ConvertGPSToGeography(gym.Latitude, gym.Longitude);
            return db.Gym.FirstOrDefault(g => SqlSpatialFunctions.Filter(g.coordinates, point) == true);
        }

        /// <summary>
        /// Gets the pokestop entity.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="pokestop">The pokestop.</param>
        /// <returns>Pokestop entity</returns>
        private PokestopEntity GetPokestopEntity(PokeDBContext db, Pokestop pokestop)
        {
            DbGeography point = this.ConvertGPSToGeography(pokestop.Latitude, pokestop.Longitude);
            return db.Pokestop.FirstOrDefault(p => SqlSpatialFunctions.Filter(p.coordinates, point) == true);
        }

        /// <summary>
        /// Gets the request pokemon entity.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="pokemon">The pokemon.</param>
        /// <returns>Request pokemon entity</returns>
        private RequestPokemonEntity GetRequestPokemonEntity(PokeDBContext db, Pokemon pokemon)
        {
            DbGeography point = this.ConvertGPSToGeography(pokemon.Latitude, pokemon.Longitude);
            return db.RequestPokemon.FirstOrDefault(p => p.pokemon_id == pokemon.Id && 
                                                    SqlSpatialFunctions.Filter(p.coordinates, point) == true);
        }

        /// <summary>
        /// Gets the pokemon entity.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="pokemon">The pokemon.</param>
        /// <returns>Pokemon entity</returns>
        private PokemonEntity GetPokemonEntity(PokeDBContext db, Pokemon pokemon)
        {
            return db.Pokemon.Find(pokemon.Id);
        }

        /// <summary>
        /// Inserts the or update gym entity.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="gym">The gym.</param>
        private void InsertOrUpdateGymEntity(PokeDBContext db, Gym gym)
        {
            GymEntity gymEntity = this.GetGymEntity(db, gym);
            if (gymEntity != null)
            {
                gymEntity.gym_id = gym.GymId;
                gymEntity.prestige = gym.Prestige;
                gymEntity.team_name = gym.TeamName;
            }
            else
            {
                gymEntity = new GymEntity
                {
                    coordinates = this.ConvertGPSToGeography(gym.Latitude, gym.Longitude),
                    gym_id = gym.GymId,
                    prestige = gym.Prestige,
                    team_name = gym.TeamName
                };
                db.Gym.Add(gymEntity);
            }
        }

        /// <summary>
        /// Inserts the or update pokestop entity.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="pokestop">The pokestop.</param>
        private void InsertOrUpdatePokestopEntity(PokeDBContext db, Pokestop pokestop)
        {
            PokestopEntity pokestopEntity = this.GetPokestopEntity(db, pokestop);
            if (pokestopEntity != null)
            {
                pokestopEntity.expiration_date = pokestop.Lure == null
                    ? (DateTime?) null : this.UnixTimeStampToDateTime(pokestop.Lure.ExpirationDateTime);
                pokestopEntity.pokestop_id = pokestop.PokestopId;
            }
            else
            {
                pokestopEntity = new PokestopEntity
                {
                    coordinates = this.ConvertGPSToGeography(pokestop.Latitude, pokestop.Longitude),
                    expiration_date = pokestop.Lure == null 
                        ? (DateTime?)null : this.UnixTimeStampToDateTime(pokestop.Lure.ExpirationDateTime),
                    pokestop_id = pokestop.PokestopId
                };
                db.Pokestop.Add(pokestopEntity);
            }
        }

        /// <summary>
        /// Inserts the or update request pokemon entity.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="pokemon">The pokemon.</param>
        private void InsertOrUpdateRequestPokemonEntity(PokeDBContext db, Pokemon pokemon)
        {
            PokemonEntity pokemonEntity = this.InsertPokemonEntity(db, pokemon);
            RequestPokemonEntity requestPokemonEntity = this.GetRequestPokemonEntity(db, pokemon);
            if (requestPokemonEntity != null)
            {
                requestPokemonEntity.expiration_date = this.UnixTimeStampToDateTime(pokemon.ExpirationDate);
            }
            else
            {
                requestPokemonEntity = new RequestPokemonEntity
                {
                    coordinates = this.ConvertGPSToGeography(pokemon.Latitude, pokemon.Longitude),
                    expiration_date = this.UnixTimeStampToDateTime(pokemon.ExpirationDate),
                    Pokemon = pokemonEntity
                };
                db.RequestPokemon.Add(requestPokemonEntity);
            }
        }

        /// <summary>
        /// Inserts the pokemon entity.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="pokemon">The pokemon.</param>
        /// <returns>Pokemon entity</returns>
        private PokemonEntity InsertPokemonEntity(PokeDBContext db, Pokemon pokemon)
        {
            PokemonEntity pokemonEntity = this.GetPokemonEntity(db, pokemon);
            if (pokemonEntity != null)
            {
                return pokemonEntity;
            }
            pokemonEntity = new PokemonEntity
            {
                id = pokemon.Id,
                name = pokemon.Name
            };
            db.Pokemon.Add(pokemonEntity);
            return pokemonEntity;
        }

        /// <summary>
        /// Converts the GPS to geography.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns>Geography</returns>
        private DbGeography ConvertGPSToGeography(double latitude, double longitude)
        {
            return DbGeography.PointFromText(
               $"POINT({longitude.ToString(CultureInfo.InvariantCulture)} {latitude.ToString(CultureInfo.InvariantCulture)})", 4326);
        }
        
        /// <summary>
        /// Unixes the time stamp to date time.
        /// </summary>
        /// <param name="unixTimeStamp">The unix time stamp.</param>
        /// <returns>Date time</returns>
        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        /// <summary>
        /// Dates the time to unix time.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>Unix time</returns>
        public long DateTimeToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalMilliseconds);
        }
    }
}
