using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    private AssetBundle myLoadedAssetBundle;
    private string[] scenePaths;

    // Start is called before the first frame update
    public void LoadScene()     
    {
        //myLoadedAssetBundle = AssetBundle.LoadFromFile("Assets/AssetBundles/scenes");
       // scenePaths = myLoadedAssetBundle.GetAllScenePaths();
        SceneManager.LoadScene("Scene1");
    }
  //  void Start()
  //  {
      //  myLoadedAssetBundle = AssetBundle.LoadFromFile("Assets/AssetBundles/scenes");
      //  scenePaths = myLoadedAssetBundle.GetAllScenePaths();
     //   SceneManager.LoadScene("Scene1", LoadSceneMode.Additive);
  //  }
 
    // Update is called once per frame
  //  void Update()
  //  {
        
   // }
}
