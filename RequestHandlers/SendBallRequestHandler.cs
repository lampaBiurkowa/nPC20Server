using CapsBallShared;
using Newtonsoft.Json;
using System;

namespace CapsBallServer
{
    public class SendBallStateRequestHandler : IRequestHandler
    {
        public static event EventHandler<BallState> BallSent;

        public int ParamsRequiredCount { get; } = 1;
        public void Handle(RequestPackage package)
        {
            BallState ballState = JsonConvert.DeserializeObject<BallState>(package.Parameters[0]);

            BallSent?.Invoke(this, ballState);
            ResponseCaller.ResponseSendBallState(ballState);
        }
    }
}
