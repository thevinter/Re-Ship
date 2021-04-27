using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler {

    private RectTransform rec;
    [SerializeField] private Canvas canvas;
    private CanvasGroup canvasGroup;
    public GameEvent select;
    private bool isActive = false;
    private ItemSlot parent;

    public void SetActive(bool isActive) {
        this.isActive = isActive;
    }

    public void SetParent(ItemSlot p) {
        parent = p;
    }

    private void Awake() {
        rec = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        isActive = false;
        var temp = GameObject.FindGameObjectWithTag("Selected");
        if (temp != null) temp.tag = "Untagged";
        eventData.pointerDrag.tag = "Selected";
        select.Raise();
            canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData) {
        rec.anchoredPosition += eventData.delta / canvas.scaleFactor;

    }

    public void OnEndDrag(PointerEventData eventData) {
        //print("StopDragging");
        if(!isActive && parent != null) {
            rec.anchoredPosition = parent.GetComponent<RectTransform>().anchoredPosition;
            isActive = true;
            BlockRaycasts(true);
        }
    }

    public void SetParentToNull() {
        if(parent!=null) parent.ResetUnit();
    }

    public void BlockRaycasts(bool value) {
        canvasGroup.blocksRaycasts = value;
    }

    public void OnPointerDown(PointerEventData eventData) {
        var temp = GameObject.FindGameObjectWithTag("Selected");
        if (temp != null) temp.tag = "Untagged";
        this.gameObject.tag = "Selected";
        select.Raise();
    }   
}
