using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPTestApp;

namespace EindopdrachtUWP
{
    static public class tests
    {
        public static void activeObjectsTest()
        {
            Player player = new Player(0, 0, 0, 0, 0, 0, 0, 0);

            long then = Stopwatch.GetTimestamp();

            List<GameObject> gameObjects = new List<GameObject>();
            for (int i = 0; i < 8000000; i++)
            {
                gameObjects.Add(new Enemy(0,0,0,0));
            }

            long now = Stopwatch.GetTimestamp();
            long delta = (now - (long)then);
            Debug.WriteLine("Added objects to list in: {0}", delta);

            /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
             * Traditional Loop.
             */

            //Create list
            List<GameObject> activeObjects = new List<GameObject>();

            //Reset stopwatch
            then = Stopwatch.GetTimestamp();
            now = Stopwatch.GetTimestamp();

            //Foreach trough the gameobjects
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.IsActive(player))
                {
                    activeObjects.Add(gameObject);
                }
            }

            //Count the time
            now = Stopwatch.GetTimestamp();
            delta = (now - (long)then);

            Debug.WriteLine("Traditional Loop: Added objects to activeObjects: {0}", delta);

            /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
             * lambda linq
             */

            //Reset stopwatch
            then = Stopwatch.GetTimestamp();
            now = Stopwatch.GetTimestamp();

            //Foreach trough the gameobjects
            IEnumerable<GameObject> activeObjectss = gameObjects.Where(element => element.IsActive(player) == true);

            //Count the time
            now = Stopwatch.GetTimestamp();
            delta = (now - (long)then);

            Debug.WriteLine("lambda linq: Added objects to activeObjects: {0}", delta);

            /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
            * lambda linq
            */
            //Reset stopwatch
            then = Stopwatch.GetTimestamp();
            now = Stopwatch.GetTimestamp();

            //Foreach trough the gameobjects
            IEnumerable<GameObject> activeObjectsss = gameObjects.AsParallel().Where(element => element.IsActive(player) == true);

            //Count the time
            now = Stopwatch.GetTimestamp();
            delta = (now - (long)then);

            Debug.WriteLine("lambda plinq: Added objects to activeObjects: {0}", delta);
        }


    }
}
