using System.Collections.Generic;

namespace UWPTestApp
{
    class Scene
    {
        /*
         * Scenes are lists of gameObjects together with a bool that signals whether or not it has been loaded.
         * A Scene is considered loaded if LoadScene() was called bevore.
         */
        private List<GameObject> gameObjects;
        private bool loaded;

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
