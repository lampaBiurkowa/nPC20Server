using GeoLib;
using System;

namespace CapsBallServer
{
    public class ApplyImpulseEventArgs
    {
        public string Nick { get; private set; }
        public Vector2 Impulse { get; private set; }

        public ApplyImpulseEventArgs(Vector2 impulse, string nick)
        {
            Nick = nick;
            Impulse = impulse;
        }
    }

    public class ApplyImpulseRequestHandler : IRequestHandler
    {
        public static event EventHandler<ApplyImpulseEventArgs> ImpulseApplied;

        public int ParamsRequiredCount { get; } = 3;
        public void Handle(RequestPackage package)
        {
            string nick = package.Parameters[0];
            float x = float.Parse(package.Parameters[1]);
            float y = float.Parse(package.Parameters[2]);
            Vector2 impulse = new Vector2(x, y);
            ImpulseApplied?.Invoke(this, new ApplyImpulseEventArgs(impulse, nick));
            ResponseCaller.ResponseImpulseApplied(nick, impulse);
        }
    }
}
