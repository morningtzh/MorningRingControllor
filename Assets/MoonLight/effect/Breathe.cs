using UnityEngine;

class Breathe : Effect
{

    private float[] insideHSV;
    private float[] outsideHSV;

    private float insideV = 1;
    private float outsideV = 1;
    private bool decrease = false;

    public Breathe(int pointCount) : base(pointCount)
    {
        insideHSV = new float[3] {234F / 360F, 1, 1};
        outsideHSV = new float[3] {178F / 360F, 1, 1};

        for (int i = 0; i < lightPointCount; i++)
        {
            insideColors[i] =  Color.HSVToRGB(insideHSV[0], insideHSV[1], insideHSV[2]);
            outsideColors[i] = Color.HSVToRGB(outsideHSV[0], outsideHSV[1], outsideHSV[2]);
        }

        insideV = insideHSV[2];
        outsideV = outsideHSV[2];
    }

    public override LightPointsColor GetNext()
    {

        if (isRunning)
        {
            insideV += (decrease ? -1 : 1) * (insideHSV[2] / 25);
            outsideV += (decrease ? -1 : 1) * (outsideHSV[2] / 25);
            if (insideV < 0) {
                insideV = 0;
                outsideV = 0;
                decrease = !decrease;
            }

            if (insideV > insideHSV[2]) {
                insideV = insideHSV[2];
                outsideV = outsideHSV[2];
                decrease = !decrease;
            }

            var inside_color = Color.HSVToRGB(insideHSV[0], insideHSV[1], insideV);
            var outside_color = Color.HSVToRGB(outsideHSV[0], outsideHSV[1], outsideV);

            for (int i = 0; i < lightPointCount; i++)
            {
                insideColors[i] = inside_color;
                outsideColors[i] = outside_color;
            }

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