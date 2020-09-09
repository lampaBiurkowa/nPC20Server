using CapsBallShared;
using GeoLib;
using Newtonsoft.Json;
using System;

namespace CapsBallServer
{
    public class BulletTriggeredRequestHandler : IRequestHandler
    {
        public static event EventHandler<BulletState> BulletTriggered;

        public int ParamsRequiredCount { get; } = 1;
        public void Handle(RequestPackage package)
        {
            BulletState bulletState = JsonConvert.DeserializeObject<BulletState>(package.Parameters[0]);

            BulletTriggered?.Invoke(this, bulletState);
            ResponseCaller.ResponseBulletTriggered(bulletState);
        }
    }
}
