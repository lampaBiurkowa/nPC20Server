using CapsBallShared;
using System.Collections.Generic;
using System.Linq;

namespace CapsBallServer
{
    static class RequestResolver
    {
        const string UNDEFINED_ID = "undefined";

        static Dictionary<IRequestHandler, string> resolver = new Dictionary<IRequestHandler, string>
        {
            { new ApplyImpulseRequestHandler(), CommandsTranslator.RequestToString(RequestCommand.APPLY_IMPULSE) },
            { new BulletTriggeredRequestHandler(), CommandsTranslator.RequestToString(RequestCommand.BULLET_TRIGGERED) },
            { new JoinGameRequestHandler(), CommandsTranslator.RequestToString(RequestCommand.JOIN_GAME) },
            { new JoinTeamRequestHandler(), CommandsTranslator.RequestToString(RequestCommand.JOIN_TEAM) },
            { new StartGameRequestHandler(), CommandsTranslator.RequestToString(RequestCommand.START_GAME) },
            { new SendBallStateRequestHandler(), CommandsTranslator.RequestToString(RequestCommand.SEND_BALL_STATE) },
            { new SendGameStateRequestHandler(), CommandsTranslator.RequestToString(RequestCommand.SEND_GAME_STATE) },
            { new SendFootballerStateRequestHandler(), CommandsTranslator.RequestToString(RequestCommand.SEND_FOOTBALLER_STATE) }
        };

        public static string HandlerToString(IRequestHandler handler) =>
            resolver.ContainsKey(handler) ? resolver[handler] : UNDEFINED_ID;

        public static IRequestHandler StringToHandler(string command) =>
            resolver.Where(p => p.Value == command).FirstOrDefault().Key;
    }
}