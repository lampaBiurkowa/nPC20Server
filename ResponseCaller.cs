using CapsBallShared;
using GeoLib;
using nDSSH;
using System.Collections.Generic;
using System.Text.Json;

namespace CapsBallServer
{
    public static class ResponseCaller
    {
        public static void ResponseAdminAdded(Player player) =>
            TeamsHandler.Broadcast(new ResponsePackage(ResponseCommand.ADMIN_ADDED));

        public static void ResponseTeamData(string targetNick, Team team)
        {
            List<string> parameters = new List<string>();
            parameters.Add(team.TeamType.ToString());
            parameters.Add(team.Name);
            parameters.Add(team.GetCount().ToString());
            foreach (Player p in team.GetPlayers())
                parameters.Add(JsonSerializer.Serialize(p));

            ResponsePackage package = new ResponsePackage(ResponseCommand.GET_TEAM, parameters);
            TeamsHandler.Broadcast(package);
        }

        public static void ResponseBonusAdded(BonusType bonusType, Vector2 position)
        {
            List<string> parameters = new List<string>(new string[] { bonusType.ToString(), position.X.ToString(), position.Y.ToString() });
            ResponsePackage package = new ResponsePackage(ResponseCommand.BONUS_ADDED, parameters);
            TeamsHandler.Broadcast(package);
        }

        public static void ResponseBulletTriggered(BulletState bulletState)
        {
            List<string> parameters = new List<string>(new string[] { JsonSerializer.Serialize(bulletState) });
            ResponsePackage package = new ResponsePackage(ResponseCommand.BULLET_TRIGGERED, parameters);
            TeamsHandler.Broadcast(package);
        }

        public static void ResponseGameStarted(string starterNick)
        {
            List<string> parameters = new List<string>(new string[] { starterNick });
            ResponsePackage package = new ResponsePackage(ResponseCommand.GAME_STARTED, parameters);
            TeamsHandler.Broadcast(package);
        }

        public static void ResponseJoinedTeam(Player player, TeamType teamType)
        {
            List<string> parameters = new List<string>(new string[] { JsonSerializer.Serialize(player), teamType.ToString() });
            ResponsePackage package = new ResponsePackage(ResponseCommand.JOINED_TEAM, parameters);
            TeamsHandler.Broadcast(package);
        }

        public static void ResponseSendBallState(MovementState ballState)
        {
            List<string> parameters = new List<string>(new string[] { JsonSerializer.Serialize(ballState) });
            ResponsePackage package = new ResponsePackage(ResponseCommand.SEND_BALL_STATE, parameters);
            TeamsHandler.Broadcast(package);
        }

        public static void ResponseSendFootballerState(FootballerState footballerState)
        {
            List<string> parameters = new List<string>(new string[] { JsonSerializer.Serialize(footballerState) });
            ResponsePackage package = new ResponsePackage(ResponseCommand.SEND_FOOTBALLER_STATE, parameters);
            TeamsHandler.Broadcast(package);
        }

        public static void ResponseSendGameState(string playerNick)
        {
            List<string> parameters = new List<string>(new string[] { JsonSerializer.Serialize(CachedData.GameState) });
            ResponsePackage package = new ResponsePackage(ResponseCommand.SEND_GAME_STATE, parameters);
            Sender.SendFeedbackByAlias(package.GetRawData(), playerNick);
        }

        public static void ResponseImpulseApplied(string nick, Vector2 impulse)
        {
            List<string> parameters = new List<string>(new string[] { nick, impulse.X.ToString(), impulse.Y.ToString() });
            ResponsePackage package = new ResponsePackage(ResponseCommand.IMPULSE_APPLIED, parameters);
            TeamsHandler.Broadcast(package);
        }
    }
}