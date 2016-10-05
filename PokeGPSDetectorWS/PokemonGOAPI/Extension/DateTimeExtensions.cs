using System;

namespace PokeGPSDetectorWS.PokemonGOAPI.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// To the unix time.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>Unix time</returns>
        public static long ToUnixTime(this DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalMilliseconds);
        }
    }
}