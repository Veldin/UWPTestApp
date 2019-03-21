using UWPTestApp;

public class Target
{
    /* 
        * Target is used to store an 2d location in the game.
        * Can also store a Targetable object.
        * 
        * If a Targetable is set the fromTop() and fomLeft() methods return the Targetable's fromTop and fromLeft instead of its own.
    */

    private float fromTop;
    private float fromLeft;
    private Targetable target;

    public Target(Targetable target)
    {
        this.target = target;
        this.fromTop = 0;
        this.fromLeft = 0;
    }

    public Target(float fromLeft, float fromTop)
    {
        this.fromTop = fromTop;
        this.fromLeft = fromLeft;
    }

    public void SetTarget(Targetable target)
    {
        this.target = target;
    }

    public void SetTarget(float fromLeft, float fromTop)
    {
        this.fromTop = fromTop;
        this.fromLeft = fromLeft;
    }

    public void SetFromLeft(float fromLeft)
    {
        this.fromLeft = fromLeft;
    }

    public void SetFromTop(float fromTop)
    {
        this.fromTop = fromTop;
    }

    public void AddFromLeft(float value)
    {
        this.fromLeft += value;
    }

    public void AddFromTop(float value)
    {
        this.fromTop += value;
    }

    /* FromTop() */
    /* 
        * If there is no target set return the fromTop field.
        * If there is, return its FromTop instead.
    */
    public float FromTop()
    {
        if (target == null)
        {
            return fromTop;
        }
        return target.FromTop();
    }

    /* FromLeft() */
    /* 
        * If there is no target set return the fromLeft field.
        * If there is, return its fromLeft instead.
    */
    public float FromLeft()
    {
        if (target == null)
        {
            return fromLeft;
        }
        return target.FromLeft();
    }
}
