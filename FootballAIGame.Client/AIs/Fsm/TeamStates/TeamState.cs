using FootballAIGame.Client.AIs.Fsm.Entities;

namespace FootballAIGame.Client.AIs.Fsm.TeamStates
{
    abstract class TeamState : State<Team>
    {
        protected Team Team { get; set; }

        protected TeamState(Team team, FsmAI footballAI) : base(team, footballAI)
        {
            Team = team;
        }

        public abstract void SetHomeRegions();
    }
}
