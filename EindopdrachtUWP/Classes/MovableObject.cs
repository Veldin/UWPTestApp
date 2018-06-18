using System;

namespace UWPTestApp
{
    public interface MovableObject
    {
        float GetMovementSpeed();
        
        void SetMovementSpeed(float speed);
    }
}
