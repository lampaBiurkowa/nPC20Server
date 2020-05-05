using nDSSH;
using System;

namespace CapsBallServer
{
    class CreatedTeamEventArgs
    {
        public Account Creator { get; private set; }
        public int TargetCount { get; private set; }
        public string TeamName { get; private set; }

        public CreatedTeamEventArgs(Account creator, string teamName, int targetCount)
        {
            Creator = creator;
            TargetCount = targetCount;
            TeamName = teamName;
        }
    }

    class CreateTeamRequestHandler : IRequestHandler
    {
        public static event EventHandler<CreatedTeamEventArgs> CreatedTeam;

        public int ParamsRequiredCount { get; } = 1;

        public void Handle(RequestPackage package)
        {
            Account creator = DBReader.GetAccountByNick(package.Alias).Result;
            string teamName = package.Parameters[0];
            int targetCount = int.Parse(package.Parameters[1]);

            CreatedTeam?.Invoke(this, new CreatedTeamEventArgs(creator, teamName, targetCount));
        }
    }
}
