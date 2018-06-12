using System;
using System.Collections.Generic;
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

    public Projectile(float width, float height, float fromLeft, float fromTop, float widthDrawOffset = 0, float heightDrawOffset = 0, float fromLeftDrawOffset = 0, float fromTopDrawOffset = 0, float damage = 0)
        : base(width, height, fromLeft, fromTop, widthDrawOffset, heightDrawOffset, fromLeftDrawOffset, fromTopDrawOffset)
    {
        shotFromTop = fromTop;
        shotFromLeft = fromLeft;
        this.damage = damage;
    }

    public override Boolean CollitionEffect(GameObject gameObject)
    {
        if (gameObject.HasTag("iets"))
        {
            // PlayHitSound();
        }

        return true;
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
        
    }

    void MovableObject.SetDeathSound(MediaElement deathSound)
    {
        this.deathSound = deathSound;
    }

    void MovableObject.PlayDeathSound()
    {
        
    }
}
