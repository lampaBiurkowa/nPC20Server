using CapsBallShared;
using Newtonsoft.Json;
using System;

namespace CapsBallServer
{
    public class StartGameEventArgs
    {
        public StadiumCoreData StadiumCoreData { get; private set; }
        public string StarterNick { get; private set; }

        public StartGameEventArgs(StadiumCoreData stadiumCoreData, string starterNick)
        {
            StadiumCoreData = stadiumCoreData;
            StarterNick = starterNick;
        }
    }

    public class StartGameRequestHandler : IRequestHandler
    {
        public static event EventHandler<StartGameEventArgs> GameStarted;

        public int ParamsRequiredCount { get; } = 1;
        public void Handle(RequestPackage package)
        {
            string serializedPlayer = package.Parameters[0];
            StadiumCoreData stadiumCoreData = JsonConvert.DeserializeObject<StadiumCoreData>(serializedPlayer);
            string starterNick = package.Alias;

            CachedData.GameState.GameStarted = true;
            CachedData.StadiumCoreData = stadiumCoreData;

            GameStarted?.Invoke(this, new StartGameEventArgs(stadiumCoreData, starterNick));
        }
    }
}
