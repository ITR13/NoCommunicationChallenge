using TMPro;
using UnityEngine;
using UnityEngine.UI;

class UiShop : MonoBehaviour
{
    private const int MaxShopAirjumps = 5;
    public const int AirJumpPrice = 5;
    [SerializeField] private Button _buyAirjumpButton;


    private SavedInt _totalJumpsFromShop, _totalJumps, _money;

    private void OnEnable()
    {
        var saveFile = SaveManager.GetSave();
        _totalJumpsFromShop = saveFile.SavedInts["TotalAirJumpsFromShop"];
        _totalJumps = saveFile.SavedInts["TotalAirJumps"];
        _money = saveFile.SavedInts["Money"];

        _totalJumpsFromShop.OnValueChanged += UpdateBuyAirjump;
        _money.OnValueChanged += UpdateBuyAirjump;
        UpdateBuyAirjump(0);

       //Rename the button to also have the current jump price
        _buyAirjumpButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Buy Airjump ({AirJumpPrice})"; 
        Debug.Log($"Shop opened. Total jumps from shop: {_totalJumpsFromShop.Value}, total jumps: {_totalJumps.Value}, money: {_money.Value}");

    }

    private void OnDisable()
    {
        _totalJumpsFromShop.OnValueChanged -= UpdateBuyAirjump;
        _money.OnValueChanged -= UpdateBuyAirjump;
    }


    private void UpdateBuyAirjump(int _)
    {
        _buyAirjumpButton.interactable = _money.Value >= AirJumpPrice && _totalJumpsFromShop.Value < MaxShopAirjumps;
    }

    public void BuyAirjump()
    {
        if (_money.Value < AirJumpPrice || _totalJumpsFromShop.Value >= MaxShopAirjumps) return;
        _money.SetValue(_money.Value - AirJumpPrice);
        _totalJumpsFromShop.SetValue(_totalJumpsFromShop.Value + 1);
        _totalJumps.SetValue(_totalJumps.Value + 1);
    }
}
