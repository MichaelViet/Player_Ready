using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    public bool isDefaultItem = false;
    public GameObject itemPrefab;
    public bool isPickedUp = false;
    public string itemId = Guid.NewGuid().ToString();
    public virtual void Use()
    {
        Debug.Log("Using " + name);
    }

    public void RemoveFromInventory()
    {
        isPickedUp = false;
        Inventory.instance.Remove(this);
    }

}
