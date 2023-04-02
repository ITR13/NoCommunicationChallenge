using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiJump : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _jumpIndicators;

    private SavedInt _remainingJumps;
    private SavedInt _totalJumps;

    private void Awake()
    {
        var saveFile = SaveManager.GetSave();
        _remainingJumps = saveFile.SavedInts["AirJumpsRemaining"];
        _totalJumps = saveFile.SavedInts["TotalAirJumps"];

        _remainingJumps.OnValueChanged += UpdateRemainingJumps;
        UpdateRemainingJumps(_remainingJumps.Value);
    }

    private void OnDestroy()
    {
        _remainingJumps.OnValueChanged -= UpdateRemainingJumps;
    }

    private void UpdateRemainingJumps(int jumps)
    {
        for (var i = 0; i < _jumpIndicators.Length; i++)
        {
            _jumpIndicators[i].SetActive(i < jumps);
        }
    }
}
