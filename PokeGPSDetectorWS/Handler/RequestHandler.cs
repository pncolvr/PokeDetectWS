using System;
using System.Web.Configuration;
using PokeGPSDatabase.Database.Controller;
using PokeGPSDetectorWS.PokemonGOAPI.Controller;
using PokeGPSModel.Model;

namespace PokeGPSDetectorWS.Handler
{
    internal class RequestHandler
    {
        /// <summary>
        /// The helper
        /// </summary>
        public PokemonGOWrapper Helper { get; set; }

        /// <summary>
        /// The last login
        /// </summary>
        public DateTime? LastLogin { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestHandler"/> class.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="lastLogin">The last login.</param>
        public RequestHandler(PokemonGOWrapper helper, DateTime ?lastLogin)
        {
            this.Helper = helper;
            this.LastLogin = lastLogin;
        }
        
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>NearbyEntity data</returns>
        public NearbyModel GetData(double latitude, double longitude, String username, String password)
        {
            if (this.Helper == null || this.LastLogin == null || DateTime.Now >= 
                this.LastLogin.Value.AddMinutes(int.Parse(WebConfigurationManager.AppSettings["last_login_minutes"])))
            {
                this.PerformLogin(username, password);
            }
            return this.GetNearbyData(latitude, longitude);
        }

        /// <summary>
        /// Performs the login.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public void PerformLogin(String username, String password)
        {
            this.Helper = new PokemonGOWrapper();
            this.Helper.Login(username, password);
            LastLogin = DateTime.Now;
        }

        /// <summary>
        /// Gets the nearby data.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns>NearbyEntity data</returns>
        private NearbyModel GetNearbyData(double latitude, double longitude)
        {
            NearbyModel nearbyModel = this.Helper.GetNearbyData(latitude, longitude).Result;
            if (nearbyModel == null)
            {
                return new DBController().GetNearbyData(latitude, longitude,
                    int.Parse(WebConfigurationManager.AppSettings["radius_kilometers"]));
            }
            new DBController().InsertNearbyData(nearbyModel, latitude, longitude);
            NearbyModel dbNearbyModel = new DBController().GetNearbyData(latitude, longitude, 
                int.Parse(WebConfigurationManager.AppSettings["radius_kilometers"]));
            return dbNearbyModel ?? nearbyModel;
        }
    }
}