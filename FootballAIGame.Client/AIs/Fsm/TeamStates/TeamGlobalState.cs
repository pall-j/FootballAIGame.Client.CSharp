using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.AIs.Fsm.Messaging;

namespace FootballAIGame.Client.AIs.Fsm.TeamStates
{
    class TeamGlobalState : State<Team>
    {
        public TeamGlobalState(Team team, FsmAI footballAI) : base(team, footballAI)
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
