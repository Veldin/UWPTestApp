using Windows.UI.Xaml.Controls;

namespace UWPTestApp
{
    public interface MovableObject
    {
        float GetMovementSpeed();
        
        void SetMovementSpeed(float speed);

    }
}
