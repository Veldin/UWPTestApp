using System;
using System.Collections.Generic;
using UWPTestApp;
using Windows.UI.Xaml.Controls;

public class Enemy : GameObject, MovableObject, Targetable
{
    int lifePoints;
    int power;
    String enemyType;
    private MediaElement deathSound;
    private float movementSpeed;
    private MediaElement moveSound;

    Enemy(float width, float height, float fromLeft, float fromTop, float widthDrawOffset = 0, float heightDrawOffset = 0, float fromLeftDrawOffset = 0, float fromTopDrawOffset = 0)
        : base(width, height, fromLeft, fromTop, widthDrawOffset, heightDrawOffset, fromLeftDrawOffset, fromTopDrawOffset)
    {
        AddTag("hostile");
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

    public void SetType(String type)
    {
        enemyType = type;
    }

    public String GetEnemyType()
    {
        return enemyType;
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
        //iets 
    }

    public override Boolean CollitionEffect(GameObject gameObject)
    {
        throw new NotImplementedException();
    }

    public override bool OnTick(List<GameObject> gameObjects, float delta)
    {
        throw new NotImplementedException();
    }
}
