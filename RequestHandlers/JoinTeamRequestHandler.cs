using System;

namespace CapsBallServer
{
    public class JoinedTeamEventArgs
    {
        public string JoinerNick { get; private set; }
        public string TeamName { get; private set; }

        public JoinedTeamEventArgs(string teamName, string joinerNick)
        {
            JoinerNick = joinerNick;
            TeamName = teamName;
        }
    }

    public class JoinTeamRequestHandler : IRequestHandler
    {
        public static event EventHandler<JoinedTeamEventArgs> JoinedTeam;

        public int ParamsRequiredCount { get; } = 1;
        public void Handle(RequestPackage package)
        {
            string joinerNick = package.Alias;
            string teamName = package.Parameters[0];

            JoinedTeam?.Invoke(this, new JoinedTeamEventArgs(teamName, joinerNick));
        }
    }
}
