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

    Projectile(float width, float height, float fromLeft, float fromTop, float widthDrawOffset = 0, float heightDrawOffset = 0, float fromLeftDrawOffset = 0, float fromTopDrawOffset = 0)
        : base(width, height, fromLeft, fromTop, widthDrawOffset, heightDrawOffset, fromLeftDrawOffset, fromTopDrawOffset)
    {
        damage = 20;
        shotFromTop = 540;
        shotFromLeft = 960;
    }

    public override Boolean CollitionEffect(GameObject gameObject)
    {
        if (gameObject.HasTag("iets"))
        {
            // PlayHitSound();
        }

        return true;
    }

    public void SetNewTarget()
    {
        
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
