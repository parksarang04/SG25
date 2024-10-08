using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI changeText;
    public TextMeshProUGUI inputChangeText;
    private CheckoutSystem checkoutSystem;
    private TestPlayerCtrl playerCtrl;
    public GameObject testShopPanel;
    public bool isPanelOn;

    void Start()
    {
        moneyText.text = GameManager.Instance.playerMoney.ToString();
        playerCtrl = FindObjectOfType<TestPlayerCtrl>();
    }

    public void IncreaseMoneyText(int amount)
    {
        GameManager.Instance.playerMoney += amount;
        moneyText.text = GameManager.Instance.playerMoney.ToString(); 
    }
    public void DecreaseMoneyText(int amount)
    {
        GameManager.Instance.playerMoney -= amount;
        moneyText.text = GameManager.Instance.playerMoney.ToString();
    }
    //public void ShowChangeText(int amount)
    //{
    //    changeText.text = amount.ToString();
    //}
    //public void ShowInputChangeText()
    //{
    //    inputChangeText.text = playerCtrl.enteredAmount;
    //}

    public void OnShopPanel()
    {
        bool isPanelActive = testShopPanel.activeInHierarchy;   //하이어라키에서 활성하면 true
        isPanelOn = isPanelActive;
        testShopPanel.SetActive(!isPanelActive);
    }
}
