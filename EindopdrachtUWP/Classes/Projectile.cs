using System;
using System.Collections.Generic;
using UWPTestApp;
using Windows.UI;

public class Projectile : GameObject, MovableObject
{
    private float shotFromTop;
    private float shotFromLeft;
    private float damage;
    private float movementSpeed;

    public string DeathSound { get; set; }
    public string MoveSound { get; set; }

    private List<GameObject> hitGameobject;

    public Projectile(float width, float height, float fromLeft, float fromTop, float widthDrawOffset = 0, float heightDrawOffset = 0, float fromLeftDrawOffset = 0, float fromTopDrawOffset = 0, float damage = 0, float shotFromLeft = 0, float shotFromTop = 0)
        : base(width, height, fromLeft, fromTop, widthDrawOffset, heightDrawOffset, fromLeftDrawOffset, fromTopDrawOffset)
    {
        this.shotFromTop = shotFromTop;
        this.shotFromLeft = shotFromLeft;
        this.damage = damage;

        Target = new Target(shotFromLeft, shotFromTop);

        hitGameobject = new List<GameObject>();

        Location = "Assets/Sprites/Enemy_Sprites/Enemy_Top.png";

        movementSpeed = 700;
    }

    public override bool CollitionEffect(GameObject gameObject)
    {
        Targetable targetable = gameObject as Targetable;
        if (targetable is Targetable)
        {
            Enemy enemy = gameObject as Enemy;
            if (enemy is Enemy)
            {
                //Check if an enemy is not already hit by this projectile.
                if (!hitGameobject.Contains(gameObject))
                {
                    enemy.AddLifePoints(damage * -1);
                    enemy.AddTag("splatter");

                 
                    AddTag("text");
                   
                }

                hitGameobject.Add(enemy);
                return true;
            }
            return false;
        }
        else if (gameObject.HasTag("solid") && !HasTag("laser"))
        {
            AddTag("destroyed");
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool SetNewHomingTarget(List<GameObject> gameObjects)
    {
        //to find the nearest target there needs to be a target to compare to.
        Targetable nearestTarget = null;
        float nearestTotalDifferenceAbs = 0; 

        //Loop trough the gameObjects to check for potential targets
        foreach (GameObject gameObject in gameObjects)
        {
            Enemy enemy = gameObject as Enemy;
            if (enemy is Enemy)
            {
                Targetable targetable = enemy as Targetable;
                if (targetable is Targetable)
                {
                    if (targetable != null)
                    {
                        //To calculate the distance, get the absolute distance.
                        float differenceLeftAbs = Math.Abs(targetable.FromLeft() - FromLeft);
                        float differenceTopAbs = Math.Abs(targetable.FromTop() - FromTop);
                        float totalDifferenceAbs = differenceLeftAbs + differenceTopAbs;

                        if (nearestTarget == null) //If there was no other target found (yet)
                        {
                            //Set the target, and the difference to check if next targets are closer.
                            nearestTarget = targetable;
                            nearestTotalDifferenceAbs = totalDifferenceAbs;
                        }
                        else if(totalDifferenceAbs < nearestTotalDifferenceAbs) //If this target is closer then the last
                        {
                            //Set the target
                            nearestTarget = targetable;
                            nearestTotalDifferenceAbs = totalDifferenceAbs;
                        }
                    }
                }
            }
        }

        //If there was a target found
        if (nearestTarget != null)
        {
            //Set the target
            Target = new Target(nearestTarget);
            return true;
        }

        return false;
    }

    public void setMovementSpeed(float movementSpeed)
    {
        this.movementSpeed = movementSpeed;
    }

    public bool SetNewCurvedTarget(List<GameObject> gameObjects)
    {
        

        foreach (GameObject gameObject in gameObjects)
        {
            Player player = gameObject as Player;
            if (player is Player)
            {

                float differenceLeftAbs = Math.Abs(player.FromLeft - FromLeft);
                float differenceTopAbs = Math.Abs(player.FromTop - FromTop);

                float totalDifferenceAbs = differenceLeftAbs + differenceTopAbs;

                if (totalDifferenceAbs > 120)
                {
                    AddTag("returning");
                    return targetPlayer(gameObjects);
                }
            }
        }


        return false;
    }

    public bool targetPlayer(List<GameObject> gameObjects)
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

    public bool SetNewTarget(List<GameObject> gameObjects)
    {
        if (HasTag("homing"))
        {
            SetNewHomingTarget(gameObjects);
        }


        if (HasTag("curved"))
        {
            SetNewCurvedTarget(gameObjects);
        }

        return false;
    }

    public override bool OnTick(List<GameObject> gameObjects, float delta)
    {
        SetNewTarget(gameObjects);

        float differenceLeftAbs = Math.Abs(Target.FromLeft() - FromLeft);
        float differenceTopAbs = Math.Abs(Target.FromTop() - FromTop);

        float totalDifferenceAbs = differenceLeftAbs + differenceTopAbs;

        float differenceTopPercent = differenceTopAbs / (totalDifferenceAbs / 100);
        float differenceLeftPercent = differenceLeftAbs / (totalDifferenceAbs / 100);

        float moveTopDistance = movementSpeed * (differenceTopPercent / 100);
        float moveLeftDistance = movementSpeed * (differenceLeftPercent / 100);

        if (Target.FromLeft() > FromLeft)
        {
            AddFromLeft((moveLeftDistance * delta) / 10000);
            Target.SetFromLeft(Target.FromLeft() + (moveLeftDistance * delta) / 10000);
        }
        else
        {
            AddFromLeft(((moveLeftDistance * delta) / 10000) * -1);
            Target.SetFromLeft(Target.FromLeft() + ((moveLeftDistance * delta) / 10000) * -1);
        }

        if (Target.FromTop() > FromTop)
        {
            AddFromTop((moveTopDistance * delta) / 10000);
            Target.SetFromTop(Target.FromTop() + (moveTopDistance * delta) / 10000);
        }
        else
        {
            AddFromTop(((moveTopDistance * delta) / 10000) * -1);
            Target.SetFromTop(Target.FromTop() + ((moveTopDistance * delta) / 10000) * -1);
        }

        if (HasTag("returning"))
        {
            if(totalDifferenceAbs < 5)
            {
                AddTag("destroyed");
            }
        }

        if (HasTag("speeding"))
        {
            damage += (delta / 6);
            movementSpeed += (delta * 1.5f);
        }

        //If a projectile has the tag text it has been ordered to drop a textbox. This means the projectile hit a target.
        if (HasTag("text"))
        {
            TextBox textBox = new TextBox(50, 50, fromLeft, fromTop - 20, 0, 0, 0, 0, Math.Round(damage, 2).ToString(), 1000);

            if (HasTag("crit"))
            {
                textBox.Color = Colors.Red;
                textBox.FontSize++;
            }

            if (HasTag("double") || HasTag("ghost"))
            {
                RemoveTag("double");
            }
            else
            {
                AddTag("destroyed");
            }

            RemoveTag("text");
            gameObjects.Add(textBox);
        }

        return true;
    }

    public void SetLocation(string location)
    {
        Location = location;
    }

    void MovableObject.SetMovementSpeed(float speed)
    {
        movementSpeed = speed;
    }

    float MovableObject.GetMovementSpeed()
    {
        return movementSpeed;
    }
}
