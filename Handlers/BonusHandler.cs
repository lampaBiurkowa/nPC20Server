using CapsBallShared;
using GeoLib;
using System;
using System.Diagnostics;

namespace CapsBallServer
{
    public static class BonusHandler
    {
        public static bool Started { get; set; } = false;
        public static int BonusesTotalCount { get; set; }

        static Random random = new Random();
        static Stopwatch timer = new Stopwatch();

        public static void Initialize()
        {
            BonusesTotalCount = Enum.GetValues(typeof(BonusType)).Length;
            Started = true;
            timer.Start();
        }

        public static void Update()
        {
            if (!Started || timer.Elapsed.Seconds < CachedData.StadiumCoreData.BonusChangeSeconds)
                return;

            for (int i = 0; i < CachedData.StadiumCoreData.BonusesCount; i++)
            {
                BonusType bonus = (BonusType)random.Next(0, BonusesTotalCount);
                Vector2 position = getValidPosition();
                ResponseCaller.ResponseBonusAdded(bonus, position);
            }
            timer.Restart();
        }

        static Vector2 getValidPosition()
        {
            Vector2 result = new Vector2();
            bool positionCorrect = false;
            while (!positionCorrect)
            {
                int x = random.Next(0, CachedData.StadiumCoreData.Width);
                int y = random.Next(0, CachedData.StadiumCoreData.Height);
                result = new Vector2(x, y);
                positionCorrect = true;
                foreach (ShapeStruct item in CachedData.StadiumCoreData.GetItems())
                {
                    if (item.ContainsCirc(new CircStruct(result, SharedConstants.BONUS_RADIUS)))
                    {
                        positionCorrect = false;
                        break;
                    }
                }
            }

            return result;
        }
    }
}
