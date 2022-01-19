using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    private AssetBundle myLoadedAssetBundle;
    private string[] scenePaths;

    public void LoadScene()     
    {
        SceneManager.LoadScene("Scene1");
    }
}
