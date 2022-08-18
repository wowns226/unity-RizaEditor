using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ScriptableObjectEditor : EditorWindow
{
    Rect headerSection;
    Rect manageItemsSection;

    Color headerSectionColor = new Color(13f / 255f, 32f / 255f, 44f / 255f, 1f);

    Texture2D headerSectionTexture;
    Texture2D manageItemsSectionTexture;

    GUISkin skin;

    ItemType type;

    string[] itemTypeToolbar = new string[4] { ItemType.None.ToString(), ItemType.Equip.ToString(), ItemType.Comsumption.ToString(), ItemType.Other.ToString() };
    int selectIndex = 0;

    static EquippableItem equippableItemData;
    static ConsumableItem consumableItemData;
    static OtherItem otherItemData;

    public static EquippableItem EquippableItemData
    {
        get { return equippableItemData; }
        set { equippableItemData = value; }
    }
    public static ConsumableItem ConsumableItemData => consumableItemData;
    public static OtherItem OtherItemData => otherItemData;

    [MenuItem("#Riza_Editor/ScriptableObject Editor")]
    static void OpenWindow()
    {
        ScriptableObjectEditor window = (ScriptableObjectEditor)GetWindow(typeof(ScriptableObjectEditor));
        window.minSize = new Vector2(600, 300);
        window.maxSize = new Vector2(600, 300);
        window.Show();
    }

    private void OnEnable()
    {
        InitTextures();
        InitData();

        skin = Resources.Load<GUISkin>("GUIStyles/ScriptableObjectEditorDesign");
    }

    private void InitTextures()
    {
        headerSectionTexture = new Texture2D(1, 1);
        headerSectionTexture.SetPixel(0, 0, headerSectionColor);
        headerSectionTexture.Apply();

        manageItemsSectionTexture = Resources.Load<Texture2D>("Icons/editor_item_gradient");
    }

    public static void InitData()
    {
        equippableItemData = (EquippableItem)ScriptableObject.CreateInstance(typeof(EquippableItem));
        consumableItemData = (ConsumableItem)ScriptableObject.CreateInstance(typeof(ConsumableItem));
        otherItemData = (OtherItem)ScriptableObject.CreateInstance(typeof(OtherItem));
    }

    private void OnGUI()
    {
        DrawLayouts();
        DrawHeader();
        DrawManageItemSettings();
    }

    private void DrawLayouts()
    {
        headerSection.x = 0;
        headerSection.y = 0;
        headerSection.width = Screen.width;
        headerSection.height = 50;

        manageItemsSection.x = 0;
        manageItemsSection.y = 50;
        manageItemsSection.width = Screen.width;
        manageItemsSection.height = Screen.height - 50;


        GUI.DrawTexture(headerSection, headerSectionTexture);
        GUI.DrawTexture(manageItemsSection, manageItemsSectionTexture);
    }

    private void DrawHeader()
    {
        GUILayout.BeginArea(headerSection);

        GUILayout.Label("ScriptableObject Manager", skin.GetStyle("Header1"));

        GUILayout.EndArea();
    }

    private void DrawManageItemSettings()
    {
        GUILayout.BeginArea(manageItemsSection);

        GUILayout.Space(10);

        GUILayout.Label("Item Creator",skin.GetStyle("ItemHeader"));

        GUILayout.BeginHorizontal();

        GUILayout.Label("Item Type", skin.GetStyle("ItemField"));
        selectIndex = GUILayout.Toolbar(selectIndex, itemTypeToolbar);
        type = (ItemType)selectIndex;

        GUILayout.EndHorizontal();

        if ( GUILayout.Button("Item Create", GUILayout.Height(30)) )
        {
            switch ( type )
            {
                case ItemType.None:
                    break;
                case ItemType.Equip:
                    ItemEditor.OpenWindow(ItemEditor.ItemSettingType.Equip);
                    break;
                case ItemType.Comsumption:
                    ItemEditor.OpenWindow(ItemEditor.ItemSettingType.Comsumption);
                    break;
                case ItemType.Other:
                    ItemEditor.OpenWindow(ItemEditor.ItemSettingType.Other);
                    break;
                default:
                    break;
            }
        }

        GUILayout.Space(30);

        GUILayout.Label("Item List Manager", skin.GetStyle("ItemHeader"));

        if ( GUILayout.Button("Open Item List", GUILayout.Height(30)) )
        {
            ItemListEditor.OpenWindow();
        }

        GUILayout.EndArea();
    }
}

public class ItemEditor : EditorWindow
{
    string fileName;

    public enum ItemSettingType
    {
        Equip,
        Comsumption,
        Other
    }

    static ItemSettingType itemSettingType;
    static ItemEditor window;

    public static void OpenWindow( ItemSettingType setting )
    {
        itemSettingType = setting;
        window = (ItemEditor)GetWindow(typeof(ItemEditor));
        window.minSize = new Vector2(300, 450);
        window.maxSize = new Vector2(300, 900);
        window.Show();
    }

    private void OnDisable()
    {
        
    }

    private void OnGUI()
    {
        switch ( itemSettingType )
        {
            case ItemSettingType.Equip:
                ScriptableObjectEditor.EquippableItemData.itemType = ItemType.Equip;
                DrawSettings(ScriptableObjectEditor.EquippableItemData);
                break;
            case ItemSettingType.Comsumption:
                ScriptableObjectEditor.ConsumableItemData.itemType = ItemType.Comsumption;
                DrawSettings(ScriptableObjectEditor.ConsumableItemData);
                break;
            case ItemSettingType.Other:
                ScriptableObjectEditor.OtherItemData.itemType = ItemType.Other;
                DrawSettings(ScriptableObjectEditor.OtherItemData);
                break;
            default:
                break;
        }
    }
    
    private void DrawSettings( Item item )
    {
        if ( GUILayout.Button("Clear Data", GUILayout.Height(30)) )
        {
            item.ClearData();
        }

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Item File Name");
        fileName = EditorGUILayout.TextField(fileName);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Item Prefab");
        item.itemPrefab = (GameObject)EditorGUILayout.ObjectField(item.itemPrefab, typeof(GameObject), false);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Item Icon");
        item.itemImage = (Sprite)EditorGUILayout.ObjectField(item.itemImage, typeof(Sprite), false, GUILayout.Width(65), GUILayout.Height(65));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Item ID");
        item.itemID = EditorGUILayout.TextField(item.itemID);
        EditorGUILayout.EndHorizontal();

        GUILayout.Label("Item Description");
        item.itemDescription = EditorGUILayout.TextArea(item.itemDescription, GUILayout.MinHeight(20), GUILayout.MaxHeight(100));

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Item Display Name");
        item.itemDisplayName = EditorGUILayout.TextField(item.itemDisplayName);
        EditorGUILayout.EndHorizontal();

        GUILayout.Label("Item Display Description");
        item.itemDisplayDescription = EditorGUILayout.TextArea(item.itemDisplayDescription, GUILayout.MinHeight(20), GUILayout.MaxHeight(100));

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("% Item Drop Probability");
        item.itemDropProbability = EditorGUILayout.Slider(item.itemDropProbability, 0f, 100f);
        EditorGUILayout.EndHorizontal();

        if ( item.itemType == ItemType.Equip )
        {
            EquippableItem equipItem = (EquippableItem)item;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Item EquipType");
            equipItem.equipType = (EquipType)EditorGUILayout.EnumPopup(equipItem.equipType);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Item ATKBonus");
            equipItem.ATKBonus = EditorGUILayout.FloatField(equipItem.ATKBonus);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Item DEFBonus");
            equipItem.DEFBonus = EditorGUILayout.FloatField(equipItem.DEFBonus);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Item HPBonus");
            equipItem.HPBonus = EditorGUILayout.FloatField(equipItem.HPBonus);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Item CRIBonus");
            equipItem.CRIBonus = EditorGUILayout.FloatField(equipItem.CRIBonus);
            EditorGUILayout.EndHorizontal();
        }
        else if ( item.itemType == ItemType.Comsumption )
        {
            ConsumableItem consumableItem = (ConsumableItem)item;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Item HealHPValue");
            consumableItem.HealHPValue = EditorGUILayout.FloatField(consumableItem.HealHPValue);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Item HealMPValue");
            consumableItem.HealMPValue = EditorGUILayout.FloatField(consumableItem.HealMPValue);
            EditorGUILayout.EndHorizontal();
        }
        else
        {

        }

        if ( fileName == null || fileName.Length < 1 )
        {
            EditorGUILayout.HelpBox("파일 이름을 작성해 주세요.", MessageType.Warning);
        }
        else if ( item.itemPrefab == null )
        {
            EditorGUILayout.HelpBox("아이템 프리팹을 설정하여 주세요.", MessageType.Warning);
        }
        else if ( item.itemImage == null )
        {
            EditorGUILayout.HelpBox("아이템 이미지를 설정하여 주세요.", MessageType.Warning);
        }
        else if ( item.itemID == null || item.itemID.Length < 1 )
        {
            EditorGUILayout.HelpBox("아이템 아이디를 작성해 주세요.", MessageType.Warning);
        }
        else if ( GUILayout.Button("Save", GUILayout.Height(30)) )
        {
            if ( !FileCheck(fileName) )
            {
                SaveItemData();
                item = null;
                window.Close();
            }
        }

        if ( GUILayout.Button("Close", GUILayout.Height(30)) )
        {
            item = null;
            window.Close();
        }
    }

    private bool FileCheck( string _fileName )
    {
        Debug.Log($"{_fileName}을 찾는중...");
        string[] a = AssetDatabase.FindAssets(_fileName);

        if ( a.Length > 0 )
        {
            Debug.LogError($"<color=red><color=green>{_fileName}</color>과 같은 이름의 파일이 존재합니다. 파일명을 수정하세요.</color>");
            return true;
        }

        return false;
    }

    private void SaveItemData()
    {
        string dataPath = "Assets/Resources/ScriptableObject/Data";

        switch ( itemSettingType )
        {
            case ItemSettingType.Equip:
                // asset 파일 만들기
                dataPath += $"/Item/{fileName}.asset";
                AssetDatabase.CreateAsset(ScriptableObjectEditor.EquippableItemData, dataPath);
                AssetDatabase.SaveAssets();

                EditorUtility.FocusProjectWindow();
                Selection.activeObject = ScriptableObjectEditor.EquippableItemData;
                break;

            case ItemSettingType.Comsumption:
                // asset 파일 만들기
                dataPath += $"/Item/{fileName}.asset";
                AssetDatabase.CreateAsset(ScriptableObjectEditor.ConsumableItemData, dataPath);
                AssetDatabase.SaveAssets();

                EditorUtility.FocusProjectWindow();
                Selection.activeObject = ScriptableObjectEditor.ConsumableItemData;
                break;

            case ItemSettingType.Other:
                // asset 파일 만들기
                dataPath += $"/Item/{fileName}.asset";
                AssetDatabase.CreateAsset(ScriptableObjectEditor.OtherItemData, dataPath);           
                AssetDatabase.SaveAssets();

                EditorUtility.FocusProjectWindow();
                Selection.activeObject = ScriptableObjectEditor.OtherItemData;
                break;
        }

        ScriptableObjectEditor.InitData();
    }
}

public class ItemListEditor : EditorWindow
{
    Dictionary<int, Item> itemList;

    static ItemListEditor window;

    int itemIndex;
    int selected = -1;
    string[] itemType = new string[4] { "None", "Equip", "Comsumption", "Other" };

    public static void OpenWindow()
    {
        window = (ItemListEditor)GetWindow(typeof(ItemListEditor));
        //window.minSize = new Vector2(450, 450);
        //window.maxSize = new Vector2(450, 900);
        window.Show();
    }

    private void OnGUI()
    {
        selected = GUILayout.Toolbar(selected, itemType);

        switch ( selected )
        {
            case 0:

                break;
            case 1:
                FindItems<EquippableItem>();

                GUILayout.BeginHorizontal();

                if ( GUILayout.Button("<") )
                {
                    itemIndex--;
                    itemIndex = Mathf.Clamp(itemIndex, 0, itemList.Count);
                }
                else if ( GUILayout.Button(">") )
                {
                    itemIndex++;
                    itemIndex = Mathf.Clamp(itemIndex, 0, itemList.Count);
                }

                GUILayout.EndHorizontal();

                if ( itemIndex >= 0 && itemIndex < itemList.Count )
                {
                    DrawSetting(itemList[itemIndex]);
                }

                break;

            case 2:
                FindItems<ConsumableItem>();

                GUILayout.BeginHorizontal();

                if ( GUILayout.Button("<") )
                {
                    itemIndex--;
                    itemIndex = Mathf.Clamp(itemIndex, 0, itemList.Count);
                }
                else if ( GUILayout.Button(">") )
                {
                    itemIndex++;
                    itemIndex = Mathf.Clamp(itemIndex, 0, itemList.Count);
                }

                GUILayout.EndHorizontal();

                if ( itemIndex >= 0 && itemIndex < itemList.Count )
                {
                    DrawSetting(itemList[itemIndex]);
                }

                break;

            case 3:
                FindItems<OtherItem>();

                GUILayout.BeginHorizontal();

                if ( GUILayout.Button("<") )
                {
                    itemIndex--;
                    itemIndex = Mathf.Clamp(itemIndex, 0, itemList.Count);
                }
                else if ( GUILayout.Button(">") )
                {
                    itemIndex++;
                    itemIndex = Mathf.Clamp(itemIndex, 0, itemList.Count);
                }

                GUILayout.EndHorizontal();

                if ( itemIndex >= 0 && itemIndex < itemList.Count )
                {
                    DrawSetting(itemList[itemIndex]);
                }

                break;

            default:
                break;
        }


        if ( GUILayout.Button("Close", GUILayout.Height(30)) )
        {
            window.Close();
        }
    }

    private void FindItems<T>() where T : Item
    {
        int index = 0;
        itemList = new Dictionary<int, Item>();

        string[] guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
        foreach ( var guid in guids )
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var item = AssetDatabase.LoadAssetAtPath<T>(assetPath);

            if ( item.GetType() == typeof(T) )
            {
                itemList.Add(index, item);
            }

            index++;
        }
    }

    private void DrawSetting(Item item)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Item File");
        item = (Item)EditorGUILayout.ObjectField(item, typeof(Item), false);
        EditorGUILayout.EndHorizontal();
        /*
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Item File Name");
        item.name = EditorGUILayout.TextField(item.name);
        EditorGUILayout.EndHorizontal();
        */
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Item Prefab");
        item.itemPrefab = (GameObject)EditorGUILayout.ObjectField(item.itemPrefab, typeof(GameObject), false);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Item Icon");
        item.itemImage = (Sprite)EditorGUILayout.ObjectField(item.itemImage, typeof(Sprite), false, GUILayout.Width(65), GUILayout.Height(65));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Item ID");
        item.itemID = EditorGUILayout.TextField(item.itemID);
        EditorGUILayout.EndHorizontal();

        GUILayout.Label("Item Description");
        item.itemDescription = EditorGUILayout.TextArea(item.itemDescription, GUILayout.MinHeight(20), GUILayout.MaxHeight(100));

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Item Display Name");
        item.itemDisplayName = EditorGUILayout.TextField(item.itemDisplayName);
        EditorGUILayout.EndHorizontal();

        GUILayout.Label("Item Display Description");
        item.itemDisplayDescription = EditorGUILayout.TextArea(item.itemDisplayDescription, GUILayout.MinHeight(20), GUILayout.MaxHeight(100));

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("% Item Drop Probability");
        item.itemDropProbability = EditorGUILayout.Slider(item.itemDropProbability, 0f, 100f);
        EditorGUILayout.EndHorizontal();

        if ( item.itemType == ItemType.Equip )
        {
            EquippableItem equipItem = (EquippableItem)item;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Item EquipType");
            equipItem.equipType = (EquipType)EditorGUILayout.EnumPopup(equipItem.equipType);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Item ATKBonus");
            equipItem.ATKBonus = EditorGUILayout.FloatField(equipItem.ATKBonus);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Item DEFBonus");
            equipItem.DEFBonus = EditorGUILayout.FloatField(equipItem.DEFBonus);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Item HPBonus");
            equipItem.HPBonus = EditorGUILayout.FloatField(equipItem.HPBonus);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Item CRIBonus");
            equipItem.CRIBonus = EditorGUILayout.FloatField(equipItem.CRIBonus);
            EditorGUILayout.EndHorizontal();
        }
        else if ( item.itemType == ItemType.Comsumption )
        {
            ConsumableItem consumableItem = (ConsumableItem)item;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Item HealHPValue");
            consumableItem.HealHPValue = EditorGUILayout.FloatField(consumableItem.HealHPValue);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Item HealMPValue");
            consumableItem.HealMPValue = EditorGUILayout.FloatField(consumableItem.HealMPValue);
            EditorGUILayout.EndHorizontal();
        }
        else
        {

        }
    }
}