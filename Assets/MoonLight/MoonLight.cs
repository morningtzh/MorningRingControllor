using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonLight : MonoBehaviour
{
    private GameObject LightPointTemplate = null;

    private static float CYCLE_D = 0.4F;
    private static float CYCLE_R = CYCLE_D / 2;

    private static float SQUARE_D = 0.6F;
    private static int LIGHT_POINT_COUNT = (int)(60 * Mathf.PI * CYCLE_D);

    private static float LIGHT_ANGEL = 2 * Mathf.PI / LIGHT_POINT_COUNT;

    private static float LIGHT_POINT_SIZE = CYCLE_D / (LIGHT_POINT_COUNT / 2);

    private GameObject[] insideLightPoints;
    private GameObject[] outsideLightPoints;

    private EffectManager effectManager;
    public bool isRunning = false;

    void CreateCycle(bool inside)
    {

        var list = inside ? insideLightPoints : outsideLightPoints;

        for (int i = 0; i < LIGHT_POINT_COUNT; i++)
        {
            var r = inside ? CYCLE_R : (CYCLE_R + 0.001F);
            float x = r * Mathf.Sin((i * LIGHT_ANGEL));
            float y = r * Mathf.Cos((i * LIGHT_ANGEL));

            GameObject lightPoint = GameObject.Instantiate(
                LightPointTemplate,
                this.transform
                );

            var prefix = inside ? "Inside" : "Outside";
            lightPoint.name = $"{prefix}_{i}";

            lightPoint.transform.localPosition = new Vector3(x, y, 0);
            lightPoint.transform.localScale = new Vector3(LIGHT_POINT_SIZE, LIGHT_POINT_SIZE, LIGHT_POINT_SIZE);
            lightPoint.transform.LookAt(this.transform);

            if (!inside) {
                lightPoint.transform.Rotate(180, 0,0) ;
            }

            Light light = lightPoint.GetComponent<Light>();
            light.range = 0.4F;
            light.intensity = 1F;

            // 后续灯光颜色从配置读取
            var h = inside ? (float)i / (float)LIGHT_POINT_COUNT : (1- (float)i / (float)LIGHT_POINT_COUNT);
            light.color = Color.HSVToRGB(h, 1, 1);

            lightPoint.SetActive(true);

            //Debug.Log($"{i}: {light.color}");

            list[i] = lightPoint;

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        effectManager = new EffectManager(LIGHT_POINT_COUNT);

        LightPointTemplate = GameObject.Find("LightPointTemplate");
        LightPointTemplate.SetActive(false);

        insideLightPoints = new GameObject[LIGHT_POINT_COUNT];
        outsideLightPoints = new GameObject[LIGHT_POINT_COUNT];

        Debug.Log(LIGHT_POINT_COUNT);

        CreateCycle(true);
        CreateCycle(false);

    }

    // Update is called once per frame
    void Update()
    {
        var colors = effectManager.GetNext();

        for (int i = 0; i < LIGHT_POINT_COUNT; i++) {
            insideLightPoints[i].GetComponent<Light>().color = colors.insideColors[i];
            outsideLightPoints[i].GetComponent<Light>().color = colors.outsideColors[i];
        }
    }
}
