using System;
using System.Collections.Generic;
using UWPTestApp;

public class Projectile : GameObject, MovableObject
{
    float shotFromTop;
    float shotFromLeft;
    float damage;
    float movementSpeed;

    public string DeathSound { get; set; }
    public string MoveSound { get; set; }

    public Projectile(float width, float height, float fromLeft, float fromTop, float widthDrawOffset = 0, float heightDrawOffset = 0, float fromLeftDrawOffset = 0, float fromTopDrawOffset = 0, float damage = 0, float shotFromLeft = 0, float shotFromTop = 0)
        : base(width, height, fromLeft, fromTop, widthDrawOffset, heightDrawOffset, fromLeftDrawOffset, fromTopDrawOffset)
    {
        this.shotFromTop = shotFromTop;
        this.shotFromLeft = shotFromLeft;
        this.damage = damage;

        Target = new Target(shotFromLeft, shotFromTop);

        Location = "Assets/Sprites/Enemy_Sprites/Enemy_Top.png";

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
                AddTag("text");
                return true;
            }
            return false;
        }
        else if (gameObject.HasTag("solid"))
        {
            AddTag("destroyed");
            return true;
        }
        else
        {
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

    public override Boolean OnTick(List<GameObject> gameObjects, float delta)
    {
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

        if (HasTag("text"))
        {
            AddTag("destroyed");
            gameObjects.Add(new TextBox(50, 50, fromLeft, fromTop - 20, 0, 0, 0, 0, damage.ToString(), 1000));
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
