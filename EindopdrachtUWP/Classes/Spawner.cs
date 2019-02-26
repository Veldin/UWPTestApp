using System;
using System.Collections.Generic;

namespace UWPTestApp
{
    class Spawner : GameObject
    {
        protected float beginDelta;                 //The delta till this spawner starts.
        protected float totalDelta;                 //The time this spawner is alive.
        protected float cooldownDelta;              //The max delta it takes to spawn the next, after it spawned a guy.
        protected float remainingCooldownDelta;     //the delta it takes to spawn the next.

        public Spawner(float width, float height, float fromLeft, float fromTop, float widthDrawOffset = 0, float heightDrawOffset = 0, float fromLeftDrawOffset = 0, float fromTopDrawOffset = 0, float beginDelta = 3000, float cooldownDelta = 4000)
        : base(width, height, fromLeft, fromTop, widthDrawOffset, heightDrawOffset, fromLeftDrawOffset, fromTopDrawOffset)
        {
            this.beginDelta = beginDelta;
            this.cooldownDelta = cooldownDelta;
            this.remainingCooldownDelta = 0;

            Location = "Assets/Sprites/Maps/Spawner.png";
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
        public override bool OnTick(List<GameObject> gameObjects, float delta)
        {
            if (BeginDelta > 0 )
            {
                BeginDelta = (BeginDelta - delta);
                return true;
            }

            totalDelta += delta;

            //Check if its time to spawn a new enemy!
            if (remainingCooldownDelta - delta < 0)
            {
                int playerLevel = 1;
                foreach (GameObject go in gameObjects)
                {
                    if (go is Player player)
                    {
                        playerLevel = player.GetLevel();
                        break;
                    }
                }

                RemainingCooldownDelta = (cooldownDelta / playerLevel);
                if (RemainingCooldownDelta < 1000) remainingCooldownDelta++;
                
                //Spawn a gameobject!
                float spawnSizeWidth = 15;
                float spawnSizeHight = 15;
                Random rand = new Random();
                float enemySize = rand.Next(10, 15);
                float spawnFromLeft = FromLeft + (Width / 2) - (spawnSizeWidth / 2);
                float spawnFromTop = FromTop + (Height / 2) - (spawnSizeHight / 2);

                Enemy enemy = new Enemy(enemySize, enemySize, spawnFromLeft, spawnFromTop, 0, 10, 0, -10);

                //Give a pickup to some enemies
                Random random = new Random();
                if(random.Next(0,6) > 3)
                {
                    enemy.AddTag("droppickup");
                }

                enemy.SetPower( 1.0f + ( 0.1f * playerLevel ) );
                enemy.SetLifePoints(275 + ( 25 * playerLevel ) );
                gameObjects.Add(enemy);
            }
            else
            {
                RemainingCooldownDelta = remainingCooldownDelta - delta;
            }
            return true;
        }

        public override bool CollitionEffect(GameObject gameObject)
        {
            return true;
        }
    }
}
