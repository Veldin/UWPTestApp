using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace UWPTestApp
{
    class Spawner : GameObject
    {
        protected float beginDelta;                 //The delta till this spawner starts.
        protected float cooldownDelta;              //The max delta it takes to spawn the next, after it spawned a guy.
        protected float remainingCooldownDelta;     //the delta it takes to spawn the next.
        protected float difficultyDelta;            //The delta to increase difficulty as time goes on.

        public Spawner(float width, float height, float fromLeft, float fromTop, float widthDrawOffset = 0, float heightDrawOffset = 0, float fromLeftDrawOffset = 0, float fromTopDrawOffset = 0, float beginDelta = 3000, float cooldownDelta = 4000)
        : base(width, height, fromLeft, fromTop, widthDrawOffset, heightDrawOffset, fromLeftDrawOffset, fromTopDrawOffset)
        {
            this.beginDelta = beginDelta;
            this.cooldownDelta = cooldownDelta;
            this.remainingCooldownDelta = 0;
            this.difficultyDelta = 0;
        }

        public float BeginDelta
        {
            get { return beginDelta; }
            set { beginDelta = value; }
        }

        public float CooldownDelta
        {
            get { return cooldownDelta; }
            set { cooldownDelta = value; }
        }

        public float RemainingCooldownDelta
        {
            get { return remainingCooldownDelta; }
            set { remainingCooldownDelta = value; }
        }

        //Any object can edit the gameObjects of the game while the logic is running.
        //And Also get the delta for timed events.
        public override Boolean OnTick(List<GameObject> gameObjects, float delta)
        {
            difficultyDelta += delta;
            Debug.WriteLine("lol: "+ difficultyDelta);


            if (BeginDelta > 0 )
            {
                BeginDelta = (BeginDelta - delta);

                return true;
            }

            if (remainingCooldownDelta - delta < 0)
            {
                RemainingCooldownDelta = cooldownDelta;
                //Spawn a gameobject!
                float spawnSizeWidth = 15;
                float spawnSizeHight = 15;
                Random rand = new Random();
                float enemySize = rand.Next(10, 15);
                float spawnFromLeft = FromLeft + (Width / 2) - (spawnSizeWidth / 2);
                float spawnFromTop = FromTop + (Height / 2) - (spawnSizeHight / 2);

                //Spawn an anemy
                gameObjects.Add(new Enemy(enemySize, enemySize, spawnFromLeft, spawnFromTop, 0, 10, 0, -10));

                
            }
            else
            {
                RemainingCooldownDelta = remainingCooldownDelta - delta;
            }

            return true;
        }

        public override Boolean CollitionEffect(GameObject gameObject)
        {
            return true;
        }
    }
}
