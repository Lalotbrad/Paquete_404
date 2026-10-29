using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class ItemDragHandie : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform originalParent;
    CanvasGroup canvasGroup;
    RectTransform rectTransform;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = .6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        Slot originalSlot = originalParent.GetComponent<Slot>();
        Slot dropSlot = null;

        if (eventData.pointerEnter != null)
        {
            // Puede ser el slot o el ítem que está dentro de él
            dropSlot = eventData.pointerEnter.GetComponentInParent<Slot>();
        }

        if (dropSlot != null && dropSlot != originalSlot)
        {
            if (dropSlot.currentItem != null)
            {
                // Intercambio: el ítem del destino se va al slot de origen
                GameObject otherItem = dropSlot.currentItem;
                otherItem.transform.SetParent(originalSlot.transform, false);
                otherItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                originalSlot.currentItem = otherItem;
            }
            else
            {
                originalSlot.currentItem = null;
            }

            // Este ítem va al slot destino
            transform.SetParent(dropSlot.transform, false);
            dropSlot.currentItem = gameObject;
        }
        else
        {
            // Soltó fuera de un slot o en el mismo: regresa a su lugar
            transform.SetParent(originalParent, false);
        }

        rectTransform.anchoredPosition = Vector2.zero;
    }
}