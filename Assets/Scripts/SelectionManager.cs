using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
 
public class SelectionManager : MonoBehaviour
{
 
    public GameObject interaction_Info_UI;
    public float maxDistance = 3f;
    TextMeshProUGUI interaction_text;
    private InteractableObject currentInteractable;
 
    private void Start()
    {
        if (interaction_Info_UI != null)
        {
            interaction_text = interaction_Info_UI.GetComponent<TextMeshProUGUI>();

            if (interaction_text == null)
            {
                interaction_text = interaction_Info_UI.GetComponentInChildren<TextMeshProUGUI>();
            }
        }

    }
 
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            var selectionTransform = hit.transform;

            if (selectionTransform.TryGetComponent<InteractableObject>(out var interactable) ||
                selectionTransform.GetComponentInParent<InteractableObject>() != null)
            {
                if (interactable == null)
                {
                    interactable = selectionTransform.GetComponentInParent<InteractableObject>();
                }

                currentInteractable = interactable;
                if (interaction_text != null)
                {
                    interaction_text.text = currentInteractable.GetItemName();
                }
                interaction_Info_UI.SetActive(true);

                if (Input.GetMouseButtonDown(0))
                {
                    currentInteractable.Interact();
                    currentInteractable = null;
                    interaction_Info_UI.SetActive(false);
                }
            }
            else 
            { 
                currentInteractable = null;
                interaction_Info_UI.SetActive(false);
            }
 
        }
          else 
            { 
                currentInteractable = null;
                interaction_Info_UI.SetActive(false);
            }
    }
}