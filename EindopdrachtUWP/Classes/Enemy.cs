using System;
using System.Collections.Generic;
using System.Diagnostics;
using UWPTestApp;
using Windows.UI.Xaml.Controls;

public class Enemy : GameObject, MovableObject, Targetable
{


    int lifePoints;
    int power;
    String enemyType;
    private MediaElement deathSound;
    private float movementSpeed;
    private MediaElement moveSound;

    public Enemy(float width, float height, float fromLeft, float fromTop, float widthDrawOffset = 0, float heightDrawOffset = 0, float fromLeftDrawOffset = 0, float fromTopDrawOffset = 0)
        : base(width, height, fromLeft, fromTop, widthDrawOffset, heightDrawOffset, fromLeftDrawOffset, fromTopDrawOffset)
    {
        AddTag("hostile");
        AddTag("solid");

        //Default movespeed and lifePoints are both 300. They can be set later on.
        movementSpeed = 300;
        lifePoints = 300;
    }

    public void SetLifePoints(int life)
    {
        lifePoints = life;
    }

    public int GetLifePoints()
    {
        return lifePoints;
    }

    public void SetPower(int power)
    {
        this.power = power;
    }

    public int GetPower()
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
            float topDifference = Target.FromTop() - FromTop;
            float leftDifference = Target.FromLeft() - FromLeft;

            //The absolute (not negative) difference between the target and this object
            float topDifferenceAbsolute = Target.FromTop() - FromTop;
            float leftDifferenceAbsolute = Target.FromLeft() - FromLeft;

            if (topDifference < 0) {//If difference is negative, make it positive
                topDifferenceAbsolute = topDifference * -1;
            }

            if (leftDifference < 0) {//If difference is negative, make it positive
                leftDifferenceAbsolute = leftDifference * -1;
            }


            float percentComplete = 0;
            if (topDifference < leftDifferenceAbsolute)
            {//If difference is negative, make it positive
                percentComplete = (0.5f + ((100f * leftDifferenceAbsolute) / topDifferenceAbsolute));
            }

            if (topDifference > leftDifferenceAbsolute)
            {//If difference is negative, make it positive
                percentComplete = (0.5f + (leftDifferenceAbsolute / (100f * topDifferenceAbsolute)));
            }

            //var change = ((V2 - V1) / Math.Abs(V1)) * 100;
            Debug.WriteLine(topDifferenceAbsolute + " - " + leftDifferenceAbsolute + " :percent: " + percentComplete);

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
