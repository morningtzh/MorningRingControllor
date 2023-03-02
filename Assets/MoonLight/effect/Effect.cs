using UnityEngine;

interface IEffect {
    LightPointsColor GetNext();
}

struct LightPointsColor {
    public bool isRunning;
    public int lightPointCount;
    public Color[] insideColors;
    public Color[] outsideColors;
}

class Effect : IEffect
{
    protected int lightPointCount;
    public Color[] insideColors;
    public Color[] outsideColors;

    protected bool isRunning = true;

    public bool running {
        get => isRunning;
        set => isRunning = value;
    }

    void GetConfig() {
        
    }

    public Effect(int pointCount) {
        lightPointCount = pointCount;

        insideColors = new Color[pointCount];
        outsideColors = new Color[pointCount];
    }

    public virtual LightPointsColor GetNext() {
        return new LightPointsColor{isRunning=false};
    }
}