using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var savedata = SaveManager.GetSave();

        savedata.SavedFloats["CheckpointX"].SetValue(transform.position.x);
        savedata.SavedFloats["CheckpointY"].SetValue(transform.position.y);
    }
}
