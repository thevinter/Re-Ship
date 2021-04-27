using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SideMenu : MonoBehaviour, IDragHandler {
    public string window_name;
    private RectTransform rec;

    [SerializeField] private Canvas canvas;

    private void Awake() {
        rec = GetComponent<RectTransform>();
    }

    public void Activate(bool active) {
        gameObject.SetActive(active);
    }

    public void OnDrag(PointerEventData eventData) {
        rec.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
}
