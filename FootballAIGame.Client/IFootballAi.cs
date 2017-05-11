using FootballAIGame.Client.SimulationEntities;

namespace FootballAIGame.Client
{
    /// <summary>
    /// Represents the Football AI. Defines methods that are called during match simulations.
    /// </summary>
    interface IFootballAI
    {
        /// <summary>
        /// Called every time the new match simulation with the AI starts.<para />
        /// Called before <see cref="GetParameters"/>.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Gets the <see cref="AIAction"/> for the specified <see cref="GameState"/>.
        /// </summary>
        /// <param name="gameState">The state of the game.</param>
        /// <returns>The <see cref="AIAction" /> for the specified <see cref="GameState" />.</returns>
        AIAction GetAction(GameState gameState);

        /// <summary>
        /// Gets the players' parameters.
        /// </summary>
        /// <returns>The array of football players with their parameters set.</returns>
        FootballPlayer[] GetParameters();
    }
}
