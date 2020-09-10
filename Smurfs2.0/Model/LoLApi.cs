using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MingweiSamuel.Camille;
using MingweiSamuel.Camille.Enums;
using MingweiSamuel.Camille.LeagueV4;
using MingweiSamuel.Camille.SummonerV4;
using MingweiSamuel.Camille.Util;

namespace Smurfs2._0
{

    /// <summary>
    /// LoLApi class
    /// used to fetch various information from the Riot Games Api
    /// </summary>
    /// <remarks>
    /// this class is a singleton class, hence it cannot be instantiated, but a single instance can be grabbed by using the static methode getInstance()
    /// </remarks>
    /// <seealso cref="https://en.wikipedia.org/wiki/Singleton_pattern"/>
    /// <author>
    /// Henry Bust
    /// </author>
    class LoLApi
    {

        //Attributes
        /// <summary>
        /// contains the only instance of the LoLApi class
        /// </summary>
        private static LoLApi instance = null;
        /// <summary>
        /// contains the RiotApi hook used to request any Riot Games Api
        /// </summary>
        private RiotApi api = null;
        /// <summary>
        /// contains the unique apiKey that is used to establish a connection to the Riot Games Api
        /// </summary>
        private string apiKey = null;

        //Constants
        /// <summary>
        /// string used in the Riot Games Api to describe the flex queue
        /// defined as a constant incase these names get changed in the Riot Games Api
        /// </summary>
        public const string QUEUE_TYPE_FLEX = "RANKED_FLEX_SR";
        /// <summary>
        /// string used in the Riot Games Api to describe the solo queue.
        /// defined as a constant incase these names get changed in the Riot Games Api
        /// </summary>
        public const string QUEUE_TYPE_SOLO = "RANKED_SOLO_5x5";

        //Constructors
        private LoLApi(string apiKey)
        {
            this.setApiKey(apiKey);
            this.setApi(RiotApi.NewInstance(this.getApiKey()));
        }

        //Selectors
        public void setApiKey(string apiKey)
        {
            this.apiKey = apiKey;
        }
        private string getApiKey()
        {
            return this.apiKey;
        }

        private RiotApi getApi()
        {
            return this.api;
        }
        private void setApi(RiotApi api)
        {
            this.api = api;
        }

        /// <summary>
        /// selector of the singleton instance of LoLApi
        /// </summary>
        /// <returns cref="LoLApi">
        /// the only object of the class LoLApi
        /// </returns>
        /// <exception cref="ApiNotInitializableException">
        /// thrown if the api could not be initialized
        /// </exception>
        public static LoLApi getInstance()
        {
            if (LoLApi.instance == null)
            {
                try
                {
                    string apiKey = Smurfs2._0.Properties.Resources.apiKey;
                    LoLApi.instance = new LoLApi(apiKey);
                    return LoLApi.instance;
                }
                catch (Exception)
                {
                    throw new ApiNotInitializableException("There was an Error while initializing the LoLApi. Check if the Api Key is correct and following path exists: /config/apiKey.txt");
                }
            }
            return LoLApi.instance;
        }

        //Functions
        public Region getRegion(string regionName)
        {
            switch (regionName)
            {
                case "br":
                    return Region.BR;
                case "eune":
                    return Region.EUNE;
                case "euw":
                    return Region.EUW;
                case "lan":
                    return Region.LAN;
                case "las":
                    return Region.LAS;
                case "na":
                    return Region.NA;
                case "oce":
                    return Region.OCE;
                case "ru":
                    return Region.RU;
                case "tr":
                    return Region.TR;
                case "jp":
                    return Region.JP;
                case "kr":
                    return Region.KR;
                case "garena":
                    return Region.Garena;
                case "pbe":
                    return Region.PBE;
                default:
                    throw new ApiInvalidRegionException("The chosen Region is not a valid Region");
            }
        }
        private bool regionIsset(Region? region)
        {
            if(region.HasValue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Region getStandardRegion()
        {
            string standardRegion = Smurfs2._0.Properties.Resources.standardRegion.ToLower();
            switch(standardRegion)
            {
                case "br":
                    return Region.BR;
                case "eune":
                    return Region.EUNE;
                case "euw":
                    return Region.EUW;
                case "lan":
                    return Region.LAN;
                case "las":
                    return Region.LAS;
                case "na":
                    return Region.NA;
                case "oce":
                    return Region.OCE;
                case "ru":
                    return Region.RU;
                case "tr":
                    return Region.TR;
                case "jp":
                    return Region.JP;
                case "kr":
                    return Region.KR;
                default:
                    throw new ApiInvalidRegionException("The chosen Region is not a valid Region");
            }
        }
        private Region validateRegion(Region? region)
        {
            if (regionIsset(region))
            {
                return region.Value;
            }
            return getStandardRegion();
        }

        /// <summary>
        /// requests summoner information from the Riotgames SummonerV4 Api
        /// </summary>
        /// <param name="summonerNames" cref="string">
        /// the summoner name of the summoner you want to get the id from
        /// </param>
        /// <param name="region" cref="Region">
        /// the region of the summoner
        /// </param>
        /// <returns cref="Summoner">
        /// an object of the class Summoner containing the requested information about the summoner
        /// </returns>
        /// <exception cref="ApiInvalidSummonerNameException">
        /// thrown if the summonername is invalid or the searched summoner does not exist in the given region
        /// </exception>
        /// <exception cref="ApiCouldNotBeReachedException">
        /// thrown if the Riot Games Api was not reached or the Api key is invalid
        /// </exception>
        public Summoner getSummoner(string summonerName, Region? region = null)
        {
            Summoner summoner = null;
            try
            {
                summoner = this.getApi().SummonerV4.GetBySummonerName(validateRegion(region), summonerName);
            }
            catch (AggregateException ae)
            {
                ae.Handle(ex =>
                {
                    if(ex is RiotResponseException)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    throw new ApiCouldNotBeReachedException();
                });
            }
            if(summoner == null)
            {
                throw new ApiInvalidSummonerNameException("The summoner you searched for does not exists, or at least does not exist in the chosen region: " + validateRegion(region).ToString());
            }
            else
            {
                return summoner;
            }

        }

        /// <summary>
        /// requests the summoner level from the Riotgames SummonerV4 Api
        /// </summary>
        /// <param name="summonerNames" cref="string">
        /// the summoner name of the summoner you want to get the id from
        /// </param>
        /// <param name="region" cref="Region">
        /// the region of the summoner
        /// </param>
        /// <returns cref="long">
        /// the level of the summoner
        /// </returns>
        /// <exception cref="ApiInvalidSummonerNameException">
        /// thrown if the summonername is invalid or the searched summoner does not exist in the given region
        /// </exception>
        /// <exception cref="ApiCouldNotBeReachedException">
        /// thrown if the Riot Games Api was not reached or the Api key is invalid
        /// </exception>
        public long getLevel(string summonerName, Region? region = null)
        {
            Summoner summoner = this.getSummoner(summonerName, validateRegion(region));
            return summoner.SummonerLevel;
        }

        /// <summary>
        /// requests the summoner id from the Riotgames SummonerV4 Api
        /// </summary>
        /// <param name="summonerNames" cref="string">
        /// the summoner name of the summoner you want to get the id from
        /// </param>
        /// <param name="region" cref="Region">
        /// the region of the summoner
        /// </param>
        /// <returns cref="string">
        /// the encrypted id of the summoner
        /// </returns>
        /// <exception cref="ApiInvalidSummonerNameException">
        /// thrown if the summonername is invalid or the searched summoner does not exist in the given region
        /// </exception>
        /// <exception cref="ApiCouldNotBeReachedException">
        /// thrown if the Riot Games Api was not reached or the Api key is invalid
        /// </exception>
        public string getId(string summonerName, Region? region = null)
        {
            Summoner summoner = this.getSummoner(summonerName, validateRegion(region));
            return summoner.Id;
        }

        /// <summary>
        /// requests the league entries from the Riotgames LeagueV4 Api
        /// </summary>
        /// <param name="summonerNames" cref="string">
        /// the summoner name of the summoner you want to get the league entries from
        /// </param>
        /// <param name="region" cref="Region">
        /// the region of the summoner
        /// </param>
        /// <returns cref="LeagueEntry[]">
        /// an array of the league entries of the summoner
        /// </returns>
        /// <exception cref="ApiInvalidSummonerNameException">
        /// thrown if the summonername is invalid or the searched summoner does not exist in the given region
        /// </exception>
        /// <exception cref="ApiCouldNotBeReachedException">
        /// thrown if the Riot Games Api was not reached or the Api key is invalid
        /// </exception>
        public LeagueEntry[] getLeagueEntries(string summonerName, Region? region = null)
        {
            return this.getApi().LeagueV4.GetLeagueEntriesForSummoner(validateRegion(region), this.getId(summonerName, validateRegion(region)));
        }

        /// <summary>
        /// requests the flex rank from the Riotgames LeagueV4 Api
        /// </summary>
        /// <param name="summonerNames" cref="string">
        /// the summoner name of the summoner you want to get the flex rank from
        /// </param>
        /// <param name="region" cref="Region">
        /// the region of the summoner
        /// </param>
        /// <returns cref="string">
        /// the flex rank of the summoner
        /// </returns>
        /// <exception cref="ApiInvalidSummonerNameException">
        /// thrown if the summonername is invalid or the searched summoner does not exist in the given region
        /// </exception>
        /// <exception cref="ApiCouldNotBeReachedException">
        /// thrown if the Riot Games Api was not reached or the Api key is invalid
        /// </exception>
        public string getFlexRank(string summonerName, Region? region = null)
        {
            foreach (LeagueEntry entry in this.getLeagueEntries(summonerName, validateRegion(region)))
            {
                if (entry.QueueType == LoLApi.QUEUE_TYPE_FLEX)
                {
                    return entry.Rank;
                }
            }
            return null;
        }

        /// <summary>
        /// requests the solo rank from the Riotgames LeagueV4 Api
        /// </summary>
        /// <param name="summonerNames" cref="string">
        /// the summoner name of the summoner you want to get the solo rank from
        /// </param>
        /// <param name="region" cref="Region">
        /// the region of the summoner
        /// </param>
        /// <returns cref="string">
        /// the solo rank of the summoner
        /// </returns>
        /// <exception cref="ApiInvalidSummonerNameException">
        /// thrown if the summonername is invalid or the searched summoner does not exist in the given region
        /// </exception>
        /// <exception cref="ApiCouldNotBeReachedException">
        /// thrown if the Riot Games Api was not reached or the Api key is invalid
        /// </exception>
        public string getSoloRank(string summonerName, Region? region = null)
        {
            foreach (LeagueEntry entry in this.getLeagueEntries(summonerName, validateRegion(region)))
            {
                if (entry.QueueType == LoLApi.QUEUE_TYPE_SOLO)
                {
                    return entry.Rank;
                }
            }
            return null;
        }

        /// <summary>
        /// requests the flex tier from the Riotgames LeagueV4 Api
        /// </summary>
        /// <param name="summonerNames" cref="string">
        /// the summoner name of the summoner you want to get the flex tier from
        /// </param>
        /// <param name="region" cref="Region">
        /// the region of the summoner
        /// </param>
        /// <returns cref="string">
        /// the flex tier of the summoner
        /// </returns>
        /// <exception cref="ApiInvalidSummonerNameException">
        /// thrown if the summonername is invalid or the searched summoner does not exist in the given region
        /// </exception>
        /// <exception cref="ApiCouldNotBeReachedException">
        /// thrown if the Riot Games Api was not reached or the Api key is invalid
        /// </exception>
        public string getFlexTier(string summonerName, Region? region = null)
        {
            foreach (LeagueEntry entry in this.getLeagueEntries(summonerName, validateRegion(region)))
            {
                if (entry.QueueType == LoLApi.QUEUE_TYPE_FLEX)
                {
                    return entry.Tier;
                }
            }
            return null;
        }

        /// <summary>
        /// requests the solo tier from the Riotgames LeagueV4 Api
        /// </summary>
        /// <param name="summonerNames" cref="string">
        /// the summoner name of the summoner you want to get the solo tier from
        /// </param>
        /// <param name="region" cref="Region">
        /// the region of the summoner
        /// </param>
        /// <returns cref="string">
        /// the solo tier of the summoner
        /// </returns>
        /// <exception cref="ApiInvalidSummonerNameException">
        /// thrown if the summonername is invalid or the searched summoner does not exist in the given region
        /// </exception>
        /// <exception cref="ApiCouldNotBeReachedException">
        /// thrown if the Riot Games Api was not reached or the Api key is invalid
        /// </exception>
        public string getSoloTier(string summonerName, Region? region = null)
        {
            foreach (LeagueEntry entry in this.getLeagueEntries(summonerName, validateRegion(region)))
            {
                if (entry.QueueType == LoLApi.QUEUE_TYPE_SOLO)
                {
                    return entry.Rank;
                }
            }
            return null;
        }

        /// <summary>
        /// requests the flex lp from the Riotgames LeagueV4 Api
        /// </summary>
        /// <param name="summonerNames" cref="string">
        /// the summoner name of the summoner you want to get the flex lp from
        /// </param>
        /// <param name="region" cref="Region">
        /// the region of the summoner
        /// </param>
        /// <returns cref="long">
        /// the flex lp of the summoner
        /// </returns>
        /// <exception cref="ApiInvalidSummonerNameException">
        /// thrown if the summonername is invalid or the searched summoner does not exist in the given region
        /// </exception>
        /// <exception cref="ApiCouldNotBeReachedException">
        /// thrown if the Riot Games Api was not reached or the Api key is invalid
        /// </exception>
        public long? getFlexLp(string summonerName, Region? region = null)
        {
            foreach (LeagueEntry entry in this.getLeagueEntries(summonerName, validateRegion(region)))
            {
                if (entry.QueueType == LoLApi.QUEUE_TYPE_FLEX)
                {
                    return entry.LeaguePoints;
                }
            }
            return null;
        }

        /// <summary>
        /// requests the solo lp from the Riotgames LeagueV4 Api
        /// </summary>
        /// <param name="summonerNames" cref="string">
        /// the summoner name of the summoner you want to get the solo lp from
        /// </param>
        /// <param name="region" cref="Region">
        /// the region of the summoner
        /// </param>
        /// <returns cref="long">
        /// the solo lp of the summoner
        /// </returns>
        /// <exception cref="ApiInvalidSummonerNameException">
        /// thrown if the summonername is invalid or the searched summoner does not exist in the given region
        /// </exception>
        /// <exception cref="ApiCouldNotBeReachedException">
        /// thrown if the Riot Games Api was not reached or the Api key is invalid
        /// </exception>
        public long? getSoloLp(string summonerName, Region? region = null)
        {
            foreach (LeagueEntry entry in this.getLeagueEntries(summonerName, validateRegion(region)))
            {
                if (entry.QueueType == LoLApi.QUEUE_TYPE_SOLO)
                {
                    return entry.LeaguePoints;
                }
            }
            return null;
        }

    }
}
