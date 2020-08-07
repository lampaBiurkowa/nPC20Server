using CapsBallShared;
using GeoLib;
using nDSSH;
using System.Collections.Generic;
using System.Text.Json;

namespace CapsBallServer
{
    public static class ResponseCaller
    {
        public static void ResponseAdminAdded(Player player) =>
            TeamsHandler.Broadcast(new ResponsePackage(ResponseCommand.ADMIN_ADDED));

        public static void ResponseTeamData(string targetNick, Team team)
        {
            List<string> parameters = new List<string>();
            parameters.Add(team.TeamType.ToString());
            parameters.Add(team.Name);
            parameters.Add(team.GetCount().ToString());
            foreach (Player p in team.GetPlayers())
                parameters.Add(JsonSerializer.Serialize(p));

            ResponsePackage package = new ResponsePackage(ResponseCommand.GET_TEAM, parameters);
            TeamsHandler.Broadcast(package);
        }

        public static void ResponseGameStarted(string starterNick)
        {
            List<string> parameters = new List<string>(new string[] { starterNick });
            ResponsePackage package = new ResponsePackage(ResponseCommand.GAME_STARTED, parameters);
            TeamsHandler.Broadcast(package);
        }

        public static void ResponseJoinedTeam(Player player, TeamType teamType)
        {
            List<string> parameters = new List<string>(new string[] { JsonSerializer.Serialize(player), teamType.ToString() });
            ResponsePackage package = new ResponsePackage(ResponseCommand.JOINED_TEAM, parameters);
            TeamsHandler.Broadcast(package);
        }

        public static void ResponseSendFootballerData(string playerNick, Vector2 position, Vector2 velocity)
        {
            List<string> parameters = new List<string>(new string[] { playerNick, position.X.ToString(), position.Y.ToString(), velocity.X.ToString(), velocity.Y.ToString() });
            ResponsePackage package = new ResponsePackage(ResponseCommand.SEND_FOOTBALLER, parameters);
            TeamsHandler.Broadcast(package);
        }

        public static void ResponseSendGameState(string playerNick)
        {
            List<string> parameters = new List<string>(new string[] { JsonSerializer.Serialize(TeamsHandler.GameState) });
            ResponsePackage package = new ResponsePackage(ResponseCommand.SEND_GAME_STATE, parameters);
            Sender.SendFeedbackByAlias(package.GetRawData(), playerNick);
        }
    }
}