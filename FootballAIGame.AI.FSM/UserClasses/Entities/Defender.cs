using FootballAIGame.AI.FSM.SimulationEntities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;
using FootballAIGame.AI.FSM.UserClasses.PlayerStates;
using FootballAIGame.AI.FSM.UserClasses.PlayerStates.GlobalStates;

namespace FootballAIGame.AI.FSM.UserClasses.Entities
{
    class Defender : FieldPlayer
    {
        public Defender(FootballPlayer player, Ai ai) : base(player, ai)
        {
            StateMachine = new FiniteStateMachine<Player>(this, new Default(this, Ai), new DefenderGlobalState(this, Ai));
        }

        public override void ProcessMessage(IMessage message)
        {
            StateMachine.ProcessMessage(this, message);
        }

    }
}
