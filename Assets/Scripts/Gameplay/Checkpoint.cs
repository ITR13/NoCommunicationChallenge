using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Checkpoint LastCheckPoint;
    
    [SerializeField]
    private ToggleFireParticle fireParticle;

    private void Start()
    {
        fireParticle.ToggleFire(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LastCheckPoint == this)
        {
            return;
        }
        
        var savedata = SaveManager.GetSave();

        savedata.SavedFloats["CheckpointX"].SetValue(transform.position.x);
        savedata.SavedFloats["CheckpointY"].SetValue(transform.position.y);

        if (LastCheckPoint != null)
        {
            LastCheckPoint.fireParticle.ToggleFire(false);
        }
        
        fireParticle.ToggleFire(true);
        LastCheckPoint = this;
    }
}
