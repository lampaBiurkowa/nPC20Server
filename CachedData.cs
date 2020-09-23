using CapsBallShared;
using System.Collections.Generic;

namespace CapsBallServer
{
    public static class CachedData
    {
        public static GameState GameState { get; set; } = new GameState();
        public static StadiumCoreData StadiumCoreData { get; set; }
    }
}
