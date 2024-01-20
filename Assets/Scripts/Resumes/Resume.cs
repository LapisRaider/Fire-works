using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resume : MonoBehaviour
{
    // should have data here
    Rigidbody m_rb;

    float m_GrabYMinPos = 20.0f;
    Vector3 m_MovementOffset = Vector3.zero;

    public float m_ClickTimeThreshold = 0.2f;
    private float m_InitialClickTime = 0.0f;
    private bool m_IsClick = true;

    private Vector3 m_PaperToMouseOffset;
    private float m_InitialZCoord;
    private Vector3 m_PrevMousePos = Vector3.zero;


    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    void OnMouseDown()
    {
        m_InitialClickTime = Time.time;

        m_InitialZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        m_PaperToMouseOffset = gameObject.transform.position - GetMouseAsWorldPoint();
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = m_InitialZCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void OnMouseDrag()
    {
        if (m_IsClick && Time.time - m_InitialClickTime < m_ClickTimeThreshold)
            return;

        m_IsClick = false;
        m_PrevMousePos = Input.mousePosition;
        transform.position = GetMouseAsWorldPoint() + m_PaperToMouseOffset;
    }
    private void OnMouseUp()
    {
        if (m_IsClick)
        {
            // click behavior
            Debug.Log("Clicked");
            return;
        }

        // toss object
        Vector3 vel = Input.mousePosition - m_PrevMousePos;
        m_rb.AddForce(new Vector3(vel.x, 0.0f, vel.y), ForceMode.Impulse);

        // reset 
        m_IsClick = true;
    }
}
