using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using PokeGPSDatabase.Database.Controller;
using PokeGPSDetectorWS.Handler;
using PokeGPSDetectorWS.Image.Controller;
using PokeGPSDetectorWS.PokemonGOAPI.Controller;
using PokeGPSModel.Model;

namespace PokeGPSDetectorWS
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    public class PokeGPSDetectorService : IPokeGPSDetectorService
    {
        /// <summary>
        /// The helper
        /// </summary>
        private PokemonGOWrapper Helper {
            get
            {
                if (HttpContext.Current.Session["Helper"] != null && HttpContext.Current.Session["Helper"] is PokemonGOWrapper)
                {
                    return HttpContext.Current.Session["Helper"] as PokemonGOWrapper;
                }
                return null;
            }
            set
            {
                HttpContext.Current.Session["Helper"] = value;
            }
        }

        /// <summary>
        /// The last login
        /// </summary>
        private DateTime? LastLogin {
            get
            {
                if (HttpContext.Current.Session["LastLogin"] != null && HttpContext.Current.Session["LastLogin"] is DateTime?)
                {
                    return HttpContext.Current.Session["LastLogin"] as DateTime?;
                }
                return null;
            }
            set
            {
                HttpContext.Current.Session["LastLogin"] = value;
            }
        }

        /// <summary>
        /// Gets the interest points.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public NearbyModel GetInterestPoints(String username, String password, double latitude, double longitude)
        {
            return this.GetDataPrivate(username, password, latitude, longitude);
        }

        /// <summary>
        /// Gets the data private.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns>Nearby model</returns>
        private NearbyModel GetDataPrivate(String username, String password, double latitude, double longitude)
        {
            try
            {
                RequestHandler request = new RequestHandler(this.Helper, this.LastLogin);
                NearbyModel model = request.GetData(latitude, longitude, username, password);
                this.Helper = request.Helper;
                this.LastLogin = request.LastLogin;
                return model;
            }
            catch
            {
                this.Helper = null;
                this.LastLogin = null;
                return new DBController().GetNearbyData(latitude, longitude,
                        int.Parse(WebConfigurationManager.AppSettings["radius_kilometers"]));
            }
        }

        /// <summary>
        /// Performs the login.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>True if success, false otherwise</returns>
        public Boolean PerformLogin(String username, String password)
        {
            try
            {
                RequestHandler request = new RequestHandler(this.Helper, this.LastLogin);
                request.PerformLogin(username, password);
                this.Helper = request.Helper;
                this.LastLogin = request.LastLogin;
                return true;
            }
            catch
            {
                this.Helper = null;
                this.LastLogin = null;
                return false;
            }
        }

        /// <summary>
        /// Gets the fort image.
        /// </summary>
        /// <param name="gymId">The gym identifier.</param>
        /// <returns>Image</returns>
        public Image.Model.Image GetGymImage(String gymId)
        {
            try
            {
                return new ImageController().GetGymImage(gymId);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the pokestop image.
        /// </summary>
        /// <param name="pokestopeId">The pokestope identifier.</param>
        /// <returns>Image</returns>
        public Image.Model.Image GetPokestopImage(String pokestopeId)
        {
            try
            {
                return new ImageController().GetPokestopImage(pokestopeId);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the pokemon image.
        /// </summary>
        /// <param name="pokemonId">The pokemon identifier.</param>
        /// <returns>Image</returns>
        public Image.Model.Image GetPokemonImage(String pokemonId)
        {
            try
            {
                return new ImageController().GetPokemonImage(pokemonId);
            }
            catch
            {
                return null;
            }
        }
    }
}