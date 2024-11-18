using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void ChangeSceneButton(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Activar un Canvas u otro GameObject
    public void ActivateCanvas(GameObject canvas)
    {
        if (canvas != null)
        {
            canvas.SetActive(true); // Activa el objeto
        }
        else
        {
            //Debug.LogWarning("El Canvas asignado es nulo.");
        }
    }

    // Desactivar un Canvas u otro GameObject
    public void DeactivateCanvas(GameObject canvas)
    {
        if (canvas != null)
        {
            canvas.SetActive(false); // Desactiva el objeto
        }
        else
        {
            //Debug.LogWarning("El Canvas asignado es nulo.");
        }
    }
}
