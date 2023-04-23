using System.Collections.Generic;
using UnityEngine;

public enum PurchaseableAbility
{
    AirJump,
    SlideDamageUp
}

public class ShopScript : MonoBehaviour
{
    [SerializeField]
    private GameObject shopOverlay;
    [SerializeField] List<PurchaseableAbility> purchaseables = new List<PurchaseableAbility>();


    private void OnTriggerEnter2D(Collider2D collision)
    {
        var ctrl = collision.GetComponent<BrackeyCharacterController>();
        if (ctrl != null) return;
        shopOverlay.SetActive(true);
        if (purchaseables.Count > 0)
        {
            for (int i = shopOverlay.transform.childCount - 1; i >= 0; i--)
            {
                shopOverlay.transform.GetChild(i).gameObject.SetActive(false);
            }
            foreach (var purchaseable in purchaseables) 
            {
                shopOverlay.transform.Find(purchaseable.ToString()).gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var ctrl = collision.GetComponent<BrackeyCharacterController>();
        if (ctrl != null) return;
        shopOverlay.SetActive(false);
    }
}
