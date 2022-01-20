using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScenesManager : MonoBehaviour
{
    // KAzdy przycisk wczutuje inna scene. Nie potrafie pobrac danych o scenach dynamicznie wiec sa podane na stale
    // Przy zmianie nazwy sceny trzeba tutaj rowniez zmienic jej nazwe
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
