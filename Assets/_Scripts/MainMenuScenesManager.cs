using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScenesManager : MonoBehaviour
{
    public void SelectScene()
    {
        switch (this.gameObject.name)
        {
            case "Button":
                SceneManager.LoadScene("Scene1");
                break;
            case "Scene2Button":
                SceneManager.LoadScene("Scene2");
                break;
        }
    }
}
