using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JOB_DEPARTMENT {
    HR,         // Increase number of applicants
    MARKETING,  // Generate demand every quarter
    PRODUCTION, // Generate supply every quarter
    QA,         // Increases price of product
    FINANCE,    // Decrease costs of product and hires
    RESEARCH,   // Increases breakthrough chance
    SECURITY,   // Decreases chance of breach
    TOTAL_DEPARTMENTS,
}

public class GameManager : SingletonBase<GameManager>
{
    public int[] departments = new int[(int)JOB_DEPARTMENT.TOTAL_DEPARTMENTS];

    // In game timer for month and quarters of month
    public int currentYear;    // 0 is first year, 4 is last year (Once 5th year ends, go to ending)
    public int currentMonth;    // 0 is January, 11 is December
    public int currentQuarter;  // 0 is first quarter of month, 3 is last
    public float quarterMonthTime;  // Time taken for a quarter of a month
    public float quarterMonthTimer; // Time elapsed, updates per frame

    /// <summary>
    /// Observer pattern for New month type events
    /// Use GameManager.onNewMonth += MyFunc;
    /// So that when theres a new month, myFunc is called
    /// </summary>

    // Stock graph variables
    public float currentMoney;
    public float currentMarketSalary;
    public float currentPrice;
    public float currentCost;
    public float currentSupply;
    public float currentDemand;
    public float currentProfits;
    public float currentSalaries;
    
    // Negative events
    public float demandLossRate; // How much of demand is lost over time
    public float resignationRate;

    // Misc events variables
    public float breakthroughChance;

    public delegate void OnNewMonth();
    
    public static event OnNewMonth onNewMonth = null;

    // Start is called before the first frame update
    void Start()
    {
        InitValues();   
    }

    // TODO : Change values based on balancing
    void InitValues() {
        currentMonth = 0;
        quarterMonthTime = 2.5f;
        quarterMonthTimer = 0.0f;

        departments[(int)JOB_DEPARTMENT.HR] = 1;
        departments[(int)JOB_DEPARTMENT.MARKETING] = 0;
        departments[(int)JOB_DEPARTMENT.PRODUCTION] = 1;
        departments[(int)JOB_DEPARTMENT.QA] = 0;
        departments[(int)JOB_DEPARTMENT.FINANCE] = 1;
        departments[(int)JOB_DEPARTMENT.RESEARCH] = 0;
        departments[(int)JOB_DEPARTMENT.SECURITY] = 0;
        
        currentMoney = 1000;
        currentSalaries = 50;
        currentMarketSalary = 0.0f;

        TabletManager.Instance.InitPieChart(departments);

    }

    // Update is called once per frame
    void Update()
    {
        // Quarter-Month timer
        quarterMonthTimer += Time.deltaTime;
        if (quarterMonthTimer >= quarterMonthTime) {
            quarterMonthTimer -= quarterMonthTime;
            UpdateFinances();
            currentQuarter++;


            if (currentQuarter >= 4) {
                currentQuarter = 0;
                currentMonth++;
                if (onNewMonth != null) { onNewMonth(); }
            }

            if (currentMonth >= 12) {
                currentMonth = 0;
                EndOfYearUpdate();
            }
        }
    }

    void EndOfYearUpdate() {
        // TODO: Transition to boss
        currentYear++;
    }

    public void Hire(float _hiringCost, JOB_DEPARTMENT _department) {
        departments[(int)_department] += 1;
        currentMoney -= _hiringCost;
        currentSalaries += _hiringCost + currentMarketSalary;
        TabletManager.Instance.UpdatePieChart((int)_department, departments[(int)_department]);

        switch (_department)
        {
            case JOB_DEPARTMENT.HR: {
                // Handled in ResumeManager
            } break;
            case JOB_DEPARTMENT.MARKETING: {
                currentProfits += 5 * currentSalaries;
            } break;
            case JOB_DEPARTMENT.PRODUCTION: {
                currentProfits += 3 * currentSalaries;
            } break;
            case JOB_DEPARTMENT.QA: {
                currentProfits += 1 * currentSalaries;
            } break;
            case JOB_DEPARTMENT.FINANCE: {
                
            } break;
            case JOB_DEPARTMENT.RESEARCH: {
                // TODO
            } break;
            case JOB_DEPARTMENT.SECURITY: {
                // TODO
            } break;

            default: break;
        }
    }
    public void Hire(Candidate _candidate) {
        Hire(_candidate.Salary, _candidate.m_Department);
    }

    void UpdateFinances() {
float profit = 0;

        switch (currentYear)
        {
            case 0:
            {
                if (currentMonth >= 3) {
                    currentDemand -= 0.25f;
                }
            } break;
            case 1:
            {

            } break;
            case 2:
            {

            } break;
            case 3:
            {

            } break;
            case 4:
            {

            } break;
            default: break;
        }
        
        currentProfits -= currentSalaries;
        if (currentProfits < 0) { currentProfits = 0; }
        currentMoney += currentProfits - currentSalaries;
        
        TabletManager.Instance.AddPointToGraph((int)currentMoney);



        /////// Old calculations
        // currentCost = 10 - Mathf.Clamp(Mathf.Log(departments[(int)JOB_DEPARTMENT.FINANCE], 1.4f), 0.0f, 10.0f);
        // currentDemand = Mathf.Clamp(7 + Mathf.Log(departments[(int)JOB_DEPARTMENT.MARKETING], 1.1f), 0.0f, 1000.0f);
        // currentSupply = Mathf.Clamp(5 + Mathf.Log(departments[(int)JOB_DEPARTMENT.PRODUCTION], 1.1f), 0.0f, 1000.0f);

        // breakthroughChance = Mathf.Clamp(1 / (1 + Mathf.Pow(1.2f, 10 - departments[(int)JOB_DEPARTMENT.RESEARCH])), 0.0f, 1.0f);

        // currentPrice = Mathf.Clamp(60 + Mathf.Log(departments[(int)JOB_DEPARTMENT.QA], 1.1f), 0.0f, 1000.0f);

        // if (currentSupply > currentDemand) {
        //     profit = currentDemand * (currentPrice - currentCost) - (currentSupply - currentDemand) * currentCost;
        // } else {
        //     profit = currentSupply * (currentPrice - currentCost);
        // }
        
        // currentMoney += profit;
    }

    // Output a value given a num, and certain specifications
    // _middleNum is the median of all possible _num
    // _largetValue is the largest possible output
    // _steepness is how steep the sigmoid curve is, more = steeper
    float SigmoidValueFunc(float _middleNum, float _largestValue, float _steepness, float _num) {
        return _largestValue / (1 + Mathf.Pow(_steepness, (_middleNum - _num)));
    }
}
