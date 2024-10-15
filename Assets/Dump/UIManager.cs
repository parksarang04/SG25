using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    private CenterCameraRaycast playerCtrl;
    public GameObject shopPanel;
    public GameObject cartPanel;
    public bool isPanelOn;

    public Image productBoxImage;
    public TextMeshProUGUI productBoxName;
    public TextMeshProUGUI productBoxCount;
    public GameObject productBoxPanel;

    void Start()
    {
        moneyText.text = GameManager.Instance.playerMoney.ToString();
        playerCtrl = FindObjectOfType<CenterCameraRaycast>();
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

    public void OnProductBoxInfo()
    {
        if (playerCtrl.productBox != null)
        {
            string productName;
            int productCount;
            Sprite productImage;

            // productBox의 정보를 가져오는 함수 호출
            playerCtrl.productBox.GetProductInfo(out productName, out productCount, out productImage);

            // UI 업데이트
            productBoxName.text = productName;
            productBoxCount.text = productCount.ToString();
            productBoxImage.sprite = productImage;
        }
    }

    public void OnProductBoxPanel()
    {
        productBoxPanel.SetActive(true);
    }
    public void CloseProductBoxPanel()
    {
        productBoxPanel.SetActive(false);
    }

    public void OnShopPanel()
    {
        bool isPanelActive = shopPanel.activeInHierarchy;
        isPanelOn = isPanelActive;
        shopPanel.SetActive(!isPanelActive);
    }

    public void ClosePanel()
    {
        cartPanel.SetActive(false);
    }
}
