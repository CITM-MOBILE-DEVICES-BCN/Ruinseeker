using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NavigationSystem
{
    public class NavigationManager
    {
        public void ChangeSceneButton(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
        public void ActivateCanvas(GameObject canvas)
        {
            if (canvas != null)
            {
                canvas.SetActive(true);
            }
            else
            {
                //Debug.LogWarning("El Canvas asignado es nulo.");
            }
        }

        public void DeactivateCanvas(GameObject canvas)
        {
            if (canvas != null)
            {
                canvas.SetActive(false);
            }
            else
            {
                //Debug.LogWarning("El Canvas asignado es nulo.");
            }
        }
    }
}
