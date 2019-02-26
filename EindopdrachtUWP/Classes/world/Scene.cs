using System;
using System.Collections.Generic;

namespace UWPTestApp
{
    class Scene
    {
        private List<GameObject> gameObjects;
        private bool loaded;

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

        public bool Isloaded()
        {
            return loaded;
        }
    }
}
