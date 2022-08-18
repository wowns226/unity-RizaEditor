using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New Item DataBase", menuName = "Assets/DataBase/Item DataBase")]
public class ItemDataBase : ScriptableObject
{
    [SerializeField]
    private List<Item> items;

    public IReadOnlyList<Item> Items => items;

    public Item FindItemBy( string ID ) => items.FirstOrDefault(x => x.itemID == ID);

#if UNITY_EDITOR
    [ContextMenu("Load Equippable Items")]
    private void LoadEquippableItems()
    {
        FindItemsBy<EquippableItem>();
    }

    [ContextMenu("Load Consumable Items")]
    private void LoadConsumableItems()
    {
        FindItemsBy<ConsumableItem>();
    }

    [ContextMenu("Load Other Items")]
    private void LoadOtherItems()
    {
        FindItemsBy<OtherItem>();
    }

    private void FindItemsBy<T>() where T : Item
    {
        items = new List<Item>();

        string[] guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
        foreach(var guid in guids )
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var item = AssetDatabase.LoadAssetAtPath<T>(assetPath);

            if ( item.GetType() == typeof(T) )
                items.Add(item);

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }
#endif
}
