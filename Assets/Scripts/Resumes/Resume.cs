using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resume : MonoBehaviour
{
    [Header("Data")]
    Candidate m_Data;

    [Header("Clicking")]
    public float m_ClickTimeThreshold = 0.2f;
    private float m_InitialClickTime = 0.0f;
    private bool m_IsClick = true;

    private Vector3 m_PaperToMouseOffset;
    private float m_InitialZCoord;
    private Vector3 m_PrevMousePos = Vector3.zero;

    [Header("Physics")]
    public float m_MaxThrowVelocity = 30.0f;
    private Rigidbody m_rb;
    private float m_GrabYMinPos = 20.0f;


    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        Debug.Log("Heere");
    }
    public void Initialize(Candidate data, Vector3 spawnPos, Vector3 landPos)
    {
        // need to lerp to a position

    }

    void OnMouseDown()
    {
        m_InitialClickTime = Time.time;
        m_IsClick = true;

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
        vel = new Vector3(vel.x, 0.0f, vel.y);
        Vector3 dir = vel.normalized;
        float magnitude = Mathf.Max(Vector3.Magnitude(vel), m_MaxThrowVelocity);

        m_rb.AddForce(magnitude * dir, ForceMode.Impulse);

        // reset 
        m_IsClick = true;
    }
}
