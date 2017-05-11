using FootballAIGame.Client.AIs.Fsm.Messaging;
using FootballAIGame.Client.AIs.Fsm.PlayerStates;
using FootballAIGame.Client.AIs.Fsm.PlayerStates.GlobalStates;
using FootballAIGame.Client.SimulationEntities;

namespace FootballAIGame.Client.AIs.Fsm.Entities
{
    /// <summary>
    /// Represents the midfielder. Derives from <see cref="FieldPlayer"/>.
    /// </summary>
    /// <seealso cref="FootballAIGame.Client.AIs.Fsm.Entities.FieldPlayer" />
    class Midfielder : FieldPlayer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Midfielder"/> class.
        /// </summary>
        /// <param name="player">The football player.</param>
        /// <param name="footballAI">The <see cref="FsmAI" /> instance to which this player belongs.</param>
        public Midfielder(FootballPlayer player, FsmAI footballAI) : base(player, footballAI)
        {
            StateMachine = new FiniteStateMachine<Player>(this, new Default(this, footballAI), new MidfielderGlobalState(this, footballAI));
        }

        /// <summary>
        /// Processes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public override void ProcessMessage(IMessage message)
        {
            StateMachine.ProcessMessage(this, message);
        }

    }
}
