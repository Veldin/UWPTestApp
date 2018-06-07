using System;
using UWPTestApp;
using Windows.UI.Xaml.Controls;

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

    void MovableObject.SetMovementSpeed(float speed)
    {
        movementSpeed = speed;
    }

    float MovableObject.GetMovementSpeed()
    {
        return movementSpeed;
    }

    public void PlayMoveSound()
    {
        throw new NotImplementedException();
    }

    public void PlayDeathsound()
    {
        throw new NotImplementedException();
    }

    public void SetMoveSound(MediaElement moveSound)
    {
        throw new NotImplementedException();
    }

    public void SetDeathSound(MediaElement deathSound)
    {
        throw new NotImplementedException();
    }

    public void PlayDeathSound()
    {
        throw new NotImplementedException();
    }
}
