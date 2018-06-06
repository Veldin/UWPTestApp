using System;

public class TextBox : MovableObject
{
    String text;
    int duration;
    float movementSpeed;

    TextBox(String text, int duration)
    {
        this.text = text;
        this.duration = duration;
    }

    public void MovableObject.SetMovementSpeed(float speed)
    {
        movementSpeed = speed;
    }

    public float MovableObject.GetMovementSpeed()
    {
        return movementSpeed;
    }
}
