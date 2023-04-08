using UnityEngine;

public class Collectible : MonoBehaviour
{
    private SavedInt _savedInt;
    private int _byteIndex;

    [SerializeField]
    private string currency;


    public void SetId(SavedInt savedInt, int byteIndex)
    {
        _savedInt = savedInt;
        _byteIndex = byteIndex;

        var active = ((savedInt.Value >> byteIndex) & 1) == 0;
        gameObject.SetActive(active);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.SetActive(false);
        _savedInt.SetValue(_savedInt.Value | (1 << _byteIndex));

        var savedata = SaveManager.GetSave();
        var c = savedata.SavedInts[currency];
        c.SetValue(c.Value + 1);
    }
}
