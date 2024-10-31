using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewProductData", menuName = "ScriptableObjects/ProductModel")]
public class ProductData : ScriptableObject
{
    public enum PRODUCTTYPE
    {
        Beverages,
        Snacks,
        FrozenFoods,
        DairyProducts,
        PersonalCare,
        Miscellaneous
    }

    public PRODUCTTYPE productType;
    public int ID;
    public string Name;
    public int buyCost;
    public int sellCost;
    public Sprite image;
    public GameObject ProductModel;
}
