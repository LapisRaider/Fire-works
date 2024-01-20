using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Resume : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // should have data here
    Rigidbody m_rb;

    Vector3 m_PrevPos = Vector3.zero;
    float m_GrabYMinPos = 20.0f;
    Vector3 m_MovementOffset = Vector3.zero;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    public void Initialize(Vector3 pos, Candidate data)
    {
        // reset sizes and positions here
        m_PrevPos = transform.position;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Debug.Log(name + " Game Object Clicked!");
        //open the thing up
    }

    public void OnMouseDown()
    {
        Debug.Log(name + " on mouse down");
        m_MovementOffset = transform.position - GetMouseWorldPosition(Input.mousePosition);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log(name + " begin drag");
        m_rb.isKinematic = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log(name + " end drag");
        m_rb.isKinematic = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mouseWorldPos = GetMouseWorldPosition(eventData.position);

        Debug.Log(mouseWorldPos + " on drag");
        Debug.Log(m_MovementOffset + " offset");
        transform.position = new Vector3(mouseWorldPos.x, m_GrabYMinPos, mouseWorldPos.y) + m_MovementOffset;
    }

    private Vector3 GetMouseWorldPosition(Vector2 screenPosition)
    {
        Vector3 mousePos = screenPosition;
        mousePos.z = Camera.main.nearClipPlane;

        Vector3 newPos = Camera.main.ScreenToWorldPoint(mousePos);

        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
