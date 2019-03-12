using System;
using System.Collections.Generic;
using System.Diagnostics;
using UWPTestApp;
using Windows.UI;

public class Projectile : GameObject, MovableObject
{
    private float shotFromTop;
    private float shotFromLeft;
    private float damage;
    private float movementSpeed;
    private float distanceTillDestroyed = 1000;         // The distance that the projectile can move before it get's destroyed

    public string DeathSound { get; set; }
    public string MoveSound { get; set; }

    private List<GameObject> hitGameobject;

    public Projectile(float width, float height, float fromLeft, float fromTop, float widthDrawOffset = 0, float heightDrawOffset = 0, float fromLeftDrawOffset = 0, float fromTopDrawOffset = 0, float damage = 0, float shotFromLeft = 0, float shotFromTop = 0, float distanceTillDestroyed = 0)
        : base(width, height, fromLeft, fromTop, widthDrawOffset, heightDrawOffset, fromLeftDrawOffset, fromTopDrawOffset)
    {
        this.shotFromTop = shotFromTop;
        this.shotFromLeft = shotFromLeft;
        this.damage = damage;
        this.distanceTillDestroyed = distanceTillDestroyed;

        // If distanceTillDestroyed is still 0 set it on 1000
        if (this.distanceTillDestroyed == 0)
        {
            this.distanceTillDestroyed = 1000;
            Debug.WriteLine("Error: No distanceTillDestroyed is given so it is set to 1000");
        }

        Target = new Target(shotFromLeft, shotFromTop);

        hitGameobject = new List<GameObject>();

        Location = "Assets/Sprites/Enemy_Sprites/Enemy_Top.png";

        movementSpeed = 700;
    }

    public override bool CollisionEffect(GameObject gameObject)
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
                        Target.SetTarget(0,0);
 
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

    public override bool IsActive(GameObject gameObject)
    {
        //Projectiles are always active.
        return true;
    }

    /*********************************************************************************************
     * In this function the movement of the projectile gets moved, a textbox of the damage is created
     * and the projectile will be destroyed here.
     * Delta is the difference in time between the previous tick en this tick (time elapsed).
     * gameobjecs are all gameobject that exists.
     ********************************************************************************************/
    public override bool OnTick(List<GameObject> gameObjects, float delta)
    {
        SetNewTarget(gameObjects);

        // Difference between location of projectile and target (direction) 
        float differenceLeftAbs = Math.Abs(Target.FromLeft() - FromLeft);
        float differenceTopAbs = Math.Abs(Target.FromTop() - FromTop);

        // Distance that needs to be traveled
        float totalDifferenceAbs = differenceLeftAbs + differenceTopAbs;

        // Precentages that the movement is top or left
        float differenceTopPercent = differenceTopAbs / (totalDifferenceAbs / 100);
        float differenceLeftPercent = differenceLeftAbs / (totalDifferenceAbs / 100);

        // Movement that the projectale is moving this tick
        float moveTopDistance = movementSpeed * (differenceTopPercent / 100);
        float moveLeftDistance = movementSpeed * (differenceLeftPercent / 100);

        // Move the projectile to the left or to the right
        if (Target.FromLeft() > FromLeft)       // Projectile goes left
        {
            AddFromLeft((moveLeftDistance * delta) / 10000);
            Target.SetFromLeft(Target.FromLeft() + (moveLeftDistance * delta) / 10000);
        }
        else                                    // Projectile goes right
        {
            AddFromLeft(((moveLeftDistance * delta) / 10000) * -1);
            Target.SetFromLeft(Target.FromLeft() + ((moveLeftDistance * delta) / 10000) * -1);
        }

        // Decrease the distanceTillDestroyed by the distance that the projectile has moved left or right
        distanceTillDestroyed -= (moveLeftDistance * delta) / 10000;

        // Move the projectile up or down
        if (Target.FromTop() > FromTop)          // Projectile goes up
        {
            AddFromTop((moveTopDistance * delta) / 10000);
            Target.SetFromTop(Target.FromTop() + (moveTopDistance * delta) / 10000);
        }
        else                                     // Projectile goes down
        {
            AddFromTop(((moveTopDistance * delta) / 10000) * -1);
            Target.SetFromTop(Target.FromTop() + ((moveTopDistance * delta) / 10000) * -1);
        }

        // Decrease the distanceTillDestroyed by the distance that the projectile has moved up or down
        distanceTillDestroyed -= (moveTopDistance * delta) / 10000;

        // Check if the distanceTillDestroyed is 0 or smaller. If that's the case tag the projectile as destroyed.
        if (distanceTillDestroyed <= 0)
        {
            AddTag("destroyed");
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
            movementSpeed += (delta * 3f);
        }

        // Decreases the speed of the projectile over time
        if (HasTag("slowing"))
        {
            movementSpeed -= (delta * 1f);
            if (movementSpeed <= 450)
            {
                movementSpeed = 450;
            }
        }

        // Increases the damage of the projectile over time
         if (HasTag("amplified"))
        {
            damage += (delta / 6);
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
