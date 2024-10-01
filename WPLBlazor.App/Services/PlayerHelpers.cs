﻿using WPLBlazor.App.Models;

namespace WPLBlazor.App.Services
{
    public class PlayerHelpers
    {
        private readonly APIService aPIService = new();
        public async Task<List<Players>> ConsolidatePlayer()
        {
            var pList = new List<Players>();

            var players = await aPIService.GetAllPlayers();

            if (players.Count == 0)
            {
                return null;
            }
            else
            {
                foreach (var item in players)
                {
                    Players playerTotals = new();
                    List<PlayerData> getPlayerData = [];
                    getPlayerData = await aPIService.GetPlayerData(item.Id);

                    if (getPlayerData.Count != 0)
                    {
                        playerTotals.Id = getPlayerData.FirstOrDefault().PlayerId;
                        playerTotals.TeamId = item.TeamId;
                        playerTotals.FirstName = item.FirstName;
                        playerTotals.LastName = item.LastName;
                        playerTotals.GamesWon = getPlayerData.Sum(x => x.GamesWon);
                        playerTotals.GamesLost = getPlayerData.Sum(y => y.GamesLost);
                        playerTotals.GamesPlayed = playerTotals.GamesWon + playerTotals.GamesLost;
                        playerTotals.Average = Decimal.Round(((decimal)playerTotals.GamesWon / (decimal)playerTotals.GamesPlayed) * 100, 2);
                        playerTotals.WeekNumber = getPlayerData.Count;
                    }
                    pList.Add(playerTotals);
                }

            }
            return pList;
        }

        public async Task<Players> GetPlayerDetails(int id)
        {
            Players playerTotals = new();
            List<PlayerData> getPlayerData = await aPIService.GetPlayerData(id);
            var playerInfo = await aPIService.GetSinglePlayer(id);
            if (getPlayerData.Count > 0)
            {
                playerTotals.Id = getPlayerData.FirstOrDefault().PlayerId;
                playerTotals.FirstName = playerInfo.FirstName;
                playerTotals.LastName = playerInfo.LastName;
                playerTotals.GamesWon = getPlayerData.Sum(gw => gw.GamesWon);
                playerTotals.GamesLost = getPlayerData.Sum(y => y.GamesLost);
                playerTotals.GamesPlayed = playerTotals.GamesWon + playerTotals.GamesLost;
                playerTotals.Average = Decimal.Round(((decimal)playerTotals.GamesWon / (decimal)playerTotals.GamesPlayed) * 100, 2);
                playerTotals.WeekNumber = getPlayerData.Count;
            }

            return playerTotals;
        }

        public async Task<TeamStats> GetTeamStats()
        {
            List<PlayerData> teamTotals = [];
            List<Weeks> weekTotals = [];
            weekTotals = (List<Weeks>)await aPIService.GetAllWeeks();
            teamTotals = await aPIService.GetAllPlayerData();
            TeamStats teamStats = new();
            teamStats.TotalGamesWon = teamTotals.Sum(x => x.GamesWon);
            teamStats.TotalGamesLost = teamTotals.Sum(y => y.GamesLost);
            teamStats.TotalGamesPlayed = teamStats.TotalGamesWon + teamStats.TotalGamesLost;
            teamStats.TotalAverage = Decimal.Round(((decimal)teamStats.TotalGamesWon / (decimal)teamStats.TotalGamesPlayed) * 100, 2);
            teamStats.WeeksPlayed = weekTotals.Count;

            return teamStats;
        }


        public async Task<List<TeamDetails>> GetTeamDetails()
        {
            List<TeamDetails> teamDetails = [];
            teamDetails = await aPIService.GetTeamDetails();

            return teamDetails;
        }
    }
}
