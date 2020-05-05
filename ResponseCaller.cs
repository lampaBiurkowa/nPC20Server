using CapsBallShared;
using nDSSH;
using System.Collections.Generic;

namespace CapsBallServer
{
    public static class ResponseCaller
    {
        public static void ResponseTeamData(Team team)
        {
            List<string> parameters = new List<string>();
            parameters.Add(team.Name);
            parameters.Add(team.GetCount().ToString());
            parameters.Add(team.TargetCount.ToString());
            List<Player> players = team.GetPlayers();
            foreach (Player p in players)
                parameters.Add(p.Account.Nick);

            //ResponsePackage package = new ResponsePackage(ResponseCommand.TEAM_DATA, parameters);
            //broadcastToTeam(team, package);
        }

        static void broadcastToTeam(Team team, ResponsePackage package)
        {
            List<Player> players = team.GetPlayers();
            for (int i = 0; i < players.Count; i++)
                if (IdResolver.UserExists(players[i].Account.Nick))
                    Sender.SendFeedbackByAlias(package.GetRawData(), players[i].Account.Nick);
        }

    }
}