using System.Collections.Generic;

namespace CapsBallServer
{
    public static class ResponseResolver
    {
        public enum Command
        {
            CREATE_TEAM,
            UNDEFINED
        }

        static Dictionary<Command, string> resolver = new Dictionary<Command, string>();
        const string UNDEFINED_ID = "undefined";

        static ResponseResolver()
        {
            resolver.Add(Command.CREATE_TEAM, "createTeam");
            resolver.Add(Command.UNDEFINED, UNDEFINED_ID);
        }

        public static string CommandToString(Command command)
        {
            if (resolver.ContainsKey(command))
                return resolver[command];

            return UNDEFINED_ID;
        }

        public static Command StringToCommand(string command)
        {
            foreach (KeyValuePair<Command, string> pair in resolver)
                if (pair.Value == command)
                    return pair.Key;

            return Command.UNDEFINED;
        }
    }
}