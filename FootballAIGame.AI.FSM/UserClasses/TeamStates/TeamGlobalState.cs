using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.TeamStates
{
    class TeamGlobalState : State<Team>
    {
        public TeamGlobalState(Team team, Ai ai) : base(team, ai)
        {
        }

        public override void Run()
        {
        }

        public override bool ProcessMessage(IMessage message)
        {
            return false;
        }

    }
}
