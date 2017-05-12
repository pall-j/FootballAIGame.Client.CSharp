using FootballAIGame.Client.AIs.Fsm.Messaging;
using FootballAIGame.Client.AIs.Fsm.PlayerStates;
using FootballAIGame.Client.AIs.Fsm.PlayerStates.GlobalStates;
using FootballAIGame.Client.SimulationEntities;

namespace FootballAIGame.Client.AIs.Fsm.Entities
{
    /// <summary>
    /// Represents the goalkeeper. Derives directly from <see cref="Player"/>.
    /// </summary>
    /// <seealso cref="FootballAIGame.Client.AIs.Fsm.Entities.Player" />
    class GoalKeeper : Player
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GoalKeeper"/> class.
        /// </summary>
        /// <param name="player">The football player.</param>
        /// <param name="footballAI">The <see cref="FsmAI" /> instance to which this player belongs.</param>
        public GoalKeeper(FootballPlayer player, FsmAI footballAI) : base(player, footballAI)
        {
            StateMachine = new FiniteStateMachine<Player>(this, new Default(this, footballAI), new GoalKeeperGlobalState(this, footballAI));
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
