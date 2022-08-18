using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Item", menuName = ( "Assets/Item/Consumable Item" ))]
public class ConsumableItem : Item
{
    public float HealHPValue;
    public float HealMPValue;

    public override void ClearData()
    {
        base.ClearData();

        HealHPValue = 0f;
        HealMPValue = 0f;
    }
}
