namespace CapsBallServer
{
    static class TeamStates
    {
        public enum State { COMPLETED, PLAYING, UNCOMPLETED, UNDEFINED }

        public const string COMPLETED_TEAM_ID = "completed";
        public const string PLAYING_TEAM_ID = "playing";
        public const string UNCOMPLETED_TEAM_ID = "uncompleted";
        public const string UNDEFINED_TEAM_ID = "undefined";

        static public string StateToString(State state)
        {
            switch (state)
            {
                case State.COMPLETED:
                    return COMPLETED_TEAM_ID;
                case State.PLAYING:
                    return PLAYING_TEAM_ID;
                case State.UNCOMPLETED:
                    return UNCOMPLETED_TEAM_ID;
                default:
                    return UNDEFINED_TEAM_ID;
            }
        }

        static public State StringToState(string stateId)
        {
            switch (stateId)
            {
                case COMPLETED_TEAM_ID:
                    return State.COMPLETED;
                case PLAYING_TEAM_ID:
                    return State.PLAYING;
                case UNCOMPLETED_TEAM_ID:
                    return State.UNCOMPLETED;
                default:
                    return State.UNDEFINED;
            }
        }
    }
}