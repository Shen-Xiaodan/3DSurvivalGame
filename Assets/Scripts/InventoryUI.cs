using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Transform slotContainer;
    public InventorySlotUI slotPrefab;
    public TextMeshProUGUI emptyText;
    public TextMeshProUGUI itemDetailText;
    public Image itemIcon;
    public KeyCode toggleKey = KeyCode.I;

    [SerializeField] private bool showItemDetails = false;
    [SerializeField, Min(1)] private int fixedColumnCount = 6;

    private bool isOpen;
    private int selectedIndex;
    private bool isSubscribed;
    private bool hasLoggedMissingSlotBindings;
    private readonly List<InventorySlotUI> slotUIs = new List<InventorySlotUI>();

    private void Start()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
        }

        ConfigureSlotContainer();
        SetDetailUIVisible(showItemDetails && false);

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
                ConfigureSlotContainer();
                RefreshUI();
                SetDetailUIVisible(showItemDetails);
            }
            else
            {
                SetDetailUIVisible(false);
            }
        }
    }

    private void RefreshUI()
    {
        if (InventoryManager.Instance == null)
        {
            if (emptyText != null)
            {
                emptyText.text = "Inventory manager missing";
                emptyText.gameObject.SetActive(true);
            }
            ClearSlots();
            SetDetails(null, 0);
            return;
        }

        var slots = InventoryManager.Instance.Slots;

        if (slotContainer == null || slotPrefab == null)
        {
            if (emptyText != null)
            {
                emptyText.text = "Slot UI not configured: assign Slot Container and Slot Prefab";
                emptyText.gameObject.SetActive(true);
            }

            ClearSlots();
            SetDetails(null, 0);

            if (!hasLoggedMissingSlotBindings)
            {
                Debug.LogWarning("InventoryUI is missing Slot Container or Slot Prefab reference.");
                hasLoggedMissingSlotBindings = true;
            }

            return;
        }

        hasLoggedMissingSlotBindings = false;

        if (emptyText != null)
        {
            bool hasAnyItem = false;
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i] != null && slots[i].itemData != null && slots[i].quantity > 0)
                {
                    hasAnyItem = true;
                    break;
                }
            }

            // emptyText.text = hasAnyItem ? string.Empty : "Backpack is empty";
            emptyText.gameObject.SetActive(!hasAnyItem);
        }

        selectedIndex = Mathf.Clamp(selectedIndex, 0, slots.Count - 1);
        RebuildSlots(slots);

        if (showItemDetails)
        {
            InventorySlot selectedSlot = slots[selectedIndex];
            if (selectedSlot != null && selectedSlot.itemData != null && selectedSlot.quantity > 0)
            {
                SetDetails(selectedSlot.itemData, selectedSlot.quantity);
            }
            else
            {
                SetDetails(null, 0);
            }
        }
        else
        {
            SetDetails(null, 0);
        }
    }

    private void RebuildSlots(IReadOnlyList<InventorySlot> slots)
    {
        ClearSlots();

        for (int i = 0; i < slots.Count; i++)
        {
            InventorySlot slot = slots[i];
            InventorySlotUI slotUI = Instantiate(slotPrefab, slotContainer);
            int index = i;
            slotUI.Bind(slot, i == selectedIndex, () =>
            {
                selectedIndex = index;
                RefreshUI();
            });
            slotUIs.Add(slotUI);
        }
    }

    private void ClearSlots()
    {
        for (int i = 0; i < slotUIs.Count; i++)
        {
            if (slotUIs[i] != null)
            {
                Destroy(slotUIs[i].gameObject);
            }
        }

        slotUIs.Clear();
    }

    private void ConfigureSlotContainer()
    {
        if (slotContainer == null)
        {
            return;
        }

        RectTransform rectTransform = slotContainer as RectTransform;
        if (rectTransform != null)
        {
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.anchoredPosition = Vector2.zero;
        }

        GridLayoutGroup gridLayoutGroup = slotContainer.GetComponent<GridLayoutGroup>();
        if (gridLayoutGroup == null)
        {
            gridLayoutGroup = slotContainer.gameObject.AddComponent<GridLayoutGroup>();
        }

        gridLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = Mathf.Max(1, fixedColumnCount);

        if (gridLayoutGroup.cellSize == Vector2.zero)
        {
            gridLayoutGroup.cellSize = new Vector2(80f, 80f);
        }

        if (gridLayoutGroup.spacing == Vector2.zero)
        {
            gridLayoutGroup.spacing = new Vector2(8f, 8f);
        }
    }

    private void SetDetailUIVisible(bool visible)
    {
        if (itemDetailText != null)
        {
            itemDetailText.gameObject.SetActive(visible);
        }

        if (itemIcon != null)
        {
            itemIcon.gameObject.SetActive(visible);
        }
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
