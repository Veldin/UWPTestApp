﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using UWPTestApp;
using Windows.UI.Xaml.Controls;

public class Enemy : GameObject, MovableObject, Targetable
{


    float lifePoints;
    float power;
    String enemyType;
    private MediaElement deathSound;
    private float movementSpeed;
    private MediaElement moveSound;

    private String direction;

    public Enemy(float width, float height, float fromLeft, float fromTop, float widthDrawOffset = 0, float heightDrawOffset = 0, float fromLeftDrawOffset = 0, float fromTopDrawOffset = 0)
        : base(width, height, fromLeft, fromTop, widthDrawOffset, heightDrawOffset, fromLeftDrawOffset, fromTopDrawOffset)
    {
        AddTag("hostile");
        AddTag("solid");

        //Default movespeed and lifePoints are both 300. They can be set later on.
        movementSpeed = 350;
        lifePoints = 300;

        this.Location = "Assets/Sprites/Enemy_Sprites/Enemy_Bottom.png";
    }

    public void SetLifePoints(float life)
    {
        lifePoints = life;
    }

    public void AddLifePoints(float life)
    {
        lifePoints += life;
    }

    public float GetLifePoints()
    {
        return lifePoints;
    }

    public void SetPower(int power)
    {
        this.power = power;
    }

    public float GetPower()
    {
        return power;
    }

    public void SetType(String type)
    {
        enemyType = type;
    }

    public String GetEnemyType()
    {
        return enemyType;
    }

    void MovableObject.SetMovementSpeed(float speed)
    {
        movementSpeed = speed;
    }

    float MovableObject.GetMovementSpeed()
    {
        return movementSpeed;
    }

    void MovableObject.SetMoveSound(MediaElement moveSound)
    {
        this.moveSound = moveSound;
    }

    void MovableObject.PlayMoveSound()
    {

    }

    void MovableObject.SetDeathSound(MediaElement deathSound)
    {
        this.deathSound = deathSound;
    }

    void MovableObject.PlayDeathSound()
    {
        //iets 
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
            player.IncreaseHealth(-1);
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
        //throw new NotImplementedException();

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
                }
                else
                {
                    newDirection = "Left";
                }
            }
            else
            {
                if (facingTop)
                {
                    newDirection = "Bottom";
                }
                else
                {
                    newDirection = "Top";
                }
            }

            if (newDirection.Equals(direction))
            {
                
            }
            else
            {
                this.Location = "Assets/Sprites/Enemy_Sprites/Enemy_" + newDirection + ".png";
                this.Sprite = null;
                direction = newDirection;
            }
            //AddFromLeft(moveTopDistance * delta / 1000);
            //AddFromTop(moveLeftDistance * delta / 1000);

            //Debug.WriteLine(" differenceTopPercent: " + (moveTopDistance * delta / 1000) + " - differenceLeftPercent: " + (moveLeftDistance * delta / 1000));
        }


        //Check dead
        if (lifePoints < 0)
        {
            Random random = new Random();

            int randomPositionOffsetOne = 0;
            int randomPositionOffsetTwo = 0;

            int randomSizeOffset = 0;

            for (int i = 0; i < 10; i++)
            {
                randomPositionOffsetOne = random.Next((int)width * -1, (int)width);
                randomPositionOffsetTwo = random.Next((int)width * -1, (int)width);

                randomSizeOffset = random.Next(((int)width * 75 / 100), ((int)width));

                gameObjects.Add(new Splatter(randomSizeOffset, randomSizeOffset, fromLeft + (width / 2) + randomPositionOffsetOne, fromTop + (height / 2) + randomPositionOffsetTwo));

            }

            //gameObjects.Add(new Splatter(randomSizeOffset, randomSizeOffset, fromLeft + (width / 2) + randomPositionOffset, fromTop + (height / 2) + randomPositionOffset));
            this.RemoveTag("splatter");


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
            this.RemoveTag("splatter");
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
