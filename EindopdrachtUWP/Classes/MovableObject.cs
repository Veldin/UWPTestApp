using Windows.UI.Xaml.Controls;

namespace UWPTestApp
{
    public interface MovableObject
    {
        float GetMovementSpeed();
        void PlayMoveSound();
        void PlayDeathSound();

        void SetMovementSpeed(float speed);
        void SetMoveSound(MediaElement moveSound);
        void SetDeathSound(MediaElement deathSound);
    }
}
