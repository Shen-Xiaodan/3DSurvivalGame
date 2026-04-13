using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public ItemData itemData;
    public int quantity;
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private readonly List<InventorySlot> slots = new List<InventorySlot>();

    public event Action OnInventoryChanged;

    public IReadOnlyList<InventorySlot> Slots => slots;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddItem(ItemData itemData, int amount = 1)
    {
        if (itemData == null || amount <= 0)
        {
            return;
        }

        InventorySlot existingSlot = slots.Find(slot => slot.itemData == itemData);

        if (existingSlot != null && itemData.stackable)
        {
            existingSlot.quantity += amount;
            if (itemData.maxStack > 0)
            {
                existingSlot.quantity = Mathf.Min(existingSlot.quantity, itemData.maxStack);
            }
        }
        else
        {
            slots.Add(new InventorySlot
            {
                itemData = itemData,
                quantity = amount
            });
        }

        OnInventoryChanged?.Invoke();
        Debug.Log($"Added item to inventory: {itemData.itemName} x{amount}");
    }
}
