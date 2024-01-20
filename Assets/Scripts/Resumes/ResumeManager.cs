using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeManager : SingletonBase<ResumeManager>
{
    // Time taken for a new batch of applicants to start flying into the screen
    public float batchTime;

    // Average number of applicants in a batch
    public int average;

    // Offset of the median to make each batch a random number of applicants
    public int offset;

    // Time taken for the special event to arrive [ Superior HR applicant to replace the player ]
    public float replacementTime;

    void NewBatch() {
        Debug.Log("NewBatch");
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.onNewMonth += NewBatch;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
