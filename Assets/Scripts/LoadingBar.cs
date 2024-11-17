using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingBar : MonoBehaviour
{
    public Slider slider; // Arrastra el Slider desde el inspector
    public float smoothSpeed = 0.2f; // Velocidad de suavizado
    public string nextSceneName = "NextScene"; // Nombre de la escena a cargar

    private float targetProgress = 0f;

    void Start()
    {
        slider.value = 0f;
        InvokeRepeating("TestLoad", 0f, 0.5f);
    }

    void Update()
    {
        // Suaviza el movimiento del slider hacia el objetivo
        slider.value = Mathf.Lerp(slider.value, targetProgress, smoothSpeed * Time.deltaTime);

        // Verifica si la barra ha llegado al final
        if (slider.value >= 0.99f) // Usamos 0.99 para compensar la interpolación
        {
            slider.value = 1f; // Aseguramos que llegue exactamente a 1
            LoadNextScene();
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

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }

}
