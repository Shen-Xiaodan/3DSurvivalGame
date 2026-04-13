using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public ItemData itemData;
    [Min(1)] public int pickupAmount = 1;
  
    public string GetItemName()
    {
        if (itemData == null)
        {
            return gameObject.name;
        }

        if (string.IsNullOrWhiteSpace(itemData.itemName))
        {
            return gameObject.name;
        }

        return itemData.itemName;
    }

    public void Interact()
    {
        if (InventoryManager.Instance != null)
        {
            if (itemData != null)
            {
                InventoryManager.Instance.AddItem(itemData, pickupAmount);
            }
            else
            {
                Debug.LogWarning("InteractableObject has no ItemData assigned.");
                return;
            }
        }
        else
        {
            Debug.LogWarning("InventoryManager not found. Item cannot be added to inventory.");
            return;
        }

        Destroy(gameObject);
    }
}