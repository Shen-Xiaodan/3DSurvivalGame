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

    [SerializeField, Min(1)] private int maxSlots = 24;

    private readonly List<InventorySlot> slots = new List<InventorySlot>();

    public event Action OnInventoryChanged;

    public IReadOnlyList<InventorySlot> Slots => slots;

    public int MaxSlots => maxSlots;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        EnsureSlotCount();
    }

    private void OnValidate()
    {
        maxSlots = Mathf.Max(1, maxSlots);
    }

    private void EnsureSlotCount()
    {
        while (slots.Count < maxSlots)
        {
            slots.Add(new InventorySlot());
        }

        while (slots.Count > maxSlots)
        {
            slots.RemoveAt(slots.Count - 1);
        }
    }

    public void AddItem(ItemData itemData, int amount = 1)
    {
        if (itemData == null || amount <= 0)
        {
            return;
        }

        EnsureSlotCount();

        int remainingAmount = amount;

        if (itemData.stackable)
        {
            for (int i = 0; i < slots.Count && remainingAmount > 0; i++)
            {
                InventorySlot slot = slots[i];

                if (slot.itemData != itemData || slot.quantity <= 0)
                {
                    continue;
                }

                int stackLimit = itemData.maxStack > 0 ? itemData.maxStack : int.MaxValue;
                int spaceLeft = stackLimit - slot.quantity;

                if (spaceLeft <= 0)
                {
                    continue;
                }

                int addAmount = Mathf.Min(spaceLeft, remainingAmount);
                slot.quantity += addAmount;
                remainingAmount -= addAmount;
            }
        }

        while (remainingAmount > 0)
        {
            InventorySlot emptySlot = slots.Find(slot => slot.itemData == null || slot.quantity <= 0);

            if (emptySlot == null)
            {
                Debug.LogWarning($"Inventory is full. Could not add all of {itemData.itemName}.");
                OnInventoryChanged?.Invoke();
                return;
            }

            int stackLimit = itemData.stackable && itemData.maxStack > 0 ? itemData.maxStack : 1;
            int addAmount = Mathf.Min(stackLimit, remainingAmount);

            emptySlot.itemData = itemData;
            emptySlot.quantity = addAmount;
            remainingAmount -= addAmount;
        }

        OnInventoryChanged?.Invoke();
        Debug.Log($"Added item to inventory: {itemData.itemName} x{amount}");
    }
}
