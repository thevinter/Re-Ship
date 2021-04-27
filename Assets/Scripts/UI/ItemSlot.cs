using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ItemSlot : MonoBehaviour, IDropHandler {
    public GameObject active = null;
    public Unit activeUnit;
    DragAndDrop draggedUnit;
    TextMeshProUGUI name_text;
    public bool debug = false;
    public void OnDrop(PointerEventData eventData) {
        draggedUnit = eventData.pointerDrag.GetComponent<DragAndDrop>();
        if (eventData.pointerDrag != null && activeUnit == null) {
            draggedUnit.SetParentToNull();
            draggedUnit.BlockRaycasts(true);
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            print("Changing active unit!");
            activeUnit = eventData.pointerDrag.GetComponent<Unit>();
            draggedUnit.SetActive(true);
            draggedUnit.SetParent(this);
        }  
    }
    void Update() {
        /*if (activeUnit == null) {
            name_text.text = "No Unit";
        }
        else name_text.text = activeUnit.unit_name;*/
        if(debug) print("Active unit"+ activeUnit);

    }
    public void Awake() {
        
        if(active == null) {
            activeUnit = null;
            draggedUnit= null;
        }
        else {
            activeUnit = active.GetComponent<Unit>();
            draggedUnit = active.GetComponent<DragAndDrop>();
            draggedUnit.SetParent(this);
        }
        
        name_text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ResetUnit() {
        draggedUnit = null;
        activeUnit = null;
    }
}
