using UnityEngine;

class HSCRing : Effect
{
    public HSCRing(int pointCount) : base(pointCount)
    {

        for (int i = 0; i < lightPointCount; i++)
        {
            insideColors[i] = Color.HSVToRGB((float)i / (float)lightPointCount, 1, 1);
            outsideColors[lightPointCount - 1 - i] = insideColors[i];
        }
    }

    public override LightPointsColor GetNext()
    {

        if (isRunning)
        {
            var inside_color = insideColors[0];
            var outside_color = outsideColors[lightPointCount - 1];

            for (int i = 0; i < lightPointCount - 1; i++)
            {
                insideColors[i] = insideColors[i + 1];
                outsideColors[lightPointCount - 1 - i] = outsideColors[lightPointCount - 2 - i];
            }

            insideColors[lightPointCount - 1] = inside_color;
            outsideColors[0] = outside_color;
        }

        return new LightPointsColor
        {
            isRunning = true,
            lightPointCount = lightPointCount,
            insideColors = insideColors,
            outsideColors = outsideColors,
        };
    }
}