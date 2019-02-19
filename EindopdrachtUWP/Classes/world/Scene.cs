using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPTestApp
{
    class Scene
    {
        private List<GameObject> gameObjects;
        private Boolean loaded;

        public Scene()
        {
            gameObjects = new List<GameObject>();
            loaded = false;
        }

        public Scene(List<GameObject> gameObjects)
        {
            this.gameObjects = gameObjects;
        }

        public List<GameObject> GetScene()
        {
            return gameObjects;
        }

        public List<GameObject> LoadScene()
        {
            loaded = true;
            return GetScene();
        }

        public Boolean Isloaded()
        {
            return loaded;
        }
    }
}
