using CapsBallShared;
using GeoLib;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CapsBallServer
{
    public static class BonusHandler
    {
        public static void RunBonusLoop()
        {
            Random random = new Random();
            int bonusesTotalCount = Enum.GetValues(typeof(BonusType)).Length;
            while (true) // B) /
            {
                for (int i = 0; i < CachedData.StadiumCoreData.BonusesCount; i++)
                {
                    BonusType bonus = (BonusType)random.Next(0, bonusesTotalCount);
                    Vector2 position = getValidPosition();
                    ResponseCaller.ResponseBonusAdded(bonus, position);
                }

                Task.Delay(CachedData.StadiumCoreData.BonusChangeSeconds * 1000);
            }
        }

        static Vector2 getValidPosition()
        {
            Random random = new Random();
            Vector2 result = new Vector2();
            bool positionCorrect = false;
            while (!positionCorrect)
            {
                int x = random.Next(0, CachedData.StadiumCoreData.Width);
                int y = random.Next(0, CachedData.StadiumCoreData.Height);
                result = new Vector2(x, y);
                positionCorrect = true;
                foreach (ShapeStruct item in CachedData.StadiumCoreData.Items)
                {
                    if (item.ContainsPoint(result))
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
