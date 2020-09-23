using CapsBallShared;
using Newtonsoft.Json;
using System;

namespace CapsBallServer
{
    public class SendGameStateRequestHandler : IRequestHandler
    {
        public static event EventHandler<GameState> GameStateSent;

        public int ParamsRequiredCount { get; } = 1;
        public void Handle(RequestPackage package)
        {
            GameState gameState = JsonConvert.DeserializeObject<GameState>(package.Parameters[0]);

            CachedData.GameState = gameState;
            GameStateSent?.Invoke(this, gameState);
            ResponseCaller.ResponseSendGameState();
        }
    }
}
