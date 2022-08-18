using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equippable Item", menuName = ( "Assets/Item/Equippable Item" ))]
public class EquippableItem : Item
{
    public EquipType equipType;
    public float ATKBonus;
    public float DEFBonus;
    public float HPBonus;
    public float CRIBonus;

    public override void ClearData()
    {
        base.ClearData();

        ATKBonus = 0f;
        DEFBonus = 0f;
        HPBonus = 0f;
        CRIBonus = 0f;
    }
}

public enum EquipType
{
    Weapon,
    Head,
    Armor,
    Gloves,
    Shoes,
    Ring,
    Necklace
}
