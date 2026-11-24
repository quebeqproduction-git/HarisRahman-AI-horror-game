using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour, IInteractable, IPickable
{
    [SerializeField]
    private ItemData _itemData;
    public string Name => _itemData.Name;

    public UnityEvent OnItemPicked;

    // Implementasi IInteractable dengan parameter PlayerCharacter
    public void Interact(PlayerCharacter character)
    {
        Pickup(character);
    }

    // Implementasi IPickable
     public virtual void Pickup(PlayerCharacter character)
    {
        if (_itemData == null || character == null) return;

        ItemData newData = new ItemData(_itemData.ID, _itemData.Name);
        character.Inventory.AddItems(newData);
        OnItemPicked?.Invoke();
        Destroy(gameObject);
    }
}