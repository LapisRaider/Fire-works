using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resume : MonoBehaviour
{
    // should have data here

    Vector3 m_PrevPos = Vector3.zero;

    bool m_IsThrown = false;

    public void Initialize(Vector3 pos, Candidate data)
    {
        // reset sizes and positions here
        m_IsThrown = false;
        m_PrevPos = transform.position;
    }

    private void OnMouseDown()
    {
        
    }

    private void OnMouseDrag() 
    { 

    }
}
