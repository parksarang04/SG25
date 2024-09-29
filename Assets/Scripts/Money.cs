using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMoneyData", menuName = "ScriptableObjects/MoneyModel")]
public class Money : ScriptableObject
{
    public int value;
    public GameObject moneyModel;
}
