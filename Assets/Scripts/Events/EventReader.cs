using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[Serializable]
public class EventObject
{
    public bool delayed;
    public string eventText;

}

[Serializable]
public class EventList
{
    public int index;
    public JOB_DEPARTMENT eventDepartment;
    public List<EventObject> events;
}


public class EventReader : MonoBehaviour
{
    public List<EventList> listOfEvents;
    public GameObject newsBar;
    public TextMeshProUGUI textObj;
    public float timeTillCut = 0.1f;
    float timer = 0.0f;

    bool eventTriggered = false;
    EventObject activeEvent = null;
    string initialStr;

    // Start is called before the first frame update
    void Start()
    {
        initialStr = textObj.text.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (eventTriggered)
        {
            timer += Time.deltaTime;
            if (timer > timeTillCut)
            {
                timer = 0.0f;
                string str = textObj.text.ToString();
                //str.Remove(0);
                if (str.Length > 0 )
                {
                    textObj.text = str.Substring(1);
                }
                else
                {
                    // Reset the event
                    eventTriggered = false;
                    textObj.text = initialStr;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            TriggerEvent(JOB_DEPARTMENT.SECURITY);
        }
    }

    public void TriggerEvent(JOB_DEPARTMENT targetDepartment)
    {
        eventTriggered = true;
        for(int i = 0; i < listOfEvents.Count; i++)
        {
            if (listOfEvents[i].eventDepartment == targetDepartment)
            {
                // Pick a random event from this
                int randomNum = UnityEngine.Random.Range(0, listOfEvents[i].events.Count);
                if (activeEvent == null)
                {
                    activeEvent = listOfEvents[i].events[randomNum];
                    textObj.text += activeEvent.eventText;
                }
                else
                {
                    Debug.Log("Event alreayd active!");
                }
                break;
            }
        }
    }
}
