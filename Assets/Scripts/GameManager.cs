using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JOB_DEPARTMENT {
    HR,
    FINANCE,
    MARKETING,
    PRODUCTION,
    RESEARCH,
    QA,
    SECURITY,
    TOTAL_DEPARTMENTS,
}

public class GameManager : SingletonBase<GameManager>
{
    public int[] departments = new int[(int)JOB_DEPARTMENT.TOTAL_DEPARTMENTS];

    // In game timer for month  
    public int currentMonth;    // 0 is January, 11 is December
    public float monthTime;     // Time taken for a month
    public float monthTimer;    // Time elapsed, updates per frame

    /// <summary>
    /// Observer pattern for New month type events
    /// Use GameManager.onNewMonth += MyFunc;
    /// So that when theres a new month, myFunc is called
    /// </summary>

    public delegate void OnNewMonth();
    
    public static event OnNewMonth onNewMonth;

    // Start is called before the first frame update
    void Start()
    {
        InitValues();   
    }

    // Change values based on balancing
    void InitValues() {
        currentMonth = 0;
        monthTime = 1.0f;
        monthTimer = 0;

        departments[(int)JOB_DEPARTMENT.HR] = 1;
        departments[(int)JOB_DEPARTMENT.FINANCE] = 1;
        departments[(int)JOB_DEPARTMENT.MARKETING] = 1;
        departments[(int)JOB_DEPARTMENT.PRODUCTION] = 1;
        departments[(int)JOB_DEPARTMENT.RESEARCH] = 1;
        departments[(int)JOB_DEPARTMENT.QA] = 1;
        departments[(int)JOB_DEPARTMENT.SECURITY] = 1;

        TabletManager.Instance.InitPieChart(departments);
    }

    // Update is called once per frame
    void Update()
    {
        // Month timer
        monthTimer += Time.deltaTime;
        if (monthTimer >= monthTime) {
            monthTimer -= monthTime;
            onNewMonth();
        }
    }
}
