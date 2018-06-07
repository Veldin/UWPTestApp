using System;

public interface MovableObject : GameObject
{
    float GetMovementSpeed();
    void PlayMoveSound();
    void PlayDeathsound();

    void SetMovementSpeed(float speed);
    void SetMoveSound(MediaElement moveSound);
    void SetDeathSound(MediaElement deathSound);
}
