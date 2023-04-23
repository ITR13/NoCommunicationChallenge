using System;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    private SavedInt _remainingJumps;
    private SavedInt _totalJumps;
    private SavedFloat _slideDamage;
    [SerializeField] private Transform m_damageCheck;

    private void Awake()
    {
        var saveFile = SaveManager.GetSave();
        _remainingJumps = saveFile.SavedInts["AirJumpsRemaining"];
        _totalJumps = saveFile.SavedInts["TotalAirJumps"];
        _slideDamage = saveFile.SavedFloats["SlideDamage"];
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

    internal void ApplySlideDamage(Vector2 vel)
    {
        if (_slideDamage.Value <= 0) return;
        float dmg = _slideDamage.Value;
        var ctrl = gameObject.GetComponent<BrackeyCharacterController>();
        var dmgMultiplier = Mathf.Clamp01((vel.magnitude - ctrl.m_SlideSpeedStopThreshold) / 3f) + 1;
        Collider2D[] hits = Physics2D.OverlapCircleAll(m_damageCheck.position, 0.3f);
        Debug.DrawLine(m_damageCheck.position, m_damageCheck.position + new Vector3(0, 0.3f, 0), Color.red);
        foreach (Collider2D hit in hits)
        {
            var dmgReceiver = hit.gameObject.GetComponent<DamageReceiver>();
            if (dmgReceiver != null)
            {
                dmgReceiver.OnDamaged(dmgMultiplier * dmg);
            }
        }
    }
}
