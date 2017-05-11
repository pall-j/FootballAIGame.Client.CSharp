using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.AIs.Fsm.Messaging;

namespace FootballAIGame.Client.AIs.Fsm.PlayerStates.GlobalStates
{
    /// <summary>
    /// Represents the field player's global state. Keeps <see cref="PlayerGlobalState"/> internally and calls
    /// its methods at the end of its own methods.
    /// </summary>
    class FieldPlayerGlobalState : PlayerState
    {
        /// <summary>
        /// Gets or sets the player' global state that is used at the end of the this state methods.
        /// </summary>
        /// <value>
        /// The player global state.
        /// </value>
        private PlayerGlobalState PlayerGlobalState { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldPlayerGlobalState"/> class.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="footballAI">The <see cref="FsmAI" /> instance to which this instance belongs.</param>
        public FieldPlayerGlobalState(Player player, FsmAI footballAI) : base(player, footballAI)
        {
            PlayerGlobalState = new PlayerGlobalState(player, footballAI);
        }

        /// <summary>
        /// Occurs every simulation step while the entity is in this state.
        /// </summary>
        public override void Run()
        {
            var team = AI.MyTeam;

            if (Player.CanKickBall(AI.Ball))
            {
                Player.StateMachine.ChangeState(new KickBall(Player, AI));
            }
            else if (team.NearestPlayerToBall == Player &&
                     team.PassReceiver == null)
            {
                Player.StateMachine.ChangeState(new PursueBall(Player, AI));
            }


            PlayerGlobalState.Run();   
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
            return PlayerGlobalState.ProcessMessage(message);
        }
    }
}
