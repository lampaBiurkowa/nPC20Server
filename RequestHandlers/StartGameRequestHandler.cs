using System;

namespace CapsBallServer
{
    public class StartGameEventArgs
    {
        public string StarterNick { get; private set; }

        public StartGameEventArgs(string starterNick) => StarterNick = starterNick;
    }

    public class StartGameRequestHandler : IRequestHandler
    {
        public static event EventHandler<StartGameEventArgs> GameStarted;

        public int ParamsRequiredCount { get; } = 0;
        public void Handle(RequestPackage package)
        {
            string starterNick = package.Alias;
            GameStarted?.Invoke(this, new StartGameEventArgs(starterNick));
        }
    }
}
