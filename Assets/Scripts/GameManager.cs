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
    // In game timer for month
    public const float MONTH_TIME = 1.0f;   // Time taken for a month, adjust this when balancing
    public int currentMonth = 0;            // 0 is January, 11 is December
    public float monthTime = MONTH_TIME;
    public float monthTimer = 0;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        // Month timer
        monthTimer += Time.deltaTime;
        if (monthTimer >= MONTH_TIME) {
            monthTimer -= MONTH_TIME;
            onNewMonth();
        }
    }
}
