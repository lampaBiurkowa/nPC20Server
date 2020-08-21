using CapsBallShared;

namespace CapsBallServer
{
    public static class CachedData
    {
        public static StadiumCoreData StadiumCoreData { get; set; }
        public static GameState GameState { get; set; } = new GameState();
    }
}
