using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPTestApp
{
    class World
    {
        private WorldBlock startingBlock;

        public World()
        {
            //New worldblock with a spread of 4
            startingBlock = new WorldBlock(4);
        }

        public WorldBlock StartingBlock
        {
            get { return startingBlock; }
            set { startingBlock = value; }
        }
    }
}
