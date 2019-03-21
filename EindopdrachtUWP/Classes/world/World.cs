﻿namespace UWPTestApp
{
    class World
    {
        /* 
        * Used to create and store a worldBlock.
        */

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
