using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public Button oncartBtn;        //왼쪽 상단 장바구니 버튼 변수
    public GameObject productPrefab;//상품창 프리팹 변수
    public GameObject shopPanel;    //상점 패널
    public GameObject productContent;

    public Button cartBtn;          //물품 장바구니 버튼 변수
    public int money;               //게임 머니 변수
    public TMP_Dropdown dropdown;   //물건 담는 드롭다운 버튼 변수
    public Texture2D image;         //물건 이미지 변수 
    public string productName;      //상품 물건 이름 변수
    public int price;               //상품 물건 가격 변수
    public ProductData[] products;

    
    void Start()
    {
        Generateproduct();
    }

    public void Generateproduct()
    {
        for(int i = 0; i < products.Length; i++)    //상품의 갯수만큼 상품창이 생긴다.
        {
            GameObject productObj = Instantiate(productPrefab, productContent.transform);     //Instantiate 복제라는 뜻
            //productObj.transform.SetParent(productContent.transform);

            
        }


    }


}
