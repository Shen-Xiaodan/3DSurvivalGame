using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private readonly List<string> items = new List<string>();

    public event Action OnInventoryChanged;

    public IReadOnlyList<string> Items => items;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddItem(string itemName)
    {
        if (string.IsNullOrWhiteSpace(itemName))
        {
            return;
        }

        items.Add(itemName);
        OnInventoryChanged?.Invoke();
        Debug.Log($"Added item to inventory: {itemName}");
    }
}
