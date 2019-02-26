using System.Collections.Generic;
using System.Diagnostics;

namespace UWPTestApp
{
    /*
     * WorldBlock is responcable for a small part of the world that is able to load independelty.
     * This way the whole map does not have to load at once, but parts can be loading on their own.
     */
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

        //Call used to create a worldblock with no adjacent rooms on the 0,0 position
        public WorldBlock() : this(null, null, null, null, 0, 0)
        {

        }

        /*
         * up, down left and right given set the rooms that are up down left or right.
         * This way the traversion does not have to be done again but is stored in the worldblock itself.
         * 
         */
        public WorldBlock(WorldBlock up, WorldBlock down, WorldBlock left, WorldBlock right, int fromLeft, int fromTop)
        {
            //Set the private from left and from top
            this.fromLeft = fromLeft;
            this.fromTop = fromTop;

            Scene = SceneFactory.GetScene(width, height);
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


        /* 
         * Traverstion between objects is only done horizontaly on the zero-th line.
         * This is done to keep the intergatry of a 2d space consistant without overlapping rooms.
         * 
         *  Example;
         *  [] = worldblock.
         *  lines = Posibility to traverse.
         *  To traverse from a to b a.Up.Right.Down is used in code.
         *  
         *  -2  [ ] [ ] [ ] [ ] [ ]
         *       |   |   |   |   |  
         *  -1  [ ] [ ] [ ] [ ] [ ]
         *       |   |   |   |   |  
         *  0   [ ]-[ ]-[ ]-[ ]-[ ]
         *       |   |   |   |   |  
         *  1   [a] [b] [ ] [ ] [ ]
         *       |   |   |   |   |  
         *  2   [ ] [ ] [ ] [ ] [ ]
         */


        public WorldBlock Up
        {
            get {
                if (up is null)
                {
                    up = new WorldBlock(null, this, null, null, Fromleft, FromTop - 1);
                }
                return up;
            }
            set { up = value; }
        }

        public WorldBlock Down
        {
            get
            {
                if (down is null)
                {
                    down = new WorldBlock(this, null, null, null, Fromleft, FromTop + 1);
                }
                return down;
            }
            set { down = value; }
        }

        public WorldBlock Left
        {
            get {
                if (fromTop == 0)
                {
                    if (left is null)
                    {
                        left = new WorldBlock(null, null, null, this, Fromleft - 1, FromTop);
                    }
                    return left;
                }

                WorldBlock needle = this;

                if (fromTop > 0)
                {
                    int fromTopNeedle = fromTop;
                    while (fromTopNeedle > 0)
                    {
                        needle = needle.Down;
                        fromTopNeedle--;
                    }
                    needle = needle.Left;
                    fromTopNeedle = fromTop;
                    while (fromTopNeedle > 0)
                    {
                        needle = needle.Up;
                        fromTopNeedle++;
                    }
                    return needle;
                }
                else if(fromTop < 0){
                    int fromTopNeedle = fromTop;
                    while (fromTopNeedle < 0)
                    {
                        needle = needle.Up;
                        fromTopNeedle--;
                    }
                    needle = needle.Left;
                    fromTopNeedle = fromTop;
                    while (fromTopNeedle < 0)
                    {
                        needle = needle.Down;
                        fromTopNeedle++;
                    }
                    return needle;
                }

                return left;
            }
            set
            {
                if (fromTop == 0)
                {
                    Left = value;
                }

                WorldBlock needle = this;

                if (fromTop > 0)
                {
                    int fromTopNeedle = fromTop;
                    while (fromTopNeedle > 0)
                    {
                        needle = needle.Down;
                        fromTopNeedle++;
                    }
                    needle = needle.Left;
                    fromTopNeedle = fromTop;
                    while (fromTopNeedle > 0)
                    {
                        needle = needle.Up;
                        fromTopNeedle--;
                    }
                    needle = value;
                }
                else if (fromTop < 0)
                {
                    int fromTopNeedle = fromTop;
                    while (fromTopNeedle < 0)
                    {
                        needle = needle.Up;
                        fromTopNeedle--;
                    }
                    needle = needle.Left;
                    fromTopNeedle = fromTop;
                    while (fromTopNeedle < 0)
                    {
                        needle = needle.Down;
                        fromTopNeedle++;
                    }
                    needle = value;
                }
            }
        }

        public WorldBlock Right
        {
            get
            {
                if (fromTop == 0)
                {
                    if (right is null)
                    {
                        right = new WorldBlock(null, null, this, null, Fromleft + 1, FromTop);
                    }
                    return right;
                }

                WorldBlock needle = this;

                if (fromTop > 0)
                {
                    int fromTopNeedle = fromTop;
                    while (fromTopNeedle > 0)
                    {
                        needle = needle.Down;
                    }
                    needle = needle.Right;
                    fromTopNeedle = fromTop;
                    while (fromTopNeedle > 0)
                    {
                        needle = needle.Up;
                    }
                    return needle;
                }
                else if (fromTop < 0)
                {
                    int fromTopNeedle = fromTop;
                    while (fromTopNeedle < 0)
                    {
                        needle = needle.Up;
                    }
                    needle = needle.Right;
                    fromTopNeedle = fromTop;
                    while (fromTopNeedle < 0)
                    {
                        needle = needle.Down;
                    }
                    return needle;
                }

                return right;
            }
            set {
                if (fromTop == 0)
                {
                    Right = value;
                }

                WorldBlock needle = this;

                if (fromTop > 0)
                {
                    int fromTopNeedle = fromTop;
                    while (fromTopNeedle > 0)
                    {
                        needle = needle.Down;
                    }
                    needle = needle.Right;
                    fromTopNeedle = fromTop;
                    while (fromTopNeedle > 0)
                    {
                        needle = needle.Up;
                    }
                    needle = value;
                }
                else if (fromTop < 0)
                {
                    int fromTopNeedle = fromTop;
                    while (fromTopNeedle < 0)
                    {
                        needle = needle.Up;
                    }
                    needle = needle.Right;
                    fromTopNeedle = fromTop;
                    while (fromTopNeedle < 0)
                    {
                        needle = needle.Down;
                    }
                    needle = value;
                }
            }
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
