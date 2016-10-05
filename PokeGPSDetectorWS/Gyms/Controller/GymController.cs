using System;
using PokemonGo.RocketAPI.GeneratedCode;
using AllEnum;
using PokeGPSModel.Model;

namespace PokeGPSDetectorWS.Gyms.Controller
{
    internal class GymController
    {
        /// <summary>
        /// Parses the gym object.
        /// </summary>
        /// <param name="gym">The gym.</param>
        /// <returns>
        /// GymEntity model
        /// </returns>
        public Gym ParseGymObject(FortData gym)
        {
            if (gym == null)
            {
                return null;
            }
            Gym model = new Gym
            {
                Latitude = gym.Latitude,
                Longitude = gym.Longitude,
                Prestige = gym.GymPoints,
                TeamName = this.GetTeamName(gym.OwnedByTeam),
                GymId = (int)gym.OwnedByTeam
            };
            return model;
        }

        /// <summary>
        /// Gets the name of the team.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <returns>
        /// Team name
        /// </returns>
        private String GetTeamName(TeamColor team)
        {
            switch (team)
            {
                case TeamColor.Blue:
                    return "Team Mystic";
                case TeamColor.Red:
                    return "Team Valor";
                case TeamColor.Yellow:
                    return "Team Instinct";
                default :
                    return "None";
            }
        }
    }
}
