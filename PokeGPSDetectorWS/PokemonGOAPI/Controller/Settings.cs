using System;
using System.Collections.Generic;
using AllEnum;
using PokemonGo.RocketAPI.Enums;
using System.Web.Configuration;

namespace PokeGPSDetectorWS.PokemonGOAPI.Controller
{
    internal class Settings : PokemonGo.RocketAPI.ISettings
    {
        public AuthType AuthType => PokemonGo.RocketAPI.Enums.AuthType.Ptc;

        public String PtcUsername => WebConfigurationManager.AppSettings["username"];

        public String PtcPassword => WebConfigurationManager.AppSettings["password"];

        public double DefaultLatitude => double.Parse(WebConfigurationManager.AppSettings["default_latitude"]);

        public double DefaultLongitude => double.Parse(WebConfigurationManager.AppSettings["default_longitude"]);      

        public string GoogleRefreshToken
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public ICollection<KeyValuePair<ItemId, int>> itemRecycleFilter
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}