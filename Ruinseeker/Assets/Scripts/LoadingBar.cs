using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    public Slider slider; // Arrastra el Slider desde el inspector
    public float loadSpeed = 0.5f; // Velocidad de carga

    private float targetProgress = 0f;

    void Start()
    {
        slider.value = 0f;
        InvokeRepeating("TestLoad", 0f, 0.5f);
    }

    void Update()
    {
        if (slider.value < targetProgress)
        {
            slider.value += loadSpeed * Time.deltaTime;
        }
    }

    // Llama a esta función para simular carga
    public void IncreaseProgress(float newProgress)
    {
        targetProgress = Mathf.Clamp(newProgress, 0f, 1f);
    }

    void TestLoad()
    {
        IncreaseProgress(slider.value + 0.1f);
    }
}
