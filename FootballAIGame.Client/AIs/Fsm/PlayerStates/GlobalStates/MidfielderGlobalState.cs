using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.AIs.Fsm.Messaging;

namespace FootballAIGame.Client.AIs.Fsm.PlayerStates.GlobalStates
{
    /// <summary>
    /// Represents the midfielder global state. Keeps <see cref="FieldPlayerGlobalState"/> internally and calls
    /// its methods at the end of its own methods.
    /// </summary>
    class MidfielderGlobalState : PlayerState
    {
        /// <summary>
        /// Gets or sets the field player's global state that is used at the end of the this state methods.
        /// </summary>
        /// <value>
        /// The field player global state.
        /// </value>
        private FieldPlayerGlobalState FieldPlayerGlobalState { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MidfielderGlobalState"/> class.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="footballAI">The <see cref="FsmAI" /> instance to which this instance belongs.</param>
        public MidfielderGlobalState(Player player, FsmAI footballAI) : base(player, footballAI)
        {
            FieldPlayerGlobalState = new FieldPlayerGlobalState(player, footballAI);
        }

        /// <summary>
        /// Occurs every simulation step while the entity is in this state.
        /// </summary>
        public override void Run()
        {
            FieldPlayerGlobalState.Run();
        }

        /// <summary>
        /// Processes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>
        ///   <c>true</c> if the specified message was handled; otherwise, <c>false</c>
        /// </returns>
        public override bool ProcessMessage(IMessage message)
        {
            return FieldPlayerGlobalState.ProcessMessage(message);
        }
    }
}
