using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danger : MonoBehaviour
{
    //If it collides with an object and thet object has InputController attahed, call repsan on it
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"{name} was impacted by " + other.gameObject.name);
        //Look for player InputController
        var player = other.gameObject.GetComponent<InputController>();
        if (player != null)
        {
            //If found, call respawnatcheckpoint
            player.RespawnAtCheckpoint();
        }
        //If not found look in the parent
        else
        {
            player = other.gameObject.GetComponentInParent<InputController>();
            if (player != null)
            {
                player.RespawnAtCheckpoint();
            }
        }
    }

}
