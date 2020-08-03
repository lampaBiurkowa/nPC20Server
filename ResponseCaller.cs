using CapsBallShared;
using nDSSH;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

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
            List<Player> players = team.GetPlayers();
            XmlSerializer serializer = new XmlSerializer(typeof(Player));
            foreach (Player p in players)
            {
                StringWriter stringWriter = new StringWriter();
                serializer.Serialize(stringWriter, p);
                parameters.Add(stringWriter.ToString());
            }

            ResponsePackage package = new ResponsePackage(ResponseCommand.GET_TEAM, parameters);
        }

        public static void ResponseGameStarted(string starterNick)
        {
            List<string> parameters = new List<string>(new string[] { starterNick });
            ResponsePackage package = new ResponsePackage(ResponseCommand.GAME_STARTED, parameters);
            TeamsHandler.Broadcast(package);
        }

        public static void ResponseJoinedTeam(string joinerNick, TeamType teamType)
        {
            List<string> parameters = new List<string>(new string[] { joinerNick, teamType.ToString() });
            ResponsePackage package = new ResponsePackage(ResponseCommand.JOINED_TEAM, parameters);
            TeamsHandler.Broadcast(package);
        }
    }
}