namespace FootballAIGame.AI.FSM
{
    /// <summary>
    /// Represents the command received from the game server.
    /// </summary>
    class Command
    {
        /// <summary>
        /// Gets or sets the command type.
        /// </summary>
        public CommandType Type { get; set; }

        /// <summary>
        /// Gets or sets the command data.
        /// </summary>
        public byte[] Data { get; set; }
    }
}
