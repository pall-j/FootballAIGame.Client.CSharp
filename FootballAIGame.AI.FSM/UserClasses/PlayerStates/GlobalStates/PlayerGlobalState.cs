using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;
using FootballAIGame.AI.FSM.UserClasses.Messaging.Messages;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates.GlobalStates
{
    class PlayerGlobalState : PlayerState
    {
        public PlayerGlobalState(Player entity, FootballAI footballAI) : base(entity, footballAI)
        {
        }

        public override void Run()
        {
        }

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

                var time = ball.TimeToCoverDistance(Vector.DistanceBetween(target.Position, ball.Position),
                    Player.MaxKickSpeed);

                if (double.IsInfinity(time)) // pass not possible
                    return true;

                var predictedTargetPosition = target.PredictedPositionInTime(time);

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
