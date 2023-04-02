using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    private SavedInt _remainingJumps;
    private SavedInt _totalJumps;
    private void Awake()
    {
        var saveFile = SaveManager.GetSave();
        _remainingJumps = saveFile.SavedInts["AirJumpsRemaining"];
        _totalJumps = saveFile.SavedInts["TotalAirJumps"];
        _totalJumps.OnValueChanged += TotalAirJumpsChanged;
    }

    private void OnDestroy()
    {
        _totalJumps.OnValueChanged -= TotalAirJumpsChanged;
    }

    public bool ExpendAirJump()
    {
        if (_remainingJumps.Value <= 0) return false;
        _remainingJumps.SetValue(_remainingJumps.Value - 1);
        return true;
    }

    private void TotalAirJumpsChanged(int _)
    {
        RefreshAirJumps();
    }

    public void RefreshAirJumps()
    {
        _remainingJumps.SetValue(_totalJumps.Value);
    }
}
