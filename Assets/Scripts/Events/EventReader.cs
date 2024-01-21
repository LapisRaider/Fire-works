using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EventObject
{
    public int index;
    public bool delayed;
    public JOB_DEPARTMENT eventDepartment;
    public string eventText;
}

public class EventReader : MonoBehaviour
{
    public List<EventObject> events;
    public GameObject newsBar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerEvent(JOB_DEPARTMENT targetDepartment)
    {

    }
}
