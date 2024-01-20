using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    private float m_GrabYMinPos = 20.0f;

    [Header("Physics")]
    public float m_MaxThrowVelocity = 30.0f;
    private Rigidbody m_rb;
    private Quaternion m_InitialRotation = Quaternion.identity;

    // flying to table
    private Vector3 m_FlyDir = Vector3.zero;
    private Vector3 m_TablePos = Vector3.zero;
    private float m_FlySpeed = 1.0f;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_InitialRotation = transform.rotation;
    }
    public void Initialize(Candidate data, Vector3 spawnPos, Vector3 landPos, float flySpeed)
    {
        m_Data = data;

        m_FlySpeed = flySpeed;

        // need to lerp to a position
        transform.position = spawnPos;
        transform.rotation = m_InitialRotation;

        m_TablePos = landPos;
        m_FlyDir = (landPos - spawnPos).normalized;
        StartCoroutine(FlyTowardsTable());
    }
    float CalculateDecleration(float initialVelocity, Vector3 finalPosition, Vector3 initialPosition)
    {
        float finalVelocity = 0.0f; // Final velocity is 0, as you want to come to a stop
        float distance = Vector3.Distance(finalPosition, initialPosition);

        // Calculate deceleration using the formula
        float deceleration = (finalVelocity * finalVelocity - initialVelocity * initialVelocity) / (2 * distance);

        return deceleration;
    }
    IEnumerator FlyTowardsTable()
    {
        m_rb.isKinematic = true;

        float deceleration = CalculateDecleration(m_FlySpeed, transform.position, m_TablePos);
        while (Vector3.Dot(m_FlyDir, (transform.position - m_TablePos).normalized) < 0)
        {
            // Move towards the target position
            m_FlySpeed += deceleration * Time.deltaTime;
            m_FlySpeed = Math.Max(m_FlySpeed, 3.0f);
            transform.position += m_FlySpeed * m_FlyDir * Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        m_rb.isKinematic = false;
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
