namespace UWPTestApp
{

    public interface MovableObject
    {
        //Movable objects have a Movement Speed that dictates how fast it moves.
        float GetMovementSpeed();
        void SetMovementSpeed(float speed);
    }
}
