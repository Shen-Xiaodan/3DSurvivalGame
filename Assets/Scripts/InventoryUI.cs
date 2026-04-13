using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public TextMeshProUGUI itemListText;
    public TextMeshProUGUI itemDetailText;
    public Image itemIcon;
    public KeyCode toggleKey = KeyCode.I;
    public KeyCode nextItemKey = KeyCode.DownArrow;
    public KeyCode previousItemKey = KeyCode.UpArrow;

    private bool isOpen;
    private int selectedIndex;
    private bool isSubscribed;

    private void Start()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
        }

        TrySubscribeToInventoryEvents();
        RefreshUI();
    }

    private void OnEnable()
    {
        TrySubscribeToInventoryEvents();
    }

    private void OnDisable()
    {
        if (InventoryManager.Instance != null && isSubscribed)
        {
            InventoryManager.Instance.OnInventoryChanged -= RefreshUI;
            isSubscribed = false;
        }
    }

    private void Update()
    {
        if (!isSubscribed)
        {
            TrySubscribeToInventoryEvents();
        }

        if (Input.GetKeyDown(toggleKey))
        {
            isOpen = !isOpen;

            if (inventoryPanel != null)
            {
                inventoryPanel.SetActive(isOpen);
            }

            if (isOpen)
            {
                RefreshUI();
            }
        }

        if (!isOpen || InventoryManager.Instance == null || InventoryManager.Instance.Slots.Count == 0)
        {
            return;
        }

        if (Input.GetKeyDown(nextItemKey))
        {
            selectedIndex = Mathf.Min(selectedIndex + 1, InventoryManager.Instance.Slots.Count - 1);
            RefreshUI();
        }

        if (Input.GetKeyDown(previousItemKey))
        {
            selectedIndex = Mathf.Max(selectedIndex - 1, 0);
            RefreshUI();
        }
    }

    private void RefreshUI()
    {
        if (itemListText == null)
        {
            return;
        }

        if (InventoryManager.Instance == null)
        {
            itemListText.text = "Inventory manager missing";
            SetDetails(null, 0);
            return;
        }

        var slots = InventoryManager.Instance.Slots;

        if (slots.Count == 0)
        {
            itemListText.text = "Backpack is empty";
            selectedIndex = 0;
            SetDetails(null, 0);
            return;
        }

        selectedIndex = Mathf.Clamp(selectedIndex, 0, slots.Count - 1);

        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < slots.Count; i++)
        {
            InventorySlot slot = slots[i];
            string marker = i == selectedIndex ? "> " : "  ";
            builder.Append(marker)
                .Append(i + 1)
                .Append(". ")
                .Append(slot.itemData.itemName)
                .Append(" x")
                .Append(slot.quantity)
                .Append('\n');
        }

        itemListText.text = builder.ToString();
        SetDetails(slots[selectedIndex].itemData, slots[selectedIndex].quantity);
    }

    private void TrySubscribeToInventoryEvents()
    {
        if (isSubscribed || InventoryManager.Instance == null)
        {
            return;
        }

        InventoryManager.Instance.OnInventoryChanged += RefreshUI;
        isSubscribed = true;
    }

    private void SetDetails(ItemData itemData, int quantity)
    {
        if (itemDetailText != null)
        {
            if (itemData == null)
            {
                itemDetailText.text = "No item selected";
            }
            else
            {
                itemDetailText.text =
                    "Name: " + itemData.itemName + "\n" +
                    "Type: " + itemData.itemType + "\n" +
                    "Quantity: " + quantity + "\n\n" +
                    itemData.description;
            }
        }

        if (itemIcon != null)
        {
            itemIcon.sprite = itemData != null ? itemData.icon : null;
            itemIcon.enabled = itemData != null && itemData.icon != null;
        }
    }
}
