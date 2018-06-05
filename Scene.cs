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
        
        public Scene()
        {
            gameObjects = new List<GameObject>();
        }

        public void AddGameObject(GameObject go)
        {
            gameObjects.Add(go);
        }

        public List<GameObject> GetScene()
        {
            return this.gameObjects;
        }


    }
}
