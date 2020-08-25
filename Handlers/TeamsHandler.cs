using CapsBallShared;
using nDSSH;
using System.Linq;

namespace CapsBallServer
{
    public static class TeamsHandler
    {
        static Team blueTeam = new Team(TeamType.BLUE, "B");
        static Team redTeam = new Team(TeamType.RED, "R");

        public static void Initialize()
        {
            JoinTeamRequestHandler.JoinedTeam += onJoinedTeam;
            StartGameRequestHandler.GameStarted += onGameStarted;
            ServerManager.ConnectionLost += onClientLeft;
        }

        static void onJoinedTeam(object sender, JoinedTeamEventArgs args)
        {
            Player joiner = new Player(DBReader.GetPublicAccount(DBReader.GetAccountByNick(args.JoinerNick).Result.Id).Result); // :D/ 

            Team teamConsidered = args.TeamType == blueTeam.TeamType ? blueTeam : redTeam;
            teamConsidered.AddPlayer(joiner);
            ResponseCaller.ResponseTeamData(joiner.PublicAccount.Nick, teamConsidered);
            ResponseCaller.ResponseJoinedTeam(joiner, teamConsidered.TeamType);
            ResponseCaller.ResponseSendGameState(joiner.PublicAccount.Nick);

            if (!isAnyAdmin())
            {
                joiner.IsAdmin = true;
                ResponseCaller.ResponseAdminAdded(joiner);
            }
        }

        static bool isAnyAdmin() => isAdminInTeam(blueTeam) || isAdminInTeam(redTeam);

        static bool isAdminInTeam(Team team) => team.Players.Any(p => p.IsAdmin);

        static void onGameStarted(object sender, StartGameEventArgs args)
        {
            if (!isAdminInTeams(args.StarterNick))
                return;

            ResponseCaller.ResponseGameStarted(args.StarterNick);
            BonusHandler.Initialize();
        }

        static bool isAdminInTeams(string nick) => isAdminInTeam(nick, blueTeam) || isAdminInTeam(nick, redTeam);
        static bool isAdminInTeam(string nick, Team team) => team.Players.Any(p => p.IsAdmin && p.PublicAccount.Nick == nick);

        static void onClientLeft(string nick)
        {
            if (!isAdminInTeams(nick));//

            blueTeam.RemovePlayer(nick);
            redTeam.RemovePlayer(nick);
            IdResolver.RemoveUser(nick);
        }

        public static void Broadcast(ResponsePackage package)
        {
            broadcastToTeam(blueTeam, package);
            broadcastToTeam(redTeam, package);
        }

        static void broadcastToTeam(Team team, ResponsePackage package) =>
            team.Players.Where(p => IdResolver.UserExists(p.PublicAccount.Nick)).ToList().ForEach(r => Sender.SendFeedbackByAlias(package.GetRawData(), r.PublicAccount.Nick));
    }
}