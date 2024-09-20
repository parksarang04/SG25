using UnityEngine;

[CreateAssetMenu(fileName = "NewProductData", menuName = "ScriptableObjects/ProductModel")]
public class ProductData : ScriptableObject
{
    public enum PRODUCTTYPE
    {
        Beverages,
        Snacks,
        DairyProducts,
        FrozenFoods,
        PersonalCare,
        Miscellaneous
    }

    public PRODUCTTYPE productType;
    public int Index;
    public string Name;
    public int buyCost;
    public int sellCost;
    public Texture2D Image;
    public GameObject ProductModel;

    void OnEnable()  // Awake 대신 OnEnable 사용
    {
        InitializeProduct();
    }

    private void InitializeProduct()
    {
        // 아이템 이름에 따라 정보를 초기화하는 코드
        if (Name == "콜라")
        {
            productType = PRODUCTTYPE.Beverages;
            Index = 1;
            buyCost = 500;
            sellCost = 1000;
        }
        else if (Name == "감자칩")
        {
            productType = PRODUCTTYPE.Snacks;
            Index = 2;
            buyCost = 700;
            sellCost = 1500;
        }
        else if (Name == "우유")
        {
            productType = PRODUCTTYPE.DairyProducts;
            Index = 3;
            buyCost = 1000;
            sellCost = 2000;
        }
    }
}
