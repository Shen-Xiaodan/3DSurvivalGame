using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI quantityText;
    public Image selectionHighlight;
    public Button button;

    private void Awake()
    {
        EnsureQuantityTextOnTop();
    }

    public void Bind(InventorySlot slot, bool selected, Action onClick)
    {
        EnsureQuantityTextOnTop();

        if (slot == null || slot.itemData == null)
        {
            SetEmpty();
            if (selectionHighlight != null)
            {
                selectionHighlight.enabled = selected;
            }

            if (button != null)
            {
                button.onClick.RemoveAllListeners();
            }

            return;
        }

        if (iconImage != null)
        {
            iconImage.sprite = slot.itemData.icon;
            iconImage.enabled = slot.itemData.icon != null;
        }

        if (quantityText != null)
        {
            quantityText.text = slot.quantity > 1 ? slot.quantity.ToString() : string.Empty;
        }

        if (selectionHighlight != null)
        {
            selectionHighlight.enabled = selected;
        }

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            if (onClick != null)
            {
                button.onClick.AddListener(() => onClick());
            }
        }
    }

    private void SetEmpty()
    {
        EnsureQuantityTextOnTop();

        if (iconImage != null)
        {
            iconImage.sprite = null;
            iconImage.enabled = false;
        }

        if (quantityText != null)
        {
            quantityText.text = string.Empty;
        }

        if (selectionHighlight != null)
        {
            selectionHighlight.enabled = false;
        }

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    private void EnsureQuantityTextOnTop()
    {
        if (quantityText == null)
        {
            return;
        }

        // Keep stack count visible over slot icon regardless of prefab child order.
        quantityText.transform.SetAsLastSibling();
    }
}
