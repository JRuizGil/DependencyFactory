using UnityEngine;

public class BeanSackDrag : MonoBehaviour
{
    private BeansContainer container;
    private Vector3 originalLocalPos;
    private bool isDragging;

    void Awake()
    {
        container = GetComponentInParent<BeansContainer>();
    }

    void OnMouseDown()
    {
        if (container == null)
            return;

        if (!container.CanStartDrag())
            return;

        originalLocalPos = transform.localPosition;
        isDragging = true;

        container.OnStartDragging(this);
    }

    void OnMouseDrag()
    {
        if (!isDragging)
            return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        transform.position = mousePos;
    }

    void OnMouseUp()
    {
        if (!isDragging)
            return;

        isDragging = false;

        bool validDrop = container.IsValidDropPosition(transform.position);

        if (!validDrop)
        {
            transform.localPosition = originalLocalPos;
        }

        container.OnEndDragging(validDrop);
    }
}
