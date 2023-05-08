using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragController : MonoBehaviour, IDragHandler
{
    public bool isDrag { private set; get; } = false;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 dragDelta = eventData.delta;

        if (Mathf.Abs(dragDelta.x) > Mathf.Abs(dragDelta.y))
        {
            if (dragDelta.x > 0)
            {
                Debug.Log("Dragged right");
                ActivateDrag();
            }
            else if (dragDelta.x < 0)
            {
                Debug.Log("Dragged left");
                ActivateDrag();
            }
        }
        else
        {
            if (dragDelta.y > 0)
            {
                Debug.Log("Dragged up");
                ActivateDrag();
            }
            else if (dragDelta.y < 0)
            {
                Debug.Log("Dragged down");
                UnActivateDrag();
            }
        }
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