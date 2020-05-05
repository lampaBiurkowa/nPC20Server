using CapsBallShared;
using nDSSH;
using System.Collections.Generic;

namespace CapsBallServer
{
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
        */
        /*
        static void onChallengeAnswered(string callingUserAlias, string challengedTeamAlias, string challengingTeamAlias, bool challangeAccepted)
        {
            if (!isCaptainOfTeam(callingUserAlias, teamsCollector.Completed))
                return;

            Team[] teamsConsidered = teamsCollector.Completed.ToArray();

            Team challengedTeam = null;
            Team challengingTeam = null;

            for (int i = 0; i < teamsConsidered.Length; i++)
            {
                if (teamsConsidered[i].TeamAlias == challengedTeamAlias)
                    challengedTeam = teamsConsidered[i];
                else if (teamsConsidered[i].TeamAlias == challengingTeamAlias)
                    challengingTeam = teamsConsidered[i];
            }

            if (challengedTeam == null || challengingTeam == null)
                return; // :D/

            ResponseCaller.ResponseChallengeAnswered(challangeAccepted, challengedTeam, challengingTeam);

            if (challangeAccepted)
                createMatchRoom(challengedTeam, challengingTeam);
        }
        */
        /*static void createMatchRoom(Team challengedTeam, Team challengingTeam)
        {
            reassignTeams(challengedTeam, challengingTeam, teamsCollector);

            MatchRoom room = new MatchRoom(challengedTeam, challengingTeam, "classic", 300, false); //joke only u know
            ServerData.MatchRoomsHandler.MatchRooms.Add(room);
        }*/

        static void reassignTeams(Team challengedTeam, Team challengingTeam, TeamsCollector collection)
        {
            collection.Completed.Remove(challengedTeam);
            collection.Completed.Remove(challengingTeam);

            collection.Playing.Add(challengedTeam);
            collection.Playing.Add(challengingTeam);
        }

        /*static void onGetPlayersJoined(string getterAlias, string teamAlias, ModeData.Mode mode)
        {
            TeamsCollector collectorConsidered = getAppropiateTeamsCollector(mode, getterAlias);
            Team[] teamsConsidered = collectorConsidered.Uncompleted.ToArray();

            foreach (Team team in teamsConsidered)
                if (team.TeamAlias == teamAlias)
                    sendAlreadyJoinedPlayers(getterAlias, team.Players);
        }*/

        /*
        static void sendAlreadyJoinedPlayers(string playerAlias, List<Player> players)
        {
            List<string> userAliaes = new List<string>();
            List<int> footballerTeamDBIds = new List<int>();

            foreach (Player player in players)
            {
                userAliaes.Add(player.User.Alias);
                footballerTeamDBIds.Add(player.Footballer.Data.Id);
            }

            ResponseCaller.ResponseSendAlreadyJoinedPlayers(playerAlias, userAliaes, footballerTeamDBIds);
        }
        */
        /*static void onGetTeamInfo(string getterAlias, string teamAlias, TeamStates.State state, ModeData.Mode mode)
        {
            TeamsCollector collectorConsidered = getAppropiateTeamsCollector(mode, getterAlias);
            Team[] teamsToSend = getAppropiateTeams(state, collectorConsidered);

            foreach (Team team in teamsToSend)
                if (team.TeamAlias == teamAlias)
                {
                    ResponseCaller.ResponseSendTeamInfo(getterAlias, team);
                    return;
                }
        }*/

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

        /*static void onReloadTeams(string senderAlias, TeamStates.State state, ModeData.Mode mode)
        {
            TeamsCollector teamsCollector = getAppropiateTeamsCollector(mode, senderAlias);
            Team[] teamsToSend = getAppropiateTeams(state, teamsCollector);

            foreach (Team team in teamsToSend)
                ResponseCaller.ResponseSendTeamInfo(senderAlias, team);
        }*/

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
}