using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using NavigationSystem;

public class GameManager : MonoBehaviour
{
    private NavigationManager navManager;

    private void Start()
    {
        navManager = new NavigationManager();
    }
    public void ChangeSceneButton(string sceneName)
    {
        navManager.ChangeSceneButton(sceneName);
    }
    public void ActivateCanvas(GameObject canvas)
    {
        navManager.ActivateCanvas(canvas);
    }

    public void DeactivateCanvas(GameObject canvas)
    {
        navManager.DeactivateCanvas(canvas);
    }
}
