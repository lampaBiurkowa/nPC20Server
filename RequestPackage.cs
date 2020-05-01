using nDSSH;
using System.Collections.Generic;

namespace CapsBallServer
{
    class RequestPackage : CommandPackage
    {
        public string Alias { get; protected set; }
        IRequestHandler handler;

        public RequestPackage(string alias, string id, IRequestHandler handler, List<string> parameters)
        {
            Alias = alias;
            this.handler = handler;
            Id = id;
            Parameters = parameters;
        }

        public RequestPackage(string alias, string id, IRequestHandler handler)
        {
            Alias = alias;
            this.handler = handler;
            Id = id;
        }

        public RequestPackage(Package package)
        {
            string[] basicComponents = package.MessageContent.Split(COMMAND_SPLIT_CHAR);

            Alias = package.ClientData.Alias;
            handler = RequestResolver.StringToHandler(basicComponents[0]);
            Id = package.ClientData.Id;

            string[] parameters = basicComponents[1].Split(PARAMETER_SPLIT_TEXT);
            foreach (string parameter in parameters)
                Parameters.Add(parameter);
        }

        public override string GetRawData()
        {
            string rawData = Alias + COMMAND_SPLIT_CHAR + RequestResolver.HadnlerToString(handler) + COMMAND_SPLIT_CHAR;

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

        public bool TryHandle()
        {
            if (Parameters.Count < handler.ParamsRequiredCount)
                return false;

            handler.Handle(this);
            return true;
        }
    }
}