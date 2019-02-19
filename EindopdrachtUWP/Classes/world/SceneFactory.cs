using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPTestApp
{
    static class SceneFactory
    {
        public static Random random = new Random((int) DateTime.UtcNow.Ticks);

        private static int tilesize = 32;

        private static bool[,] addRoom(bool[,] tiles, int left, int top, int width, int height)
        {
            int leftNeedle = left;
            while (leftNeedle < left + width)
            {
                int topNeedle = top;
                while (topNeedle < top + height)
                {
                    tiles[leftNeedle, topNeedle] = true;                    topNeedle++;
                }
                leftNeedle++;
            }

            return tiles;
        }

        public static Boolean spaceFree(bool[,] tiles, int left, int top, int width, int height)
        {
            int leftNeedle = left - 1;
            while (leftNeedle < left + width + 1)
            {                int topNeedle = top - 1;
                while (topNeedle < top + height + 1)
                {
                    //Check out of bounds
                    if (leftNeedle > tiles.GetLength(0) || leftNeedle < 0)
                    {
                        topNeedle++;
                        continue;
                    }
                    if (topNeedle > tiles.GetLength(1) || topNeedle < 0)
                    {
                        topNeedle++;
                        continue;
                    }
                    if (tiles[leftNeedle, topNeedle]) //if this square is true return false (aka no place)
                        return false;
                    topNeedle++;
                }
                leftNeedle++;
            }
            return true;
        }

        public static Scene GetScene(int width, int height)
        {

            bool[,] tiles = new bool[width / tilesize, height / tilesize];
            List<GameObject> Objects = new List<GameObject>();

            //Go for 100 passes trough, every pass makes a room and sees if its fits.
            for (int i = 0; i < 20; i++)
            {
                int newObject_Width = random.Next(4, 12);
                int newObject_Height= random.Next(4, 12);
                int left = random.Next(0, tiles.GetLength(0) - newObject_Width);
                int top = random.Next(0, tiles.GetLength(1) - newObject_Height);

                if (spaceFree(tiles,left,top, newObject_Width, newObject_Height))
                {
                    tiles = addRoom(tiles, left, top, newObject_Width, newObject_Height);

                    Objects.Add(new Wall(
                        newObject_Width * tilesize,
                        newObject_Height * tilesize,
                        left * tilesize,
                        top * tilesize,
                        0, 0, 0, 0));
                }
            }

            return new Scene(Objects);



            /* orignal map */
            return new Scene(new List<GameObject>
            {
                

                //Wall takes: width, height, fromLeft, fromTop, widthDrawOffset = 0, heightDrawOffset = 0, fromLeftDrawOffset = 0, fromTopDrawOffset = 0
                // outer walls
                /* new Wall(900, 100, -49, -99, 0, 0, 0, 0),
                new Wall(900, 100, -49, 599, 0, 0, 0, 0),
                new Wall(100, 700, -99, -49, 0, 0, 0, 0),
                new Wall(100, 700, 799, -49, 0, 0, 0, 0), */

                // garden top left
                new Wall(22, 183, 23, 23, 0, 0, 0, 0),
                new Wall(183, 22, 23, 23, 0, 0, 0, 0),
                new Wall(68, 22, 23, 184, 0, 0, 0, 0),
                new Wall(22, 183, 184, 23, 0, 0, 0, 0),
                new Wall(45, 68, 115, 23, 0, 0, 0, 0),
                new Wall(68, 22, 138, 184, 0, 0, 0, 0),
                new Wall(22, 45, 69, 115, 0, 0, 0, 0),
                new Wall(68, 22, 138, 138, 0, 0, 0, 0),

                // bottom rooms
                new Wall(22, 68, 23, 414, 0, 0, 0, 0),
                new Wall(91, 22, 23, 414, 0, 0, 0, 0),
                new Wall(22, 68, 92, 414, 0, 0, 0, 0),
                new Wall(22, 68, 138, 414, 0, 0, 0, 0),
                new Wall(91, 22, 138, 414, 0, 0, 0, 0),
                new Wall(22, 68, 207, 414, 0, 0, 0, 0),
                new Wall(22, 68, 253, 414, 0, 0, 0, 0),
                new Wall(91, 22, 253, 414, 0, 0, 0, 0),
                new Wall(22, 68, 322, 414, 0, 0, 0, 0),
                new Wall(22, 68, 368, 414, 0, 0, 0, 0),
                new Wall(91, 22, 368, 414, 0, 0, 0, 0),
                new Wall(22, 68, 437, 414, 0, 0, 0, 0),
                new Wall(22, 68, 483, 414, 0, 0, 0, 0),
                new Wall(91, 22, 483, 414, 0, 0, 0, 0),
                new Wall(22, 68, 552, 414, 0, 0, 0, 0),
                new Wall(22, 68, 23, 506, 0, 0, 0, 0),
                new Wall(91, 22, 23, 552, 0, 0, 0, 0),
                new Wall(22, 68, 92, 506, 0, 0, 0, 0),
                new Wall(22, 68, 138, 506, 0, 0, 0, 0),
                new Wall(91, 22, 138, 552, 0, 0, 0, 0),
                new Wall(22, 68, 207, 506, 0, 0, 0, 0),
                new Wall(22, 68, 253, 506, 0, 0, 0, 0),
                new Wall(91, 22, 253, 552, 0, 0, 0, 0),
                new Wall(22, 68, 322, 506, 0, 0, 0, 0),
                new Wall(22, 68, 368, 506, 0, 0, 0, 0),
                new Wall(91, 22, 368, 552, 0, 0, 0, 0),
                new Wall(22, 68, 437, 506, 0, 0, 0, 0),
                new Wall(22, 68, 483, 506, 0, 0, 0, 0),
                new Wall(91, 22, 483, 552, 0, 0, 0, 0),
                new Wall(22, 68, 552, 506, 0, 0, 0, 0),

                // room bottom right
                new Wall(22, 68, 598, 414, 0, 0, 0, 0),
                new Wall(160, 22, 598, 414, 0, 0, 0, 0),
                new Wall(22, 160, 736, 414, 0, 0, 0, 0),
                new Wall(22, 68, 598, 506, 0, 0, 0, 0),
                new Wall(160, 22, 598, 552, 0, 0, 0, 0),

                //cars
                new Wall(72, 30, 628, 280, 0, 0, 0, 0),

                // houses
                new Wall(160, 91, 207, 0, 0, 0, 0, 0),
                new Wall(160, 91, 414, 0, 0, 0, 0, 0),
                new Wall(160, 91, 621, 0, 0, 0, 0, 0),
                new Wall(160, 91, 207, 116, 0, 0, 0, 0),
                new Wall(160, 91, 414, 116, 0, 0, 0, 0),
                new Wall(160, 91, 621, 116, 0, 0, 0, 0),

                // lampposts
                new Wall(22, 22, 46, 230, 0, 0, 0, 0),
                new Wall(22, 22, 161, 230, 0, 0, 0, 0),
                new Wall(22, 22, 276, 230, 0, 0, 0, 0),
                new Wall(22, 22, 391, 230, 0, 0, 0, 0),
                new Wall(22, 22, 506, 230, 0, 0, 0, 0),
                new Wall(22, 22, 621, 230, 0, 0, 0, 0),
                new Wall(22, 22, 736, 230, 0, 0, 0, 0),
                new Wall(22, 22, 46, 368, 0, 0, 0, 0),
                new Wall(22, 22, 161, 368, 0, 0, 0, 0),
                new Wall(22, 22, 276, 368, 0, 0, 0, 0),
                new Wall(22, 22, 391, 368, 0, 0, 0, 0),
                new Wall(22, 22, 506, 368, 0, 0, 0, 0),
                new Wall(22, 22, 621, 368, 0, 0, 0, 0),
                new Wall(22, 22, 736, 368, 0, 0, 0, 0),

                // extra walls that prevent clipping
                new Wall(26, 35, 33, 33, 0, 0, 0, 0),
                new Wall(35, 26, 33, 33, 0, 0, 0, 0),
                new Wall(2, 2, 160, 45, 0, 0, 0, 0),
                new Wall(2, 2, 206, 90, 0, 0, 0, 0),
                new Wall(6, 6, -3, -3, 0, 0, 0, 0),
                new Wall(2, 2, 366, -1, 0, 0, 0, 0),
                new Wall(2, 2, 573, -1, 0, 0, 0, 0),
                new Wall(2, 2, 780, -1, 0, 0, 0, 0),
                new Wall(2, 2, 801, -1, 0, 0, 0, 0),
                new Wall(2, 2, 44, 435, 0, 0, 0, 0),
                new Wall(2, 2, 159, 435, 0, 0, 0, 0),
                new Wall(2, 2, 274, 435, 0, 0, 0, 0),
                new Wall(2, 2, 389, 435, 0, 0, 0, 0),
                new Wall(2, 2, 504, 435, 0, 0, 0, 0),
                new Wall(2, 2, 619, 435, 0, 0, 0, 0),

                //new Spawner(10, 10, 400, 300, 0, 0, 0, 0, 5000, 10000)
            });
        }

    }
}
