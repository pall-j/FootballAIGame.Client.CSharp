using FootballAIGame.AI.FSM.UserClasses.Entities;

namespace FootballAIGame.AI.FSM.UserClasses.TeamStates
{
    abstract class TeamState : State<Team>
    {
        protected Team Team { get; set; }

        protected TeamState(Team team, Ai ai) : base(team, ai)
        {
            Team = team;
        }

        public abstract void SetHomeRegions();
    }
}
