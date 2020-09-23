using CapsBallShared;
using nDSSH;
using System;
using System.Collections.Generic;

namespace CapsBallServer
{
    public class ResponsePackage : CommandPackage
    {
        public ResponseCommand Command { get; private set; }

        public ResponsePackage(ResponseCommand command, List<string> parameters)
        {
            Command = command;
            Parameters = parameters;
        }

        public ResponsePackage(ResponseCommand command)
        {
            Command = command;
        }

        public ResponsePackage(Package package)
        {
            string[] basicComponents = package.MessageContent.Split(COMMAND_SPLIT_CHAR);

            Command = CommandsTranslator.StringToResponse(basicComponents[0]);
            Parameters = new List<string>(basicComponents[1].Split(new string[] { PARAMETER_SPLIT_TEXT }, StringSplitOptions.None));
        }

        public override string GetRawData()
        {
            string rawData = CommandsTranslator.ResponseToString(Command) + COMMAND_SPLIT_CHAR;

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