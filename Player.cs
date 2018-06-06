using System;

public class Player : MovableObject, Targetable
{
    private int walkSpeed;
    private int health;
    private int armor;
    private int level;

    public Player()
    {
        walkSpeed = 20;
        health = 100;
        armor = 0;
        level = 1;
    }

    public void IncreaseHealth(int amount)
    {
        health += amount;
    }

    public void IncreaseArmor(int amount)
    {
        armor += amount;
    }

    public void IncreaseLevel()
    {
        level++;
    }

    public int GetLevel() => level;

    public override Boolean CollitionEffect(GameObject gameObject)
    {
        if (gameObject.HasTag("iets"))
        {
            PlayHitSound();
        }

        return true;
    }

    public float MovableObject.GetMovementSpeed()
    {
        return walkSpeed;
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
