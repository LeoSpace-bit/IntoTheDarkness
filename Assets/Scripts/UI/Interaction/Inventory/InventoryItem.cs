using UnityEngine.UI;

[System.Serializable]
public class InventoryItem
{
    public int UID;
    public int Amount;

    [System.NonSerialized] public string Name;
    [System.NonSerialized] public string Description;
    [System.NonSerialized] public int MaximumAmount;
    [System.NonSerialized] public UIInventoryItem UIItem;

}

