using System;

public class Projectile : MovableObject
{
    float shotFromTop;
    float shotFromLeft;
    float damage;
    float movementSpeed;
    MediaElement moveSound;
    MediaElement deathSound;

    Projectile()
    {
        damage = 20;
        shotFromTop = 540;
        shotFromLeft = 960;
    }

    public override Boolean CollitionEffect(GameObject gameObject)
    {
        if (gameObject.HasTag("iets"))
        {
            PlayHitSound();
        }

        return true;
    }

    public void SetNewTarget()
    {
        Target target = new Target();
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

    public void MovableObject.SetMovementSpeed(float speed)
    {
        movementSpeed = speed;
    }

    public float MovableObject.GetMovementSpeed()
    {
        return movementSpeed;
    }

    public void MovableObject.SetMoveSound(MediaElement moveSound)
    {
        this.moveSound = moveSound;
    }

    public void MovableObject.PlayMoveSound()
    {
        
    }

    public void MovableObject.SetDeathSound(MediaElement deathSound)
    {
        this.deathSound = deathSound;
    }

    public void MovableObject.PlayDeathSound()
    {
        
    }
}
