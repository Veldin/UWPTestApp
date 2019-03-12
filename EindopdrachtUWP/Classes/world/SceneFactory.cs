using System;
using System.Collections.Generic;

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

        public static bool spaceFree(bool[,] tiles, int left, int top, int width, int height)
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
            for (int i = 0; i < 200; i++)
            {
                int newObject_Width = random.Next(3, 7);
                int newObject_Height= random.Next(3, 7);
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

            //Add the spawner
            if (random.Next(0, 10) > 5) {  
                Objects.Add(new Spawner(10, 10, 400, 300, 0, 0, 0, 0, random.Next(0, 14000), random.Next(14000, 50000)));
            }

            Objects.Add(new Spawner(10, 10, random.Next(0, width), random.Next(0, height), 0, 0, 0, 0, random.Next(0, 14000), random.Next(14000, 50000)));

            return new Scene(Objects);
        }
    }
}
