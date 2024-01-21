using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DateChanger : MonoBehaviour
{
    public TextMeshPro Tens;
    public TextMeshPro Ones;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.currentMonth < 9)
        {
            Tens.text = "0";
            Ones.text = (GameManager.Instance.currentMonth + 1).ToString();
        }
        else
        {
            Tens.text = "1";
            Ones.text = ((GameManager.Instance.currentMonth + 1) - 10).ToString();
        }
    }
}
