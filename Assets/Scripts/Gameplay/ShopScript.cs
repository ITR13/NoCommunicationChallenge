using UnityEngine;

public class ShopScript : MonoBehaviour
{
    [SerializeField]
    private GameObject shopOverlay;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        shopOverlay.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        shopOverlay.SetActive(false);
    }
}
