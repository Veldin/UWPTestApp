namespace UWPTestApp
{
    class World
    {
        private WorldBlock startingBlock;

        public World()
        {
            startingBlock = new WorldBlock();
        }

        public WorldBlock StartingBlock
        {
            get { return startingBlock; }
            set { startingBlock = value; }
        }
    }
}
