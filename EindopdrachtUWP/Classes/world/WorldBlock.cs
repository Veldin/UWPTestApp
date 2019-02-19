using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPTestApp
{
    class WorldBlock
    {
        private Scene scene;

        //Other blocks adjacent to this block
        private WorldBlock up;
        private WorldBlock down;
        private WorldBlock left;
        private WorldBlock right;

        //Position of this block
            //Position of the whole block, not in units!
        private int fromLeft;
        private int fromTop;

        //Size of blocks in units
        public const int width = 800;
        public const int height = 600;

        public WorldBlock()
        {
            Scene = SceneFactory.GetScene(width, height);
        }



        //Call used to create a worldblock with no adjacent rooms on the 0,0 position
        public WorldBlock(int spread) : this(spread, null, null, null, null, 0, 0)
        {

        }

        //The amount of spread dictates how many block spawn around this block
        public WorldBlock(int spread, WorldBlock up, WorldBlock down, WorldBlock left, WorldBlock right, int fromLeft, int fromTop)
        {
            //Set the private from left and from top
            this.fromLeft = fromLeft;
            this.fromTop = fromTop;

            Scene = SceneFactory.GetScene(width, height);

            //Spread if the spread is higher then 0
            spread--;
            if(spread > 0)
            {
                if (Up is null)
                {
                    Up = new WorldBlock(spread, null, this, null, null, Fromleft, FromTop - 1);
                }

                if (Down is null)
                {
                    Down = new WorldBlock(spread, this, null, null, null, Fromleft, FromTop + 1);
                }

                if (Left is null)
                {
                    Left = new WorldBlock(spread, null, null, null, this, Fromleft - 1, FromTop);
                }

                if (Right is null)
                {
                    Right = new WorldBlock(spread, null, null, this, null, Fromleft + 1, FromTop);
                }
            }
        }

        //Make this block with a specivic scene
        public WorldBlock(Scene scene)
        {
            Scene = scene;
        }

        public List<GameObject> LoadScene()
        {
            List<GameObject> gameObjects = new List<GameObject>();
            gameObjects = Scene.LoadScene();

            //Change location of the objects to reflect location of the worldblock
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.FromLeft = gameObject.FromLeft + (fromLeft * width);
                gameObject.FromTop = gameObject.FromTop + (fromTop * height);
            }

            return gameObjects;
        }

        public Scene Scene
        {
            get { return scene; }
            set { scene = value; }
        }

        public WorldBlock Up
        {
            get { return up; }
            set { up = value; }
        }

        public WorldBlock Down
        {
            get { return down; }
            set { down = value; }
        }

        public WorldBlock Left
        {
            get { return left; }
            set { left = value; }
        }

        public WorldBlock Right
        {
            get { return right; }
            set { right = value; }
        }

        public int Fromleft
        {
            get { return fromLeft; }
        }

        public int FromTop
        {
            get { return fromTop; }
        }
    }
}
