using CapsBallShared;
using System.Collections.Generic;

namespace CapsBallServer
{
    class TeamsCollector
    {
        public List<Team> Completed { get; set; } = new List<Team>();
        public List<Team> Playing { get; set; } = new List<Team>();
        public List<Team> Uncompleted { get; set; } = new List<Team>();
    }
}