using CapsBallShared;
using nDSSH;
using System.Collections.Generic;

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

    static class TeamsHandler
    {
        static Team blueTeam = new Team("B", 2);
        static Team redTeam = new Team("R", 2);
        static Player admin = null;

        static TeamsHandler()
        {
            JoinTeamRequestHandler.JoinedTeam += onJoinedTeam;
        }

        static void onJoinedTeam(object sender, JoinedTeamEventArgs args)
        {
            Player joiner = new Player(DBReader.GetAccountByNick(args.JoinerNick).Result);
            if (args.TeamName == blueTeam.Name)
                blueTeam.AddPlayer(joiner);
            else
                redTeam.AddPlayer(joiner);

            if (admin == null)
                admin = joiner;
        }

        //TODO on player left
    }
}