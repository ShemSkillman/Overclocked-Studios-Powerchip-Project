using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryChip : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] 
    private Camera cam;

    [SerializeField]
    private Image image;

    private Vector2 originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        //print("Dragging has Began!");

        originalPosition = image.transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //print("Dragging");

        image.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //print("Dragging has ended!");

        image.transform.position = Input.mousePosition;
    }
}
