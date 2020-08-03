using CapsBallShared;
using System;

namespace CapsBallServer
{
    public class JoinedTeamEventArgs
    {
        public string JoinerNick { get; private set; }
        public TeamType TeamType { get; private set; }

        public JoinedTeamEventArgs(TeamType teamType, string joinerNick)
        {
            JoinerNick = joinerNick;
            TeamType = teamType;
        }
    }

    public class JoinTeamRequestHandler : IRequestHandler
    {
        public static event EventHandler<JoinedTeamEventArgs> JoinedTeam;

        public int ParamsRequiredCount { get; } = 1;
        public void Handle(RequestPackage package)
        {
            string joinerNick = package.Alias;
            TeamType teamType = (TeamType)Enum.Parse(typeof(TeamType), package.Parameters[0]);

            JoinedTeam?.Invoke(this, new JoinedTeamEventArgs(teamType, joinerNick));
        }
    }
}
