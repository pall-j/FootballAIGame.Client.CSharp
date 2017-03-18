using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;
using FootballAIGame.AI.FSM.UserClasses.Messaging.Messages;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates.GlobalStates
{
    class PlayerGlobalState : PlayerState
    {
        public PlayerGlobalState(Player entity, Ai ai) : base(entity, ai)
        {
        }

        public override void Run()
        {
        }

        public override bool ProcessMessage(IMessage message)
        {
            if (message is ReturnToHomeMessage)
            {
                Player.StateMachine.ChangeState(new MoveToHomeRegion(Player, Ai));
                return true;
            }

            if (message is SupportControllingMessage)
            {
                if (!(Player.StateMachine.CurrentState is SupportControlling))
                    Player.StateMachine.ChangeState(new SupportControlling(Player, Ai));
                return true;
            }

            if (message is GoDefaultMessage)
            {
                Player.StateMachine.ChangeState(new Default(Player, Ai));
                return true;
            }

            if (message is PassToPlayerMessage)
            {
                var ball = Ai.Ball;
                var target = ((PassToPlayerMessage) message).Receiver;

                var time = ball.TimeToCoverDistance(Vector.DistanceBetween(target.Position, ball.Position),
                    Player.MaxKickSpeed);

                if (double.IsInfinity(time)) // pass not possible
                    return true;

                var predictedTargetPosition = target.PredictedPositionInTime(time);

                if (Player.CanKickBall(ball))
                {
                    Player.KickBall(ball, predictedTargetPosition);
                    MessageDispatcher.Instance.SendMessage(new ReceivePassMessage(predictedTargetPosition));
                    Player.StateMachine.ChangeState(new Default(Player, Ai));
                }

                return true;
            }

            if (message is ReceivePassMessage)
            {
                var msg = (ReceivePassMessage) message;
                Player.StateMachine.ChangeState(new ReceivePass(Player, Ai, msg.PassTarget));
                return true;
            }

            if (message is PursueBallMessage)
            {
                Player.StateMachine.ChangeState(new PursueBall(Player, Ai));
                return true;
            }

            return false;
        }

    }
}
