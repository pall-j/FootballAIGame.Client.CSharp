using FootballAIGame.AI.FSM.SimulationEntities;

namespace FootballAIGame.AI.FSM.UserClasses.Entities
{
    abstract class FieldPlayer : Player
    {
        protected FieldPlayer(FootballPlayer player, FootballAI footballAI) : 
            base(player, footballAI)
        {
        }
    }
}
