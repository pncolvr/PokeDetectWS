using System;
using System.IO;
using System.Web.Hosting;

namespace PokeGPSDetectorWS.Image.Controller
{
    public class ImageController
    {
        /// <summary>
        /// Gets the gym image.
        /// </summary>
        /// <param name="gymId">The gym identifier.</param>
        /// <returns>Images</returns>
        public Model.Image GetGymImage(String gymId)
        {
            return new Model.Image
            {
                Base64 = GetImage(@"Images\Gyms\", gymId, "png"),
                FileName = $"{gymId}.png"
            };
        }

        /// <summary>
        /// Gets the pokestop image.
        /// </summary>
        /// <param name="pokestopId">The pokestop identifier.</param>
        /// <returns></returns>
        public Model.Image GetPokestopImage(String pokestopId)
        {
            return new Model.Image
            {
                Base64 = GetImage(@"Images\Pokestops\", pokestopId, "png"),
                FileName = $"{pokestopId}.png"
            };
        }

        /// <summary>
        /// Gets the pokemon image.
        /// </summary>
        /// <param name="pokemonId">The pokemon identifier.</param>
        /// <returns>Images</returns>
        public Model.Image GetPokemonImage(String pokemonId)
        {
            return new Model.Image
            {
                Base64 = GetImage(@"Images\Pokemons\", pokemonId, "png"),
                FileName = $"{pokemonId}.png"
            };
        }

        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="imageName">Name of the image.</param>
        /// <param name="extension">The extension.</param>
        /// <returns></returns>
        private String GetImage(String path, String imageName, String extension)
        {
            return Convert.ToBase64String(File.ReadAllBytes($@"{HostingEnvironment.ApplicationPhysicalPath}{path}\{imageName}.{extension}"));
        } 
    }
}