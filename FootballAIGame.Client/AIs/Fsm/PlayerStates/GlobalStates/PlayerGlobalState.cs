using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.AIs.Fsm.Messaging;
using FootballAIGame.Client.AIs.Fsm.Messaging.Messages;
using FootballAIGame.Client.CustomDataTypes;

namespace FootballAIGame.Client.AIs.Fsm.PlayerStates.GlobalStates
{
    /// <summary>
    /// Represents the player global state.
    /// </summary>
    /// <seealso cref="FootballAIGame.Client.AIs.Fsm.PlayerStates.PlayerState" />
    class PlayerGlobalState : PlayerState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerGlobalState"/> class.
        /// </summary>
        /// <param name="entity">The entity to which this instance belongs.</param>
        /// <param name="footballAI">The <see cref="T:FootballAIGame.Client.AIs.Fsm.FsmAI" /> instance to which this player belongs.</param>
        public PlayerGlobalState(Player entity, FsmAI footballAI) : base(entity, footballAI)
        {
        }

        /// <summary>
        /// Occurs every simulation step while the entity is in this state.
        /// </summary>
        public override void Run()
        {
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
            if (message is ReturnToHomeMessage)
            {
                Player.StateMachine.ChangeState(new MoveToHomeRegion(Player, AI));
                return true;
            }

            if (message is SupportControllingMessage)
            {
                if (!(Player.StateMachine.CurrentState is SupportControlling))
                    Player.StateMachine.ChangeState(new SupportControlling(Player, AI));
                return true;
            }

            if (message is GoDefaultMessage)
            {
                Player.StateMachine.ChangeState(new Default(Player, AI));
                return true;
            }

            if (message is PassToPlayerMessage)
            {
                var ball = AI.Ball;
                var target = ((PassToPlayerMessage) message).Receiver;

                var time = ball.GetTimeToCoverDistance(Vector.GetDistanceBetween(target.Position, ball.Position),
                    Player.MaxKickSpeed);

                if (double.IsInfinity(time)) // pass not possible
                    return true;

                var predictedTargetPosition = target.PredictPositionInTime(time);

                if (Player.CanKickBall(ball))
                {
                    Player.KickBall(ball, predictedTargetPosition);
                    MessageDispatcher.Instance.SendMessage(new ReceivePassMessage(predictedTargetPosition));
                    Player.StateMachine.ChangeState(new Default(Player, AI));
                }

                return true;
            }

            if (message is ReceivePassMessage)
            {
                var msg = (ReceivePassMessage) message;
                Player.StateMachine.ChangeState(new ReceivePass(Player, AI, msg.PassTarget));
                return true;
            }

            if (message is PursueBallMessage)
            {
                Player.StateMachine.ChangeState(new PursueBall(Player, AI));
                return true;
            }

            return false;
        }
    }
}
