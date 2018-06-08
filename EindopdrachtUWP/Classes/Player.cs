using System;
using System.Collections.Generic;
using UWPTestApp;
using Windows.UI.Xaml.Controls;

public class Player : GameObject, MovableObject, Targetable
{
    private float walkSpeed;
    private int health;
    private int armor;
    private int level;
    private MediaElement deathSound;
    private MediaElement moveSound;

    public Player(float width, float height, float fromLeft, float fromTop, float widthDrawOffset = 0, float heightDrawOffset = 0, float fromLeftDrawOffset = 0, float fromTopDrawOffset = 0)
        : base(width, height, fromLeft, fromTop, widthDrawOffset, heightDrawOffset, fromLeftDrawOffset, fromTopDrawOffset)
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
        if (gameObject.HasTag("hostile"))
        {
            // PlayHitSound();
        }

        return true;
    }

    float MovableObject.GetMovementSpeed()
    {
        return walkSpeed;
    }

    void MovableObject.SetMoveSound(MediaElement moveSound)
    {
        this.moveSound = moveSound;
    }

    void MovableObject.PlayMoveSound()
    {
        // to be implemented
    }

    void MovableObject.SetDeathSound(MediaElement deathSound)
    {
        this.deathSound = deathSound;
    }

    void MovableObject.PlayDeathSound()
    {
        // to be implemented
    }

    void MovableObject.SetMovementSpeed(float speed)
    {
        walkSpeed = speed;
    }

    public override bool OnTick(List<GameObject> gameObjects, float delta)
    {
        throw new NotImplementedException();
    }
}
