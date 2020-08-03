using CapsBallShared;
using nDSSH;
using System.Collections.Generic;

namespace CapsBallServer
{
    public static class ResponseCaller
    {
        public static void ResponseAdminAdded(Player player) =>
            TeamsHandler.Broadcast(new ResponsePackage(ResponseCommand.ADMIN_ADDED));

        public static void ResponseTeamData(Team team)
        {
            List<string> parameters = new List<string>();
            parameters.Add(team.Name);
            parameters.Add(team.GetCount().ToString());
            List<Player> players = team.GetPlayers();
            foreach (Player p in players)
                parameters.Add(p.Account.Nick);

            //ResponsePackage package = new ResponsePackage(ResponseCommand.TEAM_DATA, parameters);
            //broadcastToTeam(team, package);
        }

        public static void ResponseGameStarted(string starterNick)
        {
            List<string> parameters = new List<string>(new string[] { starterNick });
            ResponsePackage package = new ResponsePackage(ResponseCommand.GAME_STARTED, parameters);
            TeamsHandler.Broadcast(package);
        }
    }
}