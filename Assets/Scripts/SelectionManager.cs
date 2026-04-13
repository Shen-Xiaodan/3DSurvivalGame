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
 
    private void Start()
    {
        //interaction_text = interaction_Info_UI.GetComponent<Text>();
        interaction_text = interaction_Info_UI.GetComponent<TextMeshProUGUI>();

    }
 
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            var selectionTransform = hit.transform;
 
            if (selectionTransform.GetComponent<InteractableObject>())
            {
                interaction_text.text = selectionTransform.GetComponent<InteractableObject>().GetItemName();
                interaction_Info_UI.SetActive(true);
            }
            else 
            { 
                interaction_Info_UI.SetActive(false);
            }
 
        }
          else 
            { 
                interaction_Info_UI.SetActive(false);
            }
    }
}