[System.Serializable]
public class ItemState
{
    public string itemId;
    public bool isPickedUp;

    public ItemState(string itemId, bool isPickedUp)
    {
        this.itemId = itemId;
        this.isPickedUp = isPickedUp;
    }
}

