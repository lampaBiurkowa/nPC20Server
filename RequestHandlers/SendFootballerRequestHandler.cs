using CapsBallShared;
using Newtonsoft.Json;
using System;

namespace CapsBallServer
{
    public class SendFootballerRequestHandler : IRequestHandler
    {
        public static event EventHandler<FootballerState> FootballerSent;

        public int ParamsRequiredCount { get; } = 1;
        public void Handle(RequestPackage package)
        {
            FootballerState footballerState = JsonConvert.DeserializeObject<FootballerState>(package.Parameters[0]);

            FootballerSent?.Invoke(this, footballerState);
            ResponseCaller.ResponseSendFootballerState(footballerState);
        }
    }
}
