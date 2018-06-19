public class Target
{
    private float fromTop;
    private float fromLeft;
    private Targetable target;

    public Target(Targetable target)
    {
        this.target = target;
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

    public float FromTop()
    {
        if (target == null)
        {
            return fromTop;
        }
        return target.FromTop();
    }

    public float FromLeft()
    {
        if (target == null)
        {
            return fromLeft;
        }
        return target.FromLeft();
    }
}
