using System;

public class Enemy
{
    int lifePoints;
    int power;
    String type;

    Enemy()
    {
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

    public void SetType(String Type)
    {
        this.type = type;
    }

    public String GetType()
    {
        return type;
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

    public override Boolean CollitionEffect(GameObject gameObject)
    {
        if (gameObject.HasTag("iets"))
        {
            PlayHitSound();
        }

        return true;
    }
}
