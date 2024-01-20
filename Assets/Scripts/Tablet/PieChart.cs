using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieChart : MonoBehaviour
{
    public Image[] pieChartImages;
    public float[] values;

    //public float totalAmount = 0;

    // Start is called before the first frame update
    void Start()
    {
        SetValues(values);
    }

    // Update is called once per frame
    void Update()
    {
        // For testing
        //SetValues(values);
    }

    /// <summary>
    /// To update the pie chart, call this function
    /// </summary>
    /// <param name="index">Index to change pie chart</param>
    /// <param name="val">Value to change to</param>
    public void SetValues(int index, float val)
    {
        // Update the values
        values[index] = val;
        // Set values to reset the pie chart
        SetValues(values);
    }

    private void SetValues(float[] values)
    {
        float totalValues = 0;
        for(int i = 0; i < pieChartImages.Length; ++i)
        {
            totalValues += FindPercentage(values, i);
            pieChartImages[i].fillAmount = totalValues;
        }    
    }

    private float FindPercentage(float[] values, int index)
    {
            float totalValues = 0;
            for(int i = 0; i < values.Length; ++i)
            {
                totalValues += values[i];
            }

            return values[index] / totalValues;
    }
}
