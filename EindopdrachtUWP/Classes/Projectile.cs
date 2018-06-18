using System;
using System.Collections.Generic;
using System.Diagnostics;
using UWPTestApp;
using Windows.UI.Xaml.Controls;

public class Projectile : GameObject, MovableObject
{
    float shotFromTop;
    float shotFromLeft;
    float damage;
    float movementSpeed;
    MediaElement moveSound;
    MediaElement deathSound;

    public Projectile(float width, float height, float fromLeft, float fromTop, float widthDrawOffset = 0, float heightDrawOffset = 0, float fromLeftDrawOffset = 0, float fromTopDrawOffset = 0, float damage = 0, float shotFromLeft = 0, float shotFromTop = 0)
        : base(width, height, fromLeft, fromTop, widthDrawOffset, heightDrawOffset, fromLeftDrawOffset, fromTopDrawOffset)
    {
        this.shotFromTop = shotFromTop;
        this.shotFromLeft = shotFromLeft;
        this.damage = damage;

        this.Target = new Target(shotFromLeft, shotFromTop);

        this.Location = "Assets/Sprites/Enemy_Sprites/Enemy_Top.png";

        movementSpeed = 700;
    }

    public override Boolean CollitionEffect(GameObject gameObject)
    {
        Targetable targetable = gameObject as Targetable;
        if (targetable is Targetable)
        {
            Enemy enemy = gameObject as Enemy;
            if (enemy is Enemy)
            {
                enemy.AddLifePoints(damage * -1);

                enemy.AddTag("splatter");
                //AddTag("destroyed");
                AddTag("text");
                return true;
            }
            return false;
        } else if (gameObject.HasTag("solid")) {
            AddTag("destroyed");
            return true;
        } else {
            return false;
        }
    }

    public bool SetNewTarget(List<GameObject> gameObjects)
    {
        if (HasTag("homing"))
        {
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
                            Target = new Target(targetable);
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public float GetShotFromTop()
    {
        return shotFromTop;
    }

    public float GetShotFromLeft()
    {
        return shotFromLeft;
    }

    public override Boolean OnTick(List<GameObject> gameObjects, float delta)
    {

        float differenceLeftAbs = Math.Abs(Target.FromLeft() - FromLeft);
        float differenceTopAbs = Math.Abs(Target.FromTop() - FromTop);

        float totalDifferenceAbs = differenceLeftAbs + differenceTopAbs;

        float differenceTopPercent = differenceTopAbs / (totalDifferenceAbs / 100);
        float differenceLeftPercent = differenceLeftAbs / (totalDifferenceAbs / 100);
        //totalDifference = differenceTop + differenceLeft;
        //totalDifferenceAbs = differenceTopAbs + differenceLeftAbs;

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

        if (HasTag("text"))
        {
            AddTag("destroyed");
            gameObjects.Add(new TextBox(50, 50, fromLeft, fromTop - 20, 0, 0, 0, 0, damage.ToString(), 1000));
        }

        return true;
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
        // move sound
    }

    void MovableObject.SetDeathSound(MediaElement deathSound)
    {
        this.deathSound = deathSound;
    }

    void MovableObject.PlayDeathSound()
    {
        // death sound
    }
}
