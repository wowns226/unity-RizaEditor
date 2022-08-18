using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Assets/Item")]
public class Item : ScriptableObject
{
    public string itemID;
    public string itemDescription;
    public GameObject itemPrefab;
    public Sprite itemImage;
    public string itemDisplayName;
    [TextArea(3, 10)]
    public string itemDisplayDescription;
    public ItemType itemType;
    [Range(0, 100)]
    public float itemDropProbability;

    public virtual void ClearData()
    {
        itemID = string.Empty;
        itemDescription = string.Empty;
        itemPrefab = null;
        itemImage = null;
        itemDisplayName = string.Empty;
        itemDisplayDescription = string.Empty;
        itemType = ItemType.None;
        itemDropProbability = 0f;
    }
}
