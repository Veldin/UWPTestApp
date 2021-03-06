﻿using EindopdrachtUWP;
using EindopdrachtUWP.Classes;
using System;
using System.Collections.Generic;
using UWPTestApp;

public class Enemy : GameObject, MovableObject, Targetable
{
    private float lifePoints; 
    private float maxLifePoints;
    private float power; 

    private float damage; 
    private bool ableToHit;
    private float damageCountDownTimer;
    private float damageCountDownTimerMax;

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

    private float movementSpeed;

    public Enemy(float width, float height, float fromLeft, float fromTop, float widthDrawOffset = 0, float heightDrawOffset = 0, float fromLeftDrawOffset = 0, float fromTopDrawOffset = 0)
        : base(width, height, fromLeft, fromTop, widthDrawOffset, heightDrawOffset, fromLeftDrawOffset, fromTopDrawOffset)
    {
        AddTag("hostile");
        AddTag("solid");

        //Default movespeed and lifePoints are both 300. They can be set later on.
        movementSpeed = 220;
        lifePoints = 300;
        damage = 50;
        maxLifePoints = lifePoints;
        ableToHit = true;
        damageCountDownTimerMax = 3000;

        Location = "Assets/Sprites/Enemy_Sprites/Enemy_Bottom.png";

        Random r = new Random();
        DeathSound = DeathSounds[r.Next(9)];
    }

    public void SetLifePoints(float life)
    {
        lifePoints = life;
        maxLifePoints = life;
    }

    public void AddLifePoints(float life)
    {
        lifePoints += life;
    }

    public float GetLifePoints()
    {
        return lifePoints;
    }

    public float GetMaxLifePoints()
    {
        return maxLifePoints;
    }

    public void SetPower(float power)
    {
        this.power = power;
    }

    public float GetPower()
    {
        return power;
    }

    void MovableObject.SetMovementSpeed(float speed)
    {
        movementSpeed = speed;
    }

    float MovableObject.GetMovementSpeed()
    {
        return movementSpeed;
    }

    public override Boolean CollitionEffect(GameObject gameObject) {
        if (gameObject.HasTag("solid"))
        {
            //We had this a recursive function first, that fired until there was no collition.
            //But this made them warp further due to them pushing eachother 
            int maxCollitions = 9000;
            while (IsColliding(gameObject) && maxCollitions > 0)
            {
                //Check collition from the left or right.
                if ((gameObject.FromLeft + gameObject.Width) >= (FromLeft + Width))
                {
                    AddFromLeft(-1);
                }

                if ((gameObject.FromLeft + gameObject.Width) < (FromLeft + Width))
                {
                    AddFromLeft(1);
                }

                //Check collition from top or bottom.
                if ((gameObject.FromTop + gameObject.Height) >= (FromTop + Height))
                {
                    AddFromTop(-1);
                }

                if ((gameObject.FromTop + gameObject.Height) < (FromTop + Height))
                {
                    AddFromTop(1);
                }

                maxCollitions--;
            }
        }

        //If its an instance of player
        Player player = gameObject as Player;
        if (player is Player)
        {
            if (ableToHit)
            {
                if (player.getArmour() <= 0)
                {
                    player.IncreaseHealth(GetPower() * damage * -1);
                    ableToHit = false;
                    MainPage.Current.updateHealth();
                }
                else
                {
                    player.IncreaseArmour(GetPower() * damage * -1);
                    if(player.getArmour() < 0)
                    {
                        player.IncreaseHealth(player.getArmour());
                        player.setArmour(0);
                        MainPage.Current.updateHealth();
                    }
                    ableToHit = false;
                    MainPage.Current.updateArmour();
                }
                player.AddTag("hit");
            }
            MainPage.Current.killstreak = 0;
            MainPage.Current.updateKillstreak();
        }
        return true;
    }

    private bool getThenSetTarget(List<GameObject> gameObjects)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            Player player = gameObject as Player;
            if (player is Player)
            {
                Targetable targetable = player as Targetable;
                if (targetable is Targetable)
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

    public override bool OnTick(List<GameObject> gameObjects, float delta)
    {
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
            getThenSetTarget(gameObjects);
        }
        else
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

            Boolean facingTop;
            Boolean facingLeft;

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
            String newDirection;
            Boolean directionChanged = false;

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

            if (newDirection.Equals(direction))
            {
                
            }
            else
            {
                Location = "Assets/Sprites/Enemy_Sprites/Enemy_" + newDirection + ".png";
                if (HasTag("droppickup"))
                {
                    Location = "Assets/Sprites/Enemy_Sprites/Enemy2_" + newDirection + ".png";
                }
                Sprite = null;
                direction = newDirection;
            }
        }

        //Check dead
        if (lifePoints <= 0)
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
                gameObjects.Add(new Pickup(15, 17, FromLeft, FromTop));
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

    float Targetable.FromTop(){
        return FromTop;
    }

    float Targetable.FromLeft()
    {
        return FromLeft;
    }
}
