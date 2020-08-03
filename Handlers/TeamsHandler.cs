using CapsBallShared;
using nDSSH;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CapsBallServer
{
    #region howno
    /*
    static class TeamsHandler
    {
        static TeamsCollector teamsCollector = new TeamsCollector();

        public static void Initialize()
        {
            initializeEvents();
        }

        static void initializeEvents()
        {
            JoinTeamRequestHandler.JoinedTeam += onJoinedToTeam;
            CreateTeamRequestHandler.CreatedTeam += onTeamCreated;
        }
        /*
        static void onChallenged(string callingUserAlias, string challengingTeamAlias, string challengedTeamAlias)
        {
            if (!isCaptainOfTeam(callingUserAlias, teamsCollector.Completed))
                return;

            Team[] teamsConsidered = teamsCollector.Completed.ToArray();

            Team challengingTeam = null;
            Team challengedTeam = null;

            foreach (Team team in teamsConsidered)
            {
                if (team.TeamAlias == challengingTeamAlias)
                    challengingTeam = team;
                else if (team.TeamAlias == challengedTeamAlias)
                    challengedTeam = team;
            }

            if (challengingTeam == null || challengedTeam == null)
                return;

            foreach (Player player in challengingTeam.Players)
                ResponseCaller.ResponseSendTeamInfo(challengedTeam.CaptainAlias, challengingTeam);

            ResponseCaller.ResponseChallange(challengedTeam.CaptainAlias, challengingTeamAlias);
        }
    
    static void reassignTeams(Team challengedTeam, Team challengingTeam, TeamsCollector collection)
        {
            collection.Completed.Remove(challengedTeam);
            collection.Completed.Remove(challengingTeam);

            collection.Playing.Add(challengedTeam);
            collection.Playing.Add(challengingTeam);
        }

        static Team[] getAppropiateTeams(TeamStates.State state, TeamsCollector collector)
        {
            switch (state)
            {
                case TeamStates.State.COMPLETED:
                    return collector.Completed.ToArray();
                case TeamStates.State.PLAYING:
                    return collector.Playing.ToArray();
                case TeamStates.State.UNCOMPLETED:
                    return collector.Uncompleted.ToArray();
                default:
                    return new Team[0];
            }
        }

        static void onJoinedToTeam(object sender, JoinedTeamEventArgs args)
        {
            Team[] teamsConsidered = teamsCollector.Uncompleted.ToArray();

            foreach (Team team in teamsConsidered)
            {
                if (team.Name != args.TeamName)
                    continue;

                Player player = new Player(DBReader.GetAccountByNick(args.JoinerNick).Result);
                team.AddPlayer(player);

                handleTeamCompleted(team);
                ResponseCaller.ResponseTeamData(team);
            }
        }

        static void handleTeamCompleted(Team team)
        {
            if (team.GetCount() == team.TargetCount)
            {
                teamsCollector.Completed.Add(team);
                teamsCollector.Uncompleted.Remove(team);
            }
        }


        static void onTeamCreated(object sender, CreatedTeamEventArgs args)
        {
            Team newTeam = new Team(args.TeamName, args.TargetCount);
            newTeam.AddPlayer(new Player(args.Creator));
            teamsCollector.Uncompleted.Add(newTeam);
        }

        static void removeUserFromTeamCollector(string alias, TeamsCollector collector)
        {
            removeUserFromTeamList(alias, collector.Completed);
            removeUserFromTeamList(alias, collector.Playing);
            removeUserFromTeamList(alias, collector.Uncompleted);
        }

        static void removeUserFromTeamList(string nick, List<Team> teams)
        {
            foreach (Team team in teams)
            {
                List<Player> players = team.GetPlayers();
                foreach (Player player in players)
                    if (player.Account.Nick == nick)
                    {
                        handleRemovalPlayerFromTeam(player, team, teams);
                        return;
                    }
            }
        }

        static void handleRemovalPlayerFromTeam(Player player, Team team, List<Team> teams)
        {
            bool isCaptain = team.GetCaptainNick() == player.Account.Nick;
            team.RemovePlayer(player.Account.Nick);
            if (isCaptain || team.GetCount() == 0)
                teams.Remove(team);

            ResponseCaller.ResponseTeamData(team);
        }
    }
    */ 
    #endregion

    public static class TeamsHandler
    {
        static Team blueTeam = new Team("B");
        static Team redTeam = new Team("R");
        //static List<Player> playersActive = new List<Player>();

        public static void Initialize()
        {
            JoinTeamRequestHandler.JoinedTeam += onJoinedTeam;
            StartGameRequestHandler.GameStarted += onGameStarted;
            ServerManager.ConnectionLost += onClientLeft;
        }

        static void onJoinedTeam(object sender, JoinedTeamEventArgs args)
        {
            Player joiner = new Player(DBReader.GetAccountByNick(args.JoinerNick).Result);
            //playersActive.Add(joiner);
            System.Console.WriteLine($"{joiner.Account.Nick} joins { args.TeamName}");

            if (args.TeamName == blueTeam.Name)
                blueTeam.AddPlayer(joiner);
            else
                redTeam.AddPlayer(joiner);

            if (!isAnyAdmin())
            {
                joiner.IsAdmin = true;
                ResponseCaller.ResponseAdminAdded(joiner);
                System.Console.WriteLine("kupadmin");
            }
        }

        static bool isAnyAdmin() => isAdminInTeam(blueTeam) || isAdminInTeam(redTeam);

        static bool isAdminInTeam(Team team) => team.Players.Any(p => p.IsAdmin);

        static void onGameStarted(object sender, StartGameEventArgs args)
        {
            if (!isAdminInTeams(args.StarterNick))
                return;

            ResponseCaller.ResponseGameStarted(args.StarterNick);
        }

        static bool isAdminInTeams(string nick) => isAdminInTeam(nick, blueTeam) || isAdminInTeam(nick, redTeam);
        static bool isAdminInTeam(string nick, Team team) => team.Players.Any(p => p.IsAdmin && p.Account.Nick == nick);

        static void onClientLeft(string nick)
        {
            System.Console.WriteLine($"{nick} lifinwsdfs");
            if (isAdminInTeams(nick));

            blueTeam.RemovePlayer(nick);
            redTeam.RemovePlayer(nick);
            IdResolver.RemoveUser(nick);
            //playersActive.Remove(playersActive.Where(item => item.Account.Nick == nick).FirstOrDefault());
        }

        public static void Broadcast(ResponsePackage package)
        {
            broadcastToTeam(blueTeam, package);
            System.Console.WriteLine(blueTeam.Players.Where(p => IdResolver.UserExists(p.Account.Nick)).ToList().Count);
            broadcastToTeam(redTeam, package);
            System.Console.WriteLine($"b {blueTeam.Players.Count} r{redTeam.Players.Count} {package.GetRawData()}");
        }

        static void broadcastToTeam(Team team, ResponsePackage package) =>
            team.Players.Where(p => IdResolver.UserExists(p.Account.Nick)).ToList().ForEach(r => Sender.SendFeedbackByAlias(package.GetRawData(), r.Account.Nick));
    }
}