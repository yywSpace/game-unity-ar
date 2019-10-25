namespace Script.Client.clients
{
    public static class ClientLab
    {
        private static GameClient gameClient;

        /// <summary>
        /// GameClient 单例
        /// </summary>
        /// <returns></returns>
        public static GameClient GetGameClient()
        {
            return gameClient ?? (gameClient = new GameClient());
        }

    }
}