using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    RectTransform rt;

    Vector2 origin;
    float conversionRate;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        origin = rt.localPosition;
        conversionRate = rt.sizeDelta.x / 20f;
    }

    // void Update()
    // {
    //     Debug.Log(rt.sizeDelta);
    // }

    public void SetHealth(float health)
    {
        slider.value = health;
    }

    public void SetMaxHealth(float health, float currentHealth)
    {
        slider.maxValue = health;
        rt.sizeDelta = new Vector2(conversionRate * health, rt.sizeDelta.y);
        rt.localPosition = new Vector2((conversionRate * (health - 20)) / 2 + origin.x, rt.localPosition.y);
        slider.value = currentHealth;
    }
}
