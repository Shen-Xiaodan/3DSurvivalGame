using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string ItemName;
  
    public string GetItemName()
    {
        return ItemName;
    }

    public void Interact()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem(ItemName);
        }
        else
        {
            Debug.LogWarning("InventoryManager not found. Item cannot be added to inventory.");
        }

        Destroy(gameObject);
    }
}