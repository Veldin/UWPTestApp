using System;

public class Target
{
    float fromTop;
    float fromLeft;
    Targetable target;

    Target(Targetable target)
    {
        this.target = target;
    }

    Target(float fromTop, float fromLeft)
    {
        this.fromTop = fromTop;
        this.fromLeft = fromLeft;
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public void SetFromLeft(float fromLeft)
    {
        this.fromLeft = fromLeft;
    }

    public void SetFromTop(float fromTop)
    {
        this.fromTop = fromTop;
    }

    public float GetFromTop()
    {
        return fromTop;
    }

    public float GetFromLeft()
    {
        return fromLeft;
    }
}
