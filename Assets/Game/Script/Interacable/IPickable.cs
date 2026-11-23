public interface IPickable
{
    public void Pickup();
}

    [System.Serializable]
public class ItemData
{
    // Membuat variable untuk menyimpan id item
    public string ID;
    // Membuat variable untuk menyimpan id item
    public string Name;
 
    // Membuat constructor class ItemData untuk mengisi data id dan nama
    public ItemData(string id, string name)
    {
        ID = id;
        Name = name;
    }
}