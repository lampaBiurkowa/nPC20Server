using System.Collections.Generic;

namespace CapsBallServer
{
    static class RequestResolver
    {
        static Dictionary<IRequestHandler, string> resolver = new Dictionary<IRequestHandler, string>();
        const string UNDEFINED_ID = "undefined";

        static RequestResolver()
        {
            resolver.Add(new CreateTeamRequestHandler(), "addforce");
        }

        public static string HadnlerToString(IRequestHandler handler)
        {
            if (resolver.ContainsKey(handler))
                return resolver[handler];

            return UNDEFINED_ID;
        }

        public static IRequestHandler StringToHandler(string command)
        {
            foreach (KeyValuePair<IRequestHandler, string> pair in resolver)
                if (pair.Value == command)
                    return pair.Key;

            return null; // :D
        }
    }
}