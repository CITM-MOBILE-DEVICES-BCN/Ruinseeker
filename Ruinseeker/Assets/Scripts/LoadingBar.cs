using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingBar : MonoBehaviour
{
    public Slider slider; // Arrastra el Slider desde el inspector
    public float loadSpeed = 0.5f; // Velocidad de carga
    public string nextSceneName = "NextScene"; // Nombre de la escena a cargar

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

        if (slider.value >= 1f)
        {
            LoadNextScene();
        }

    }

    // Llama a esta funci�n para simular carga
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
