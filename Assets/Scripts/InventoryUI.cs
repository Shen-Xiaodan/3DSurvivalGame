using System.Text;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public TextMeshProUGUI inventoryText;
    public KeyCode toggleKey = KeyCode.I;

    private bool isOpen;

    private void Start()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
        }

        RefreshUI();
    }

    private void OnEnable()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged += RefreshUI;
        }
    }

    private void OnDisable()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged -= RefreshUI;
        }
    }

    private void Update()
    {
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
    }

    private void RefreshUI()
    {
        if (inventoryText == null)
        {
            return;
        }

        if (InventoryManager.Instance == null)
        {
            inventoryText.text = "Inventory manager missing";
            return;
        }

        var items = InventoryManager.Instance.Items;

        if (items.Count == 0)
        {
            inventoryText.text = "Backpack is empty";
            return;
        }

        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < items.Count; i++)
        {
            builder.Append(i + 1).Append(". ").Append(items[i]).Append('\n');
        }

        inventoryText.text = builder.ToString();
    }
}
