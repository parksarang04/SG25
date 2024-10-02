using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI changeText;
    private CheckoutSystem checkoutSystem;
    private GameManager gameManager;
    public GameObject testShopPanel;
    public bool isPanelOn;

    public void IncreaseMoneyText(int amount)
    {
        gameManager.playerMoney += amount;
        moneyText.text = gameManager.playerMoney.ToString(); 
    }

    public void OnShopPanel()
    {
        bool isPanelActive = testShopPanel.activeInHierarchy;
        isPanelOn = isPanelActive;
        testShopPanel.SetActive(!isPanelActive);
    }
}
