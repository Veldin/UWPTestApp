using System.Collections.Generic;

namespace UWPTestApp
{
    class Scene
    {
        private List<GameObject> gameObjects;
        
        public Scene()
        {
            gameObjects = new List<GameObject>();
        }

        public Scene(List<GameObject> gameObjects)
        {
            this.gameObjects = gameObjects;
        }

        public List<GameObject> GetScene()
        {
            return gameObjects;
        }
    }
}
