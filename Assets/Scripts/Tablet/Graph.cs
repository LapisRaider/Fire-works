using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
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

    [Tooltip("List of current active circles in graph")]
    List<GameObject> listOfCircles;
    [Tooltip("List of lines active in the graph")]
    List<GameObject> listOfLines;

    [Tooltip("For initializing the graph with starting values")]
    public List<int> initialList;

    //public List<Vector2> circlePositions;
    
    void Awake()
    {
        graphContainer = GameObject.Find("GraphContainer").GetComponent<RectTransform>();
        listOfCircles = new List<GameObject>();
        listOfLines = new List<GameObject>();
        //List<int> valList = new List<int>() { 10, 20, 40, 70, 50, 40, 10,};
        //ShowGraph(valList);
        //CreateCircle(new Vector2(15, 25));
        InitializeGraph(initialList);
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

    /// <summary>
    /// Only used if we want to initialize the graph with values. Mostly used for testing.
    /// </summary>
    /// <param name="list">Initial list of values</param>
    void InitializeGraph(List<int> list)
    {

            for(int i = 0; i < list.Count; ++i)
            {
                float graphHeight = graphContainer.sizeDelta.y;
                

                float xPosition = (xScale * 0.5f) + (i - 1) * xScale;
                if (i == 0) // Just for first dot to have the effect of the graph line coming out of the side
                {
                    xPosition = 0;
                }
                float yPosition = (list[i] / yMax) * graphHeight;
                GameObject newCircle = CreateCircle(new Vector2(xPosition, yPosition));
                if(listOfCircles.Count < maxPoints)
                {
                    listOfCircles.Add(newCircle);
                }
                else
                {
                    // Always remove the first item in the list
                    listOfCircles.RemoveAt(0);
                    // Add a new circle
                    listOfCircles.Add(newCircle);
                }
            }

            // Clear the lines
            RefreshLines();
            // Recreate
            CreateLines();  

            listOfCircles[0].GetComponent<Image>().enabled = false;
    }

    void AddPointToGraph(int Point)
    {
        float graphHeight = graphContainer.sizeDelta.y;

        float xPosition = 0;
        float yPosition = 0;
        if(listOfCircles.Count < maxPoints)
        {
            xPosition = xScale + listOfCircles.Count * xScale;
            yPosition = (Point / yMax) * graphHeight;
            GameObject newCircle = CreateCircle(new Vector2(xPosition, yPosition));
            listOfCircles.Add(newCircle);
        }
        else
        {
            // Need to shift all the point's x position
            for(int i = 0; i < listOfCircles.Count - 1; ++i)
            {
                RectTransform firstTransform = listOfCircles[i].GetComponent<RectTransform>();
                RectTransform secondTransform = listOfCircles[i + 1].GetComponent<RectTransform>();
                Debug.Log("First Pos: " + firstTransform.anchoredPosition);
                Debug.Log("Second Pos: " + secondTransform.anchoredPosition);
                Vector2 tempPos = firstTransform.anchoredPosition; // Store the first points transform
                firstTransform.anchoredPosition = new Vector2(firstTransform.anchoredPosition.x, secondTransform.anchoredPosition.y);
                secondTransform.anchoredPosition = new Vector2(secondTransform.anchoredPosition.x, tempPos.y);
            }

            xPosition = listOfCircles[listOfCircles.Count - 1].GetComponent<RectTransform>().anchoredPosition.x;
            yPosition = (Point / yMax) * graphHeight;
            GameObject newCircle = CreateCircle(new Vector2(xPosition, yPosition));

            Destroy(listOfCircles[listOfCircles.Count - 1]);
            // Always remove the first item in the list
            listOfCircles.RemoveAt(listOfCircles.Count - 1);




            // Add a new circle
            listOfCircles.Add(newCircle);

            listOfCircles[0].GetComponent<Image>().enabled = false;
        }

        // Honestly can probably find a way to add on lines without destroying and recreating them all
        // But there isnt that many and time is short D;
        // Clear the lines
        RefreshLines();
        // Recreate
        CreateLines();
    }

    void CreateLines()
    {
            GameObject prevCircle = null;
            // Loop through the list
            for(int i = 0; i < listOfCircles.Count; ++i)
            {
                if (prevCircle != null)
                {
                        GameObject connectionObject = CreateDotConnection(prevCircle.GetComponent<RectTransform>().anchoredPosition, listOfCircles[i].GetComponent<RectTransform>().anchoredPosition);
                        listOfLines.Add(connectionObject);
                }

                prevCircle = listOfCircles[i];
            }
    }

    void RefreshLines()
    {
        // Destroy the line objects
        for(int i = 0; i < listOfLines.Count; ++i)
        {
            Destroy(listOfLines[i]);
        }
        // Clear the list after destroying 
        listOfLines.Clear();
    }

    GameObject CreateDotConnection(Vector2 dotA, Vector2 dotB) 
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

        return connectionObject;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
                AddPointToGraph(50);
        }
    }
}
