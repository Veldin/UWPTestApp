using System;
using UWPTestApp;

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

    public void SetTarget(Targetable target)
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
        if (target == null)
        {
            return fromTop;
        }
        return 0;
    }

    public float GetFromLeft()
    {
        if (target == null)
        {
            return fromLeft;
        }
        return 0;
    }

    public Targetable GetTarget()
    {
        if (target != null)
        {
            return target;
        }
        return null;
    }
}
