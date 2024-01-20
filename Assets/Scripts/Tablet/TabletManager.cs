using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


enum TabletState {Hidden, Hovering, Opened, TotalStates}
enum Screens { Profile, TotalScreens }
/// <summary>
/// This file will handle changing of screens
/// </summary>
public class TabletManager : MonoBehaviour
{
    public List<GameObject> tabletScreens;
    // Start is called before the first frame update
    [Tooltip("The increase in y position when hovering over the tablet")]
    public float hoverY = 0.0f;

    //bool isHovering = false;
    Vector3 hoverPosition;
    Vector3 originalPosition;
    public Vector3 openPosition;

    TabletState currState;
    void Start()
    {
        currState = TabletState.Hidden;
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

    public void ToggleScreen(int index)
    {

    }

    public void OnMouseEnter()
    {
        //Debug.Log("HOVERING OVER TABLET");
        if(currState == TabletState.Hidden)
        {
            currState = TabletState.Hovering;
            StartCoroutine(LerpPosition(hoverPosition, 0.5f));
        }
    }

    public void OnMouseExit()
    {
        if (currState == TabletState.Hovering)
        {
            currState = TabletState.Hidden;
            StartCoroutine(LerpPosition(originalPosition, 0.5f));
        }
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time/duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
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

    // IEnumerator CloseTablet(float duration)
    // {

    // }

}