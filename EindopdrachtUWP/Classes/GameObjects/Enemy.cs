using EindopdrachtUWP;
using EindopdrachtUWP.Classes;
using System;
using System.Collections.Generic;
using UWPTestApp;

public class Enemy : GameObject, ITargetable
{
    private float lifePoints;
    public float MaxLifePoints { get; set; }
    protected float power;
    private readonly float damage;
    private float movementSpeed;
    private bool ableToHit;
    private float damageCountDownTimer;
    private readonly float damageCountDownTimerMax;

    public string DeathSound { get; set; }
    public string MoveSound { get; set; }

    public static string[] DeathSounds = new string[]
    {
        "Enemy_Sounds\\Zombie_Death1.wav",
        "Enemy_Sounds\\Zombie_Death2.wav",
        "Enemy_Sounds\\Zombie_Death3.wav",
        "Enemy_Sounds\\Zombie_Death4.wav",
        "Enemy_Sounds\\Zombie_Death5.wav",
        "Enemy_Sounds\\Zombie_Death6.wav",
        "Enemy_Sounds\\Zombie_Death7.wav",
        "Enemy_Sounds\\Zombie_Death8.wav",
        "Enemy_Sounds\\Zombie_Death9.wav",
    };

    public Enemy(float width, float height, float fromLeft, float fromTop, float widthDrawOffset = 0, float heightDrawOffset = 0, float fromLeftDrawOffset = 0, float fromTopDrawOffset = 0)
        : base(width, height, fromLeft, fromTop, widthDrawOffset, heightDrawOffset, fromLeftDrawOffset, fromTopDrawOffset)
    {
        AddTag("hostile");  //This gameobject is hostyle
        AddTag("solid");    //This gameobject has collition

        //Default statistics
        movementSpeed = 190;
        LifePoints = 300;
        damage = 50;
        MaxLifePoints = LifePoints;

        //After every attack the attack goes on cooldown.
        ableToHit = true;
        damageCountDownTimerMax = 2500;

        //Set default sprite
        Location = "Assets/Sprites/Enemy_Sprites/Enemy_Bottom.gif";

        //Get a deathsound
        Random rand = new Random();
        DeathSound = DeathSounds[rand.Next(9)];
    }

    /*
     * Used to set the lifepoints, also sets max lifepoints
     */
    public float LifePoints
    {
        get { return lifePoints; }
        set
        {
            lifePoints = value;
            MaxLifePoints = value;
        }
    }

    public void AddLifePoints(float life)
    {
        lifePoints += life;
    }

    public float Power
    {
        get { return power; }
        set { power = value; }
    }

    public void AddMovementSpeed(float movementSpeed)
    {
        this.movementSpeed += movementSpeed;
    }

    public override bool CollisionEffect(GameObject gameObject)
    {
        if (gameObject.HasTag("solid")) //Check if the gameobject should be considered solid
        {
            //We had this a recursive function first, that fired until there was no collision.
            //But this made them warp further due to them pushing eachother so a maxCollisions is set.
            int maxCollisions = 90;
            while (IsColliding(gameObject) && maxCollisions > 0)
            {
                //Check collision from the left or right.
                if ((gameObject.FromLeft + gameObject.Width) >= (FromLeft + Width))
                {
                    AddFromLeft(-1);
                }

                if ((gameObject.FromLeft + gameObject.Width) < (FromLeft + Width))
                {
                    AddFromLeft(1);
                }

                //Check collision from top or bottom.
                if ((gameObject.FromTop + gameObject.Height) >= (FromTop + Height))
                {
                    AddFromTop(-1);
                }

                if ((gameObject.FromTop + gameObject.Height) < (FromTop + Height))
                {
                    AddFromTop(1);
                }
                maxCollisions--;
            }
        }

        //If its an instance of player
        Player player = gameObject as Player;
        if (player is Player)
        {
            if (ableToHit)
            {
                if (player.Armour <= 0)
                {
                    player.IncreaseHealth(Power * damage * -1);
                    ableToHit = false;
                    MainPage.Current.UpdateHealth();
                }
                else
                {
                    player.IncreaseArmour(Power * damage * -1);
                    if (player.Armour < 0)
                    {
                        player.IncreaseHealth(player.Armour);
                        player.Armour = 0;
                        MainPage.Current.UpdateHealth();
                    }
                    ableToHit = false;
                    MainPage.Current.UpdateArmour();
                }
                player.AddTag("hit");
            }
            MainPage.Current.killstreak = 0;
            MainPage.Current.UpdateKillstreak();
        }
        return true;
    }

    private bool GetThenSetTarget(List<GameObject> gameObjects)
    {
        //return false;
        foreach (GameObject gameObject in gameObjects)
        {
            Player player = gameObject as Player;
            if (player is Player)
            {
                ITargetable targetable = player as ITargetable;
                if (targetable is ITargetable)
                {
                    if (targetable != null)
                    {
                        Target = new Target(targetable);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public override bool IsActive(GameObject gameObject)
    {
        if (Math.Abs(gameObject.FromLeft - this.FromLeft) < 2001)
        {
            return true;
        }

        if (Math.Abs(gameObject.FromTop - this.FromTop) < 2001)
        {
            return true;
        }
        return false;
    }

    public override bool OnTick(List<GameObject> gameObjects, float delta)
    {
        //After every attack the attack goes on cooldown.
        if (damageCountDownTimer - delta < 0)
        {
            damageCountDownTimer = damageCountDownTimerMax;
            ableToHit = true;
        }
        else
        {
            damageCountDownTimer -= delta;
        }

        //If you don't have a target, try to find one!
        if (Target == null)
        {
            GetThenSetTarget(gameObjects);
        }

        //This is not an if-else so it reavaluates target after the getThenSetTarget()
        if (Target != null)
        {
            //The difference between the target and this object
            float differenceLeftAbs = Math.Abs(Target.FromLeft() - FromLeft);
            float differenceTopAbs = Math.Abs(Target.FromTop() - FromTop);

            float totalDifferenceAbs = differenceLeftAbs + differenceTopAbs;

            float differenceTopPercent = differenceTopAbs / (totalDifferenceAbs / 100);
            float differenceLeftPercent = differenceLeftAbs / (totalDifferenceAbs / 100);
            //totalDifference = differenceTop + differenceLeft;
            //totalDifferenceAbs = differenceTopAbs + differenceLeftAbs;

            float moveTopDistance = movementSpeed * (differenceTopPercent / 100);
            float moveLeftDistance = movementSpeed * (differenceLeftPercent / 100);

            bool facingTop;
            bool facingLeft;

            if (Target.FromLeft() > FromLeft)
            {
                AddFromLeft((moveLeftDistance * delta) / 10000);
                facingLeft = true;
            }
            else
            {
                AddFromLeft(((moveLeftDistance * delta) / 10000) * -1);
                facingLeft = false;
            }

            if (Target.FromTop() > FromTop)
            {
                AddFromTop((moveTopDistance * delta) / 10000);
                facingTop = true;
            }
            else
            {
                AddFromTop(((moveTopDistance * delta) / 10000) * -1);
                facingTop = false;
            }

            //Get the direction 
            string newDirection;

            if (moveLeftDistance > moveTopDistance)
            {
                if (facingLeft)
                {
                    newDirection = "Right";
                    WidthDrawOffset = width / 2;
                    HeightDrawOffset = 0;
                    FromTopDrawOffset = 0;
                    FromLeftDrawOffset = 0;
                }
                else
                {
                    newDirection = "Left";
                    WidthDrawOffset = width / 2;
                    HeightDrawOffset = 0;
                    FromTopDrawOffset = 0;
                    FromLeftDrawOffset = width / 2 * -1;
                }
            }
            else
            {
                if (facingTop)
                {
                    newDirection = "Bottom";
                    WidthDrawOffset = 0;
                    HeightDrawOffset = width / 2;
                    FromTopDrawOffset = 0;
                    FromLeftDrawOffset = 0;
                }
                else
                {
                    newDirection = "Top";
                    WidthDrawOffset = 0;
                    HeightDrawOffset = width / 2;
                    FromTopDrawOffset = width / 2 * -1;
                    FromLeftDrawOffset = 0;
                }
            }

            if (newDirection.Equals(Direction))
            {
            }
            else
            {
                Location = "Assets/Sprites/Enemy_Sprites/Enemy_" + newDirection + ".gif";
                if (HasTag("droppickup"))
                {
                    Location = "Assets/Sprites/Enemy_Sprites/Enemy2_" + newDirection + ".gif";
                }
                Sprite = null;
                Direction = newDirection;
            }
        }

        //Check dead
        if (LifePoints <= 0)
        {
            Random random = new Random();
            int randomPositionOffsetOne = 0;
            int randomPositionOffsetTwo = 0;
            int randomSizeOffset = 0;

            for (int i = 0; i < 15; i++)
            {
                randomPositionOffsetOne = random.Next((int)width * -1, (int)width);
                randomPositionOffsetTwo = random.Next((int)width * -1, (int)width);
                randomSizeOffset = random.Next(((int)width * 75 / 100), ((int)width));

                gameObjects.Add(new Splatter(randomSizeOffset, randomSizeOffset, fromLeft + (width / 2) + randomPositionOffsetOne, fromTop + (height / 2) + randomPositionOffsetTwo));
            }
            RemoveTag("splatter");

            //check if this enemy has a pickup to drop
            if (HasTag("droppickup"))
            {
                gameObjects.Add(new Pickup(15, 17, FromLeft, FromTop)); //Drop a pickup (this is a random pickup)
                RemoveTag("droppickup");
            }
            AddTag("destroyed");
        }

        //Generate a splatter
        if (HasTag("splatter"))
        {
            Random random = new Random();
            int randomPositionOffsetOne = random.Next((int)width * -1, (int)width);
            int randomPositionOffsetTwo = random.Next((int)width * -1, (int)width);
            int randomSizeOffset = random.Next(((int)width * 75 / 100), ((int)width));

            gameObjects.Add(new Splatter(randomSizeOffset, randomSizeOffset, fromLeft + (width / 2) + randomPositionOffsetOne, fromTop + (height / 2) + randomPositionOffsetTwo));
            RemoveTag("splatter");
        }
        return true;
    }

    float ITargetable.FromTop()
    {
        return FromTop;
    }

    float ITargetable.FromLeft()
    {
        return FromLeft;
    }
}
