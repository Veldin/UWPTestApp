using System;
using System.Collections.Generic;
using UWPTestApp;
using Windows.UI;

public class TextBox : GameObject, MovableObject
{
    private float maxDuration;

    public TextBox(float width, float height, float fromLeft, float fromTop, float widthDrawOffset = 0, float heightDrawOffset = 0, float fromLeftDrawOffset = 0, float fromTopDrawOffset = 0, string text = "undefined", float duration = 130)
        : base(width, height, fromLeft, fromTop, widthDrawOffset, heightDrawOffset, fromLeftDrawOffset, fromTopDrawOffset)
    {
        Text = text;
        Duration = duration;
        maxDuration = duration;
        MovementSpeed = 200;

        FontSize = 12;
        Color = Colors.White;

        Target = new Target(fromLeft, fromTop - 1000);
    }

    void MovableObject.SetMovementSpeed(float speed)
    {
        MovementSpeed = speed;
    }

    float MovableObject.GetMovementSpeed()
    {
        return MovementSpeed;
    }

    public string Text { get; set; }

    public float Duration { get; set; }

    public float MovementSpeed { get; set; }

    public int FontSize { get; set; }

    public Color Color { get; set; }

    public override bool OnTick(List<GameObject> gameObjects, float delta)
    {
        Duration -= delta;

        float percentage = ((Duration - maxDuration) / maxDuration) * 200;

        if (Duration < 0)
        {
            AddTag("destroyed");
        }

        Target.AddFromLeft(delta / 1000);

        actMovement(delta);

        Target.AddFromLeft(percentage);

        return true;
    }

    private void actMovement(float delta)
    {
        float differenceLeftAbs = Math.Abs(Target.FromLeft() - FromLeft);
        float differenceTopAbs = Math.Abs(Target.FromTop() - FromTop);

        float totalDifferenceAbs = differenceLeftAbs + differenceTopAbs;

        float differenceTopPercent = differenceTopAbs / (totalDifferenceAbs / 100);
        float differenceLeftPercent = differenceLeftAbs / (totalDifferenceAbs / 100);

        float moveTopDistance = MovementSpeed * (differenceTopPercent / 100);
        float moveLeftDistance = MovementSpeed * (differenceLeftPercent / 100);

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

    public override bool CollisionEffect(GameObject gameObject)
    {
        return true;
    }
}
