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

    void OnEnable()  // Awake ��� OnEnable ���
    {
        InitializeProduct();
    }

    private void InitializeProduct()
    {
        // ������ �̸��� ���� ������ �ʱ�ȭ�ϴ� �ڵ�
        if (Name == "�ݶ�")
        {
            productType = PRODUCTTYPE.Beverages;
            Index = 1;
            buyCost = 500;
            sellCost = 1000;
        }
        else if (Name == "����Ĩ")
        {
            productType = PRODUCTTYPE.Snacks;
            Index = 2;
            buyCost = 700;
            sellCost = 1500;
        }
        else if (Name == "����")
        {
            productType = PRODUCTTYPE.DairyProducts;
            Index = 3;
            buyCost = 1000;
            sellCost = 2000;
        }
    }
}
