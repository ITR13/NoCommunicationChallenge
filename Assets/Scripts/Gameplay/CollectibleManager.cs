using UnityEngine;

class CollectibleManager : MonoBehaviour
{
    private void Awake()
    {
        var savedata = SaveManager.GetSave();

        var collectibles = FindObjectsOfType<Collectible>();

        var prevId = -1;
        SavedInt savedInt = null;

        for (var i = 0; i < collectibles.Length; i++)
        {
            var id = i / 30;
            if(prevId != id)
            {
                prevId = id;
                savedInt = savedata.SavedInts[$"CollectibleSet.{id}"];    
            }

            var byteIndex = i % 30;
            collectibles[i].SetId(savedInt, byteIndex);
        }
    }
}