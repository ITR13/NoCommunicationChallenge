using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playerPrototype;
    void Start()
    {
        var savedata = SaveManager.GetSave();

        savedata.SavedFloats["CheckpointX"].SetValue(transform.position.x);
        savedata.SavedFloats["CheckpointY"].SetValue(transform.position.y);

        Instantiate(playerPrototype, transform.position, Quaternion.identity);

        
    }

}
