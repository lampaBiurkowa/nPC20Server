using CapsBallShared;
using System;

namespace CapsBallServer
{
    class JoinedTeamEventArgs
    {
        public string TeamName { get; private set; }

        public JoinedTeamEventArgs(string teamName)
        {
            TeamName = teamName;
        }
    }

    class JoinTeamRequestHandler : IRequestHandler
    {
        public static event EventHandler<JoinedTeamEventArgs> JoinedTeam;

        public int ParamsRequiredCount { get; } = 1;
        public void Handle(RequestPackage package)
        {
            
        }
    }
}
