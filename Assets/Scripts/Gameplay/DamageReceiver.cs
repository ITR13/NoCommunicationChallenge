using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    [SerializeField] private float Health = 1f;
    
    internal void OnDamaged(float damage)
    {
        Health -= Mathf.Min(1, damage);
        if (Health < 0f) OnDeath();
    }

    private void OnDeath()
    {
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
