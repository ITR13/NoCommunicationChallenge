using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField] private AssetReference menuSceneReference;
    [SerializeField] private Image loadingBar;

    //public static AssetReference SceneToLoad;
    public static string SceneToLoad;

    private void Awake()
    {
        loadingBar.fillAmount = 0;
    }

    private IEnumerator Start()
    {
        yield return null;
        //var menuOp = SceneToLoad.LoadSceneAsync(LoadSceneMode.Single, true);

        AsyncOperation menuOp = SceneManager.LoadSceneAsync(SceneToLoad);
        while (!menuOp.isDone)
        {
            loadingBar.fillAmount = (menuOp.progress) / 2;
            yield return null;
        }
        loadingBar.fillAmount = 1;
    }


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void FirstSceneToLoad()
    {
        var loading = FindObjectOfType<Loading>();
        if(loading == null) return;
        SceneToLoad = "MainMenu";
    }
}
