using CapsBallShared;
using Newtonsoft.Json;
using System;

namespace CapsBallServer
{
    public class SendBonusDataEventArgs
    {
        public string ReceiverNick { get; private set; }
        public BonusItemData BonusData { get; private set; }

        public SendBonusDataEventArgs(BonusItemData bonusData, string receiverNick)
        {
            ReceiverNick = receiverNick;
            BonusData = bonusData;
        }
    }

    public class SendBonusDataRequestHandler : IRequestHandler
    {
        public static event EventHandler<SendBonusDataEventArgs> BonusDataSent;

        public int ParamsRequiredCount { get; } = 2;
        public void Handle(RequestPackage package)
        {
            BonusItemData bonusData = JsonConvert.DeserializeObject<BonusItemData>(package.Parameters[0]);
            string receiverNick = package.Parameters[1];
            BonusDataSent?.Invoke(this, new SendBonusDataEventArgs(bonusData, receiverNick));
            ResponseCaller.ResponseBonusAdded(bonusData, receiverNick);
        }
    }
}
