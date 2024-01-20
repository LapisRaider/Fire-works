using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


enum TabletState {Hidden, Hovering, Opened, TotalStates}
public enum Screens { Home, Profile, TotalScreens }
/// <summary>
/// This file will handle changing of screens
/// </summary>
public class TabletManager : SingletonBase<TabletManager>
{
    public List<GameObject> tabletScreens;
    public List<Sprite> tabletSprites;
    public List<GameObject> tabletButtons;
    // Start is called before the first frame update
    [Tooltip("The increase in y position when hovering over the tablet")]
    public float hoverY = 0.0f;

    [Tooltip("Reference to the game object rendering image of tablet")]
    public GameObject tabletImage;

    [Tooltip("Reference to the bg image of the tablet")]
    public GameObject tabletBG;

    //bool isHovering = false;
    Vector3 hoverPosition;
    Vector3 originalPosition;
    public Vector3 openPosition;

    PieChart pieChartRef;
    Graph graphRef;

    TabletState currState;
    Screens currScreen;

    override public void Awake()
    {
        base.Awake();
        pieChartRef = GetComponent<PieChart>();
        graphRef = GetComponent<Graph>();
    }
    void Start()
    {
        currState = TabletState.Opened;
        currScreen = Screens.Home;
        originalPosition = gameObject.GetComponent<Transform>().position;
        hoverPosition = gameObject.GetComponent<Transform>().position + new Vector3(0, 0, hoverY);
    }

    // Update is called once per frame
    void Update()
    {
        if(currState == TabletState.Hovering)
        {
            if(Input.GetMouseButtonDown(0))
            {
                currState = TabletState.Opened;
                StartCoroutine(OpenTablet(openPosition, 0.5f));
            }
        }
    }

    public void OnMouseEnter()
    {
        //Debug.Log("HOVERING OVER TABLET");
        if(currState == TabletState.Hidden)
        {
            currState = TabletState.Hovering;
            StartCoroutine(LerpPosition(hoverPosition, 0.5f, this.transform));
        }
    }

    public void OnMouseExit()
    {
        if (currState == TabletState.Hovering)
        {
            currState = TabletState.Hidden;
            StartCoroutine(LerpPosition(originalPosition, 0.5f, this.transform));
        }
    }

    public void ButtonPress(int screen)
    {
        Debug.Log("Screen " + screen + " Pressed");
        StartCoroutine(LerpRectPosition(new Vector3(0, 0, 0), 0.8f, tabletButtons[screen - 1].GetComponent<RectTransform>()));
        StartCoroutine(LerpScale(new Vector3(4f, 4f, 0), 0.8f, tabletButtons[screen - 1].transform));
        StartCoroutine(ScreenTransition(screen, 2.0f));
        for(int i = 0; i < tabletButtons.Count; ++i)
        {
            if (i == screen - 1) continue;

            tabletButtons[i].SetActive(false);
        }
    }

    void ChangeScreen(int index)
    {
        tabletScreens[(int)currScreen].SetActive(false);
        tabletScreens[index].SetActive(true);
    }

    public void UpdatePieChart(int index, float val)
    {
        pieChartRef.SetValues(index, val);
    }

    public void InitPieChart(int[] val)
    {
        pieChartRef.InitValues(val);
    }

    public void AddPointToGraph(int point)
    {
        graphRef.AddPointToGraph(point);
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration, Transform objTransform)
    {
        float time = 0;
        Vector3 startPosition = objTransform.position;
        while (time < duration)
        {
            objTransform.position = Vector3.Lerp(startPosition, targetPosition, time/duration);
            time += Time.deltaTime;
            yield return null;
        }
        objTransform.position = targetPosition;
    }

    IEnumerator LerpRectPosition(Vector3 targetPosition, float duration, RectTransform objTransform)
    {
        float time = 0;
        Vector3 startPosition = objTransform.anchoredPosition;
        while (time < duration)
        {
            objTransform.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        objTransform.anchoredPosition = targetPosition;
    }

    IEnumerator LerpScale(Vector3 targetScale, float duration, Transform objTransform)
    {
        float time = 0;
        Vector3 startScale = objTransform.localScale;
        while (time < duration)
        {
            objTransform.localScale = Vector3.Lerp(startScale, targetScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        objTransform.localScale = targetScale;

    }

    IEnumerator OpenTablet(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
       Vector3 startScale = transform.localScale;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time/duration);
           transform.localScale = Vector3.Lerp(startScale, new Vector3(1.2f, 1.2f, 0), time/duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = openPosition;
        transform.localScale = new Vector3(1.4f, 1.4f, 0);
    }

    IEnumerator ScreenTransition(int index, float duration)
    {
        float time = 0;
        SpriteRenderer bgSprite = tabletBG.GetComponent<SpriteRenderer>();
        Vector3 originalColor = new Vector3(255, 255, 255);
        Vector3 targetColor = new Vector3(0, 0, 0);
        while (time < duration)
        {
            if (time <= duration * 0.5f)
            {
                Vector3 newRGB = Vector3.Lerp(targetColor, originalColor, time / (duration * 0.5f));
                bgSprite.color = new Color(newRGB.x, newRGB.y, newRGB.z);
            }
            else
            {
                if (bgSprite.sprite != tabletSprites[index])
                    bgSprite.sprite = tabletSprites[index];

                Vector3 newRGB = Vector3.Lerp(originalColor, targetColor, (time * 0.5f) / (duration * 0.5f));
                bgSprite.color = new Color(newRGB.x, newRGB.y, newRGB.z);
            }
            yield return null;
        }

        bgSprite.color = new Color(255, 255, 255);
        ChangeScreen(index);
    }
}
