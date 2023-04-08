using UnityEngine;

public class UiJump : MonoBehaviour
{
    [SerializeField]
    private GameObject _jumpIndicatorPrefab;

    private SavedInt _remainingJumps;
    private SavedInt _totalJumps;

    private void Awake()
    {
        var saveFile = SaveManager.GetSave();
        _remainingJumps = saveFile.SavedInts["AirJumpsRemaining"];
        _totalJumps = saveFile.SavedInts["TotalAirJumps"];
        _totalJumps.OnValueChanged += UpdateTotalJumps; 
        _remainingJumps.OnValueChanged += UpdateRemainingJumps;
        ProduceJumps();
        UpdateRemainingJumps(_remainingJumps.Value);
    }

    private void UpdateTotalJumps(int obj)
    {
        ProduceJumps();    
    }

    private void ProduceJumps()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        for (var i = 0; i < _totalJumps.Value; i++)
        {
            Instantiate(_jumpIndicatorPrefab, transform);
        }
    }

    private void OnDestroy()
    {
        _remainingJumps.OnValueChanged -= UpdateRemainingJumps;
    }

    private void UpdateRemainingJumps(int jumps)
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i < jumps);
        }
    }
}
