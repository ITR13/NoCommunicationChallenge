using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class GameScript : MonoBehaviour
{
    [SerializeField]
    private AssetReference _menuScene;

    public void BackToMainMenu()
    {
        Loading.SceneToLoad = _menuScene;
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMainMenu();
        }
    }
}
