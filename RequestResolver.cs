using CapsBallShared;
using System.Collections.Generic;
using System.Linq;

namespace CapsBallServer
{
    static class RequestResolver
    {
        static Dictionary<IRequestHandler, string> resolver = new Dictionary<IRequestHandler, string>();
        const string UNDEFINED_ID = "undefined";

        static RequestResolver()
        {
            resolver.Add(new ChallangeTeamRequestHandler(), CommandsTranslator.RequestToString(RequestCommand.CHALLANGE_TEAM));
            resolver.Add(new CreateTeamRequestHandler(), CommandsTranslator.RequestToString(RequestCommand.CREATE_TEAM));
            resolver.Add(new JoinGameRequestHandler(), CommandsTranslator.RequestToString(RequestCommand.JOIN_GAME));
            resolver.Add(new JoinTeamRequestHandler(), CommandsTranslator.RequestToString(RequestCommand.JOIN_TEAM));
        }

        public static string HandlerToString(IRequestHandler handler) =>
            resolver.ContainsKey(handler) ? resolver[handler] : UNDEFINED_ID;

        public static IRequestHandler StringToHandler(string command) =>
            resolver.Where(p => p.Value == command).FirstOrDefault().Key;
    }
}