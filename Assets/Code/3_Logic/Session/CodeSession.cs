namespace SessionEvent
{
    public readonly struct CodeSession
    {
        public readonly int Pos;
        public readonly int Key;

        public CodeSession(int pos, int key)
        {
            Pos = pos;
            Key = key;
        }
    }
    public readonly struct MenuCodeSession
    {
        public readonly int[] Key;

        public MenuCodeSession(int[] key)
        {
            Key = key;
        }
    }
    public readonly struct ResetCode { }
    public readonly struct RollCode { }

    public readonly struct PlayerCount
    {
        public readonly int PlayerConut;

        public PlayerCount(int playerConut)
        {
            PlayerConut = playerConut;
        }
    }
}