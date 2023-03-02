using System;
using System.Reflection;
using System.Linq;


using UnityEngine;

using System.Collections.Generic;

class EffectManager : IEffect
{
    private static Dictionary<string, Type> effects;

    private static Effect currEffect;

    private int lightPointCount;

    public static void AddEffect(Type e) {
        
        if (e.BaseType.Name != typeof(Effect).Name) {
            Debug.Log($"EffectManager Adding Effect failed, {e.Name} basetype is {e.BaseType.Name}");
            return;
        }

        effects.Add(e.Name, e);

        Debug.Log($"EffectManager Added Effect {e.Name} {e.BaseType.Name}, {e.BaseType.FullName}");
    }

    public bool UseEffect(string name) {
        if (!effects.ContainsKey(name)) {
            Debug.LogWarning($"EffectManager don't have effect {name}");
            return false;
        }

        currEffect = (Effect)System.Activator.CreateInstance(effects[name], lightPointCount);

        return true;
    }

    public EffectManager(int pointCount){
        lightPointCount = pointCount;

        effects = new Dictionary<string, Type>();

        AddEffect(typeof(HSCRing));
        AddEffect(typeof(Breathe));

        // 默认效果
        UseEffect("Breathe");
    }

    public LightPointsColor GetNext() {
        if (currEffect == null) {
            return new LightPointsColor{isRunning=false};
        }

        return currEffect.GetNext();
    }

    public string[] GetEffectsInfo() {
        return effects.Keys.ToArray<string>();
    }
}

class EffectRegister
{
    public EffectRegister(Type t) {
        EffectManager.AddEffect(t);
    }
}