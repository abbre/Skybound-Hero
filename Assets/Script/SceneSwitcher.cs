using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneSwitcher : MonoBehaviour
{

    public void SwitchScene(string SceneToChange)
    {
        
        SceneManager.LoadScene(SceneToChange);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
