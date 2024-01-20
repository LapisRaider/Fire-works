using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{
    public Sprite circleSprite;
    private RectTransform graphContainer;

    [Tooltip("Max number of points to show on graph")]
    public int maxPoints;

    [Tooltip("Distance between points on x axis")]
    public float xScale = 25.0f;

    [Tooltip("Max height of the points")]
    public float yMax = 75.0f;

    List<GameObject> listOfCircles;
    
    void Awake()
    {
        graphContainer = GameObject.Find("GraphContainer").GetComponent<RectTransform>();

        List<int> valList = new List<int>() { 10, 20, 40, 70, 50, 40, 10,};
        ShowGraph(valList);
        //CreateCircle(new Vector2(15, 25));
    }

    GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject circleObj = new GameObject("Circle", typeof(Image));
        circleObj.transform.SetParent(graphContainer, false);
        circleObj.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = circleObj.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(4, 4);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);

        return circleObj;
    }

    void ShowGraph(List<int> valueList)
    {
        float graphHeight = graphContainer.sizeDelta.y;
        GameObject previousCircle = null;
        for (int i = 0; i < valueList.Count; ++i)
        {
            float xPosition = xScale + i * xScale;
            float yPosition = (valueList[i] / yMax) * graphHeight;
            GameObject newCircle = CreateCircle(new Vector2(xPosition, yPosition));
            //if(listOfCircles.Count <= )
            if (previousCircle != null)
            {
                // Create a connection
                CreateDotConnection(previousCircle.GetComponent<RectTransform>().anchoredPosition, newCircle.GetComponent<RectTransform>().anchoredPosition);
            }
            previousCircle = newCircle;
        }
    }

    void CreateDotConnection(Vector2 dotA, Vector2 dotB) 
    {
        GameObject connectionObject = new GameObject("ConnectionObj", typeof(Image));
        connectionObject.transform.SetParent(graphContainer, false);
        connectionObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        RectTransform rectTransform = connectionObject.GetComponent<RectTransform>();
        Vector2 dir = (dotB - dotA).normalized;
        float distance = Vector2.Distance(dotA, dotB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 1.2f);
        rectTransform.anchoredPosition = dotA + dir * distance * 0.5f;
        float angle = MathF.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rectTransform.rotation = Quaternion.Euler(0f, 0f, angle);
        // Vector3 temp = Vector3.Cross(new Vector2(0, 1), dir);
        // Debug.Log(dir);
        // if (temp.z > 0)
        // {
        //     angle = Mathf.Acos(Vector2.Dot(new Vector2(0, 1), dir));
        // }
        // else
        // {
        //     angle = Mathf.Acos(Vector2.Dot(dir, new Vector2(0, 1)));

        // }
        // rectTransform.localEulerAngles = new Vector3(0, 0, angle);
        // Quaternion rotation = Quaternion.LookRotation(dir, new Vector3(0, 0, 1));
        // rectTransform.rotation = rotation;
    }

    // void CreateLineConnection(Vector2 dotA, Vector2 dotB)
    // {
    //         GameObject lineObj = new GameObject("LineObj", typeof(LineRenderer));
    //         lineObj.transform.SetParent(graphContainer, false);

    //         LineRenderer line = lineObj.GetComponent<LineRenderer>();
    //         line.positionCount = 2; // Cause only two points
    //         //Vector2[] positions = {dotA, dotB};
    //         line.SetPosition(0, dotA);
    //         line.SetPosition(1, dotB);
    //         line.useWorldSpace = false;
    // }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
