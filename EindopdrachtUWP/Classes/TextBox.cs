using System;
using System.Collections.Generic;
using System.Diagnostics;
using UWPTestApp;
using Windows.UI;
using Windows.UI.Xaml.Controls;

public class TextBox : GameObject, MovableObject
{
    private String text;
    private float duration;
    private float maxDuration;

    private float movementSpeed;
    private Color color;
    private int fontSize;

    private int animationOffset;

    public TextBox(float width, float height, float fromLeft, float fromTop, float widthDrawOffset = 0, float heightDrawOffset = 0, float fromLeftDrawOffset = 0, float fromTopDrawOffset = 0, string text = "undefined", float duration = 100)
        : base(width, height, fromLeft, fromTop, widthDrawOffset, heightDrawOffset, fromLeftDrawOffset, fromTopDrawOffset)
    {
        this.text = text;
        this.duration = duration;
        this.maxDuration = duration;
        this.movementSpeed = 200;

        this.fontSize = 12;
        this.color = Colors.White;

        this.Target = new Target(fromLeft, fromTop - 1000);
    }

    void MovableObject.SetMovementSpeed(float speed)
    {
        movementSpeed = speed;
    }

    float MovableObject.GetMovementSpeed()
    {
        return movementSpeed;
    }


    public String Text
    {
        get { return text; }
        set { text = value; }
    }

    public float Duration
    {
        get { return duration; }
        set { duration = value; }
    }

    public float MovementSpeed
    {
        get { return movementSpeed; }
        set { movementSpeed = value; }
    }

    public int FontSize
    {
        get { return fontSize; }
        set { fontSize = value; }
    }

    public Color Color
    {
        get { return color; }
        set { color = value; }
    }

    public void PlayMoveSound()
    {
        //throw new NotImplementedException();
    }

    public void PlayDeathsound()
    {
        //throw new NotImplementedException();
    }

    public void SetMoveSound(String moveSound)
    {
        //throw new NotImplementedException();
    }

    public void SetDeathSound(String deathSound)
    {
        //throw new NotImplementedException();
    }

    public void PlayDeathSound()
    {
        //throw new NotImplementedException();
    }

    public override bool OnTick(List<GameObject> gameObjects, float delta)
    {
        duration -= delta;

        float percentage = ((duration - maxDuration) / maxDuration) * 200;

        if (duration < 0)
        {
            AddTag("destroyed");
        }

        Target.AddFromLeft(animationOffset * delta / 1000);

        actMovement(delta);

        Target.AddFromLeft(percentage);

        //throw new NotImplementedException();
        return true;
    }

    private void actMovement(float delta)
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


        //Due to players being able to stand in himself only greater then or smaller then need to be checked.
        if (Target.FromLeft() > FromLeft)
        {
            AddFromLeft((moveLeftDistance * delta) / 10000);
        }
        else if (Target.FromLeft() < FromLeft)
        {
            AddFromLeft(((moveLeftDistance * delta) / 10000) * -1);
        }

        if (Target.FromTop() > FromTop)
        {
            AddFromTop((moveTopDistance * delta) / 10000);
        }
        else if (Target.FromTop() < FromTop)
        {
            AddFromTop(((moveTopDistance * delta) / 10000) * -1);
        }
    }

    public override bool CollitionEffect(GameObject gameObject)
    {
        //throw new NotImplementedException();
        return true;
    }
}
