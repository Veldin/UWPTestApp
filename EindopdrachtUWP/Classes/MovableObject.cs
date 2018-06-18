using System;

namespace UWPTestApp
{
    public interface MovableObject
    {
        float GetMovementSpeed();
        void PlayMoveSound();
        void PlayDeathSound();

        void SetMovementSpeed(float speed);
        void SetMoveSound(String moveSound);
        void SetDeathSound(String deathSound);
    }
}
