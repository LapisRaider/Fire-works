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
    [System.NonSerialized] public JOB_DEPARTMENT targetDepartment;

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
        GameManager.onNewMonth += Call;
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
                    if (activeEvent != null && activeEvent.delayed == false)
                    {
                        // If the event belongs to research
                        if (activeEvent.targetDepartment == JOB_DEPARTMENT.RESEARCH)
                        {
                            GameManager.Instance.currentMoney *= 1.5f;

                        }

                        activeEvent = null;
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            TriggerEvent(JOB_DEPARTMENT.SECURITY);
        }
    }

    void Call()
    {
        //Every January and may for now
        if (activeEvent == null)
        {
            if (GameManager.Instance.currentMonth == 0 && GameManager.Instance.currentQuarter == 1)
            {
                TriggerEvent(JOB_DEPARTMENT.SECURITY);
            }
            else if (GameManager.Instance.currentMonth == 4 && GameManager.Instance.currentQuarter == 0)
            {
                TriggerEvent(JOB_DEPARTMENT.SECURITY);
            }

            float breakthroughChance = Mathf.Clamp(1 / (1 + Mathf.Pow(1.2f, 10 - GameManager.Instance.departments[(int)JOB_DEPARTMENT.RESEARCH])), 0.0f, 1.0f);
            float r = UnityEngine.Random.Range(0.0f, 1.0f);
            if (r <= breakthroughChance)
            {
                // Succeed
                TriggerEvent(JOB_DEPARTMENT.RESEARCH);
            }
        }

        // There is a delayed action
        if (activeEvent != null && activeEvent.delayed)
        {
            float odds = ((Mathf.Log(GameManager.Instance.departments[(int)JOB_DEPARTMENT.SECURITY])/Mathf.Log(5)) + 1) / 2;
            odds = Mathf.Clamp(odds, 0.0f, 1.0f);

            float r = UnityEngine.Random.Range(0.0f, 1.0f);
            if (r > odds) {
                // Fail odds
                GameManager.Instance.currentMoney *= 0.5f;

                //TODO GID ADD NEWS
            }

            activeEvent = null;
        }
       // bool a = true;
    }

    public void TriggerEvent(JOB_DEPARTMENT targetDepartment)
    {
        if (activeEvent != null)
        {
            Debug.Log("Event alreayd active!");
            return;
        }

        eventTriggered = true;
        for(int i = 0; i < listOfEvents.Count; i++)
        {
            if (listOfEvents[i].eventDepartment == targetDepartment)
            {
                // Pick a random event from this
                int randomNum = UnityEngine.Random.Range(0, listOfEvents[i].events.Count);
                activeEvent = listOfEvents[i].events[randomNum];
                textObj.text += activeEvent.eventText;

                break;
            }
        }
    }
}
