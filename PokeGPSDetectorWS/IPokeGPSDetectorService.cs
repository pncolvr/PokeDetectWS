using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using PokeGPSModel.Model;

namespace PokeGPSDetectorWS
{
    
    [ServiceContract]
    public interface IPokeGPSDetectorService
    {
        /// <summary>
        /// Gets the interest points.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns>NearbyEntity data</returns>
        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "/GetInterestPoints")]
        NearbyModel GetInterestPoints(String username, String password, double latitude, double longitude);

        /// <summary>
        /// Gets the gym image.
        /// </summary>
        /// <param name="gymId">The gym identifier.</param>
        /// <returns>Image</returns>
        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "/GetGymImage")]
        Image.Model.Image GetGymImage(String gymId);

        /// <summary>
        /// Gets the pokestop image.
        /// </summary>
        /// <param name="pokestopeId">The pokestope identifier.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "/GetPokestopImage")]
        Image.Model.Image GetPokestopImage(String pokestopeId);

        /// <summary>
        /// Gets the pokemon image.
        /// </summary>
        /// <param name="pokemonId">The pokemon identifier.</param>
        /// <returns>Image</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", 
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Wrapped,
                   UriTemplate = "/GetPokemonImage")]
        Image.Model.Image GetPokemonImage(String pokemonId);

        /// <summary>
        /// Performs the login.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Wrapped,
                   UriTemplate = "/PerformLogin")]
        Boolean PerformLogin(String username, String password);
    }
}
