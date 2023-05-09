using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool isDrag { private set; get; } = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector2 dragDelta = eventData.delta;

        if (Mathf.Abs(dragDelta.x) <= Mathf.Abs(dragDelta.y) && dragDelta.y < 0)
        {
            UnActivateDrag();
        }
        else
        {
            ActivateDrag();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }

    private void ActivateDrag()
    {
        if (!isDrag)
        {
            isDrag = true;
            transform.localPosition += (Vector3.up * 30);
        }
    }

    private void UnActivateDrag()
    {
        if (isDrag)
        {
            isDrag = false;
            transform.localPosition = Vector3.zero;
        }
    }
}