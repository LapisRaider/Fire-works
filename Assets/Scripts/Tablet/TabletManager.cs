using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

enum TabletState {Hidden, Hovering, Opened, TotalStates}
public enum Screens { Home, Profile, Help, TotalScreens }

[Serializable]
public class ScreenObject
{
    public List<GameObject> uiObjects = new List<GameObject> ();
}

public struct savedUIObject
{
    public Vector3 originalPosition;
    public Vector3 originalScale;
}


/// <summary>
/// This file will handle changing of screens
/// </summary>
public class TabletManager : SingletonBase<TabletManager>, IPointerEnterHandler, IPointerExitHandler
{
    public List<GameObject> tabletScreens;
    public List<Sprite> tabletSprites;
    public List<GameObject> tabletButtons;
    public List<ScreenObject> screenObjects;
    public List<String> helpTextList;
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
    public Vector3 openScale;
    public Vector3 closeScale;

    PieChart pieChartRef;
    Graph graphRef;

    TabletState currState;
    Screens currScreen;

    bool isHovering = false;

    public TextMeshProUGUI helpText;

    public List<savedUIObject> savedUIObjects;

    override public void Awake()
    {
        base.Awake();
        pieChartRef = GetComponentInChildren<PieChart>();
        graphRef = GetComponentInChildren<Graph>();
    }
    void Start()
    {
        currState = TabletState.Hidden;
        currScreen = Screens.Home;
        originalPosition = tabletImage.GetComponent<Transform>().position;
        hoverPosition = tabletImage.GetComponent<Transform>().position + new Vector3(0, hoverY, 0);
        savedUIObjects = new List<savedUIObject>();

        for (int i = 0; i < tabletButtons.Count; ++i)
        {
            savedUIObject savedObject = new savedUIObject();
            savedObject.originalPosition = tabletButtons[i].GetComponent<RectTransform>().anchoredPosition;
            savedObject.originalScale = tabletButtons[i].transform.localScale;
            savedUIObjects.Add(savedObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (currState == TabletState.Hovering)
            {
                currState = TabletState.Opened;
                StartCoroutine(ToggleTablet(true, 0.2f));
            }
            // If its not hovering and the tablet is open
            else if (isHovering == false && currState == TabletState.Opened)
            {
                // Close the tablet
                StartCoroutine(ToggleTablet(false, 0.2f));
            }
        }

        if (currState == TabletState.Hovering && isHovering == false)
        {
            currState = TabletState.Hidden;
            StartCoroutine(LerpPosition(originalPosition, 0.2f, tabletImage.transform));
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("HOVERING OVER TABLET");

        if (currState == TabletState.Hidden)
        {
            currState = TabletState.Hovering;
            StartCoroutine(LerpPosition(hoverPosition, 0.2f, tabletImage.transform));
        }

        isHovering = true;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("NOT HOVERING OVER TABLET");
        if (currState == TabletState.Hovering)
        {
            currState = TabletState.Hidden;
            StartCoroutine(LerpPosition(originalPosition, 0.2f, tabletImage.transform));
        }

        isHovering = false;

    }

    public void ButtonPress(int screen)
    {
        Debug.Log("Screen " + screen + " Pressed");
        // Back buttons to go back to menu only has screen transitioning
        if (screen != 0) // Only menu buttons going away from home screen has the scale effect
        {
            StartCoroutine(LerpRectPosition(new Vector3(0, 0, 0), 0.25f, tabletButtons[screen - 1].GetComponent<RectTransform>()));
            StartCoroutine(LerpScale(new Vector3(4.0f, 4.0f, 0), 0.25f, tabletButtons[screen - 1].transform));
            for (int i = 0; i < tabletButtons.Count; ++i)
            {
                tabletButtons[i].GetComponent<Button>().enabled = false;

                if (i == screen - 1) continue;

                tabletButtons[i].SetActive(false);
            }

            // If going into stats screen
            if (screen == (int)Screens.Profile)
            {
                // Fade in the stats object
                ResetStats();
            }
            if (screen == (int)Screens.Help)
            {
                ResetHelp();
            }
        }
        // Going back to home screen
        // Fade elements in those screens to black
        else if (screen == 0)
        {
            // For profile, fade the graph and piechart
            if (currScreen == Screens.Profile)
            {
                for(int i = 0; i < screenObjects[(int)currScreen].uiObjects.Count; ++i)
                { 
                    if (screenObjects[(int)currScreen].uiObjects[i].GetComponent<Button>() != null)
                    {
                        screenObjects[(int)currScreen].uiObjects[i].GetComponent<Button>().enabled = false;
                    }

                    StartCoroutine(FadeEffect(true, 0.3f, screenObjects[(int)currScreen].uiObjects[i]));
                    // First loop for main containers
                    foreach (Transform child in screenObjects[(int)currScreen].uiObjects[i].transform)
                    {
                        // If it has a sprite renderer
                        // it neeeds to fade
                        // Second loop for nested objects
                        if (child.gameObject.GetComponent<Image>() != null)
                        {
                            StartCoroutine(FadeEffect(true, 0.3f, child.gameObject));
                        }
                        // Last loop to check for any objects created in run time
                        // imo only going to be used for graph
                        foreach(Transform anotherChild in child)
                        {
                            if (anotherChild.gameObject.GetComponent<Image>() != null)
                            {
                                StartCoroutine(FadeEffect(true, 0.3f, anotherChild.gameObject));
                            }

                        }
                    }
                }
            }
        }

        StartCoroutine(ScreenTransitionStart(screen, 0.3f));
    }

    void ChangeScreen(int index)
    {
        tabletScreens[(int)currScreen].SetActive(false);
        tabletScreens[index].SetActive(true);
        currScreen = (Screens)index;

        if (currScreen == Screens.Home)
        {
            ResetButtons();
        }
    }

    void ResetButtons()
    {
        for(int i = 0; i < savedUIObjects.Count; ++i)
        {
            tabletButtons[i].SetActive(true);
            tabletButtons[i].GetComponent<RectTransform>().anchoredPosition = savedUIObjects[i].originalPosition;
            tabletButtons[i].transform.localScale = savedUIObjects[i].originalScale;
            tabletButtons[i].GetComponent<Button>().enabled = true;
        }
    }

    void ResetHelp()
    {
        for (int i = 0; i < screenObjects[(int)Screens.Profile].uiObjects.Count; ++i)
        {
            if (screenObjects[(int)Screens.Profile].uiObjects[i].GetComponent<Button>() != null)
            {
                screenObjects[(int)Screens.Profile].uiObjects[i].GetComponent<Button>().enabled = true;
            }
        }
    }

        void ResetStats()
    {
        for (int i = 0; i < screenObjects[(int)Screens.Profile].uiObjects.Count; ++i)
        {
            if (screenObjects[(int)Screens.Profile].uiObjects[i].GetComponent<Button>() != null)
            {
                screenObjects[(int)Screens.Profile].uiObjects[i].GetComponent<Button>().enabled = true;
            }

            StartCoroutine(FadeEffect(false, 0.3f, screenObjects[(int)Screens.Profile].uiObjects[i]));
            // First loop for main containers
            foreach (Transform child in screenObjects[(int)Screens.Profile].uiObjects[i].transform)
            {
                // If it has a sprite renderer
                // it neeeds to fade
                // Second loop for nested objects
                if (child.gameObject.GetComponent<Image>() != null)
                {
                    StartCoroutine(FadeEffect(false, 0.3f, child.gameObject));
                }
                // Last loop to check for any objects created in run time
                // imo only going to be used for graph
                foreach (Transform anotherChild in child)
                {
                    if (anotherChild.gameObject.GetComponent<Image>() != null)
                    {
                        StartCoroutine(FadeEffect(false, 0.3f, anotherChild.gameObject));
                    }

                }
            }
        }
    }

    public void ToggleHelpText(int index)
    {
        helpText.text = helpTextList[index];
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

    IEnumerator ToggleTablet(bool toggle, float duration)
    {
       float time = 0;
       Vector3 startPosition = tabletImage.transform.position;
       Vector3 startScale = tabletImage.transform.localScale;
        if (toggle)
        {
            while (time < duration)
            {
                tabletImage.transform.position = Vector3.Lerp(startPosition, openPosition, time / duration);
                tabletImage.transform.localScale = Vector3.Lerp(startScale, openScale, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            tabletImage.transform.position = openPosition;
            tabletImage.transform.localScale = openScale;
        }
        else
        {
            while (time < duration)
            {
                tabletImage.transform.position = Vector3.Lerp(startPosition, hoverPosition, time / duration);
                tabletImage.transform.localScale = Vector3.Lerp(startScale, closeScale, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            tabletImage.transform.position = hoverPosition;
            tabletImage.transform.localScale = closeScale;
            currState = TabletState.Hovering;
        }
    }


    IEnumerator ScreenTransitionStart(int index, float duration)
    {
        float time = 0;
        Image bgSprite = tabletBG.GetComponent<Image>();
        //Color targetColor = new Color(0, 0, 0);
        //Color originalColor = new Color(255, 255, 255);
        while (time < duration)
        {
            Color rgb = Color.Lerp(Color.white, Color.black, time / duration);
            bgSprite.color = rgb;
            time += Time.deltaTime;
            yield return null;
        }

        bgSprite.color = Color.black;
        for(int i = 0; i < tabletButtons.Count; ++i)
        {
            tabletButtons[i].SetActive(false);
        }
        StartCoroutine(ScreenTransitionEnd(index, duration));
            //ChangeScreen(index);
    }

    IEnumerator ScreenTransitionEnd(int index, float duration)
    {
        float time = 0;
        Image bgSprite = tabletBG.GetComponent<Image>();
        //Color targetColor = new Color(0, 0, 0);
        //Color originalColor = new Color(255, 255, 255);
        while (time < duration)
        {
            Color rgb = Color.Lerp(Color.black, Color.white, time / duration);
            bgSprite.color = rgb;

            time += Time.deltaTime;
            Debug.Log("Color : " + bgSprite.color);
            yield return null;
        }

        bgSprite.color = Color.white;
        ChangeScreen(index);
    }

    IEnumerator FadeEffect(bool fade, float duration, GameObject objectToFade)
    {
        float time = 0;
        if (objectToFade.GetComponent<Image>() != null)
        {
            Image bgSprite = objectToFade.GetComponent<Image>();
            //Color targetColor = new Color(0, 0, 0);
            //Color originalColor = new Color(255, 255, 255);
            if (fade)
            {
                Color color = bgSprite.color;
                while (time < duration)
                {
                    float alpha = math.lerp(1f, 0f, time / duration);
                    //Color rgb = Color.Lerp(Color.white, Color.black, time / duration);
                    color.a = alpha;
                    bgSprite.color = color;
                    time += Time.deltaTime;
                    yield return null;
                }
                color = bgSprite.color;
                color.a = 0f;
                bgSprite.color = color;
            }
            else
            {
                Color color = bgSprite.color;
                while (time < duration)
                {
                    float alpha = math.lerp(0f, 1f, time / duration);
                    //Color rgb = Color.Lerp(Color.white, Color.black, time / duration);
                    color.a = alpha;
                    bgSprite.color = color;
                    time += Time.deltaTime;
                    yield return null;
                }
                color = bgSprite.color;
                color.a = 1f;
                bgSprite.color = color;
            }
        }
        else if (objectToFade.GetComponent<SpriteRenderer>() != null)
        {
            SpriteRenderer bgSprite = objectToFade.GetComponent<SpriteRenderer>();
            //Color targetColor = new Color(0, 0, 0);
            //Color originalColor = new Color(255, 255, 255);
            if (fade)
            {
                while (time < duration)
                {
                    Color rgb = Color.Lerp(Color.white, Color.black, time / duration);
                    bgSprite.color = rgb;
                    time += Time.deltaTime;
                    yield return null;
                }

                bgSprite.color = Color.black;
            }
            else
            {
                while (time < duration)
                {
                    Color rgb = Color.Lerp(Color.black, Color.white, time / duration);
                    bgSprite.color = rgb;
                    time += Time.deltaTime;
                    yield return null;
                }
                bgSprite.color = Color.white;
            }
        }


    }
}
