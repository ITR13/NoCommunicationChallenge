using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    [SerializeField]
    private Button _playButton;

    [SerializeField]
    private Button _deleteSaveButton;

    public AssetReference _gameScene;
    private bool _started;

    private void Start()
    {
        _playButton.onClick.AddListener(Play);
        _deleteSaveButton.interactable = SaveManager.CheckForSave();
        _deleteSaveButton.onClick.AddListener(() =>
            {
                SaveManager.DeleteSave();
                _deleteSaveButton.interactable = false;
            }
        );
    }

    public void Play()
    {
        if (_started) return;
        _started = true;
        
        Loading.SceneToLoad = "Game";
        SceneManager.LoadScene(0);
    }
}
