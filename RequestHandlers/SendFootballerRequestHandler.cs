

using GeoLib;
using System;
using System.Globalization;

namespace CapsBallServer
{
    public class SendFootballerEventArgs
    {
        public string PlayerNick { get; private set; }
        public Vector2 Position { get; private set; }
        public float Rotation { get; private set; }
        public Vector2 Velocity { get; private set; }

        public SendFootballerEventArgs(string playerNick, Vector2 position, float rotation, Vector2 velocity)
        {
            PlayerNick = playerNick;
            Position = position;
            Rotation = rotation;
            Velocity = velocity;
        }
    }

    public class SendFootballerRequestHandler : IRequestHandler
    {
        public static event EventHandler<SendFootballerEventArgs> FootballerSent;

        public int ParamsRequiredCount { get; } = 5;
        public void Handle(RequestPackage package)
        {
            string playerNick = package.Alias;
            Vector2 position = new Vector2(float.Parse(package.Parameters[0]), float.Parse(package.Parameters[1]));
            float rotation = float.Parse(package.Parameters[2]);
            Vector2 velocity = new Vector2(float.Parse(package.Parameters[3]), float.Parse(package.Parameters[4]));

            FootballerSent?.Invoke(this, new SendFootballerEventArgs(playerNick, position, rotation, velocity));
            ResponseCaller.ResponseSendFootballerData(playerNick, position, rotation, velocity);
        }
    }
}
