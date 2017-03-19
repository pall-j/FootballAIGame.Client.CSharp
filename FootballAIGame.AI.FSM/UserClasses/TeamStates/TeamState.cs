using FootballAIGame.AI.FSM.UserClasses.Entities;

namespace FootballAIGame.AI.FSM.UserClasses.TeamStates
{
    abstract class TeamState : State<Team>
    {
        protected Team Team { get; set; }

        protected TeamState(Team team, FootballAI footballAI) : base(team, footballAI)
        {
            Team = team;
        }

        public abstract void SetHomeRegions();
    }
}
