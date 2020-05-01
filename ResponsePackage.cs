using nDSSH;
using System.Collections.Generic;

namespace CapsBallServer
{
    class ResponsePackage : CommandPackage
    {
        public ResponseResolver.Command Command { get; private set; }

        public ResponsePackage(ResponseResolver.Command command, List<string> parameters)
        {
            Command = command;
            Parameters = parameters;
        }

        public ResponsePackage(ResponseResolver.Command command)
        {
            Command = command;
        }

        public ResponsePackage(Package package)
        {
            string[] basicComponents = package.MessageContent.Split(COMMAND_SPLIT_CHAR);

            Command = ResponseResolver.StringToCommand(basicComponents[0]);

            string[] parameters = basicComponents[1].Split(PARAMETER_SPLIT_TEXT);
            foreach (string parameter in parameters)
                Parameters.Add(parameter);
        }

        public override string GetRawData()
        {
            string rawData = ResponseResolver.CommandToString(Command) + COMMAND_SPLIT_CHAR;

            for (int i = 0; i < Parameters.Count; i++)
            {
                if (Parameters[i].Contains(PARAMETER_SPLIT_TEXT))
                    return ERROR_TEXT; //params cannot contain split text cause it would harm the communication

                rawData += Parameters[i];
                if (i != Parameters.Count - 1)
                    rawData += PARAMETER_SPLIT_TEXT;
            }

            return rawData;
        }
    }
}