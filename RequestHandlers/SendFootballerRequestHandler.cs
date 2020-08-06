

using GeoLib;
using System;
using System.Globalization;

namespace CapsBallServer
{
    public class SendFootballerEventArgs
    {
        public string PlayerNick { get; private set; }
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }

        public SendFootballerEventArgs(string playerNick, Vector2 position, Vector2 velocity)
        {
            PlayerNick = playerNick;
            Position = position;
            Velocity = velocity;
        }
    }

    public class SendFootballerRequestHandler : IRequestHandler
    {
        public static event EventHandler<SendFootballerEventArgs> FootballerSent;

        public int ParamsRequiredCount { get; } = 4;
        public void Handle(RequestPackage package)
        {
            string playerNick = package.Alias;
            Vector2 position = new Vector2(float.Parse(package.Parameters[0]), float.Parse(package.Parameters[1]));
            Vector2 velocity = new Vector2(float.Parse(package.Parameters[2]), float.Parse(package.Parameters[3]));

            FootballerSent?.Invoke(this, new SendFootballerEventArgs(playerNick, position, velocity));
            ResponseCaller.RequestSendFootballerData(playerNick, position, velocity);
        }
    }
}
