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
        int a = 11 - GameManager.Instance.currentMonth;

        if (a <= 9)
        {
            Tens.text = "0";
            Ones.text = (a).ToString();
        }
        else
        {
            Tens.text = "1";
            Ones.text = (a - 10).ToString();
        }
    }
}
