using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public string[] bossStrings;
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
    public float currentDemand;
    public float currentProfits;
    public float currentSalaries;

    public int numBadHires;

    // Misc events variables
    public delegate void OnNewMonth();
    
    public static event OnNewMonth onNewMonth = null;

    [Header("Animation")]
    public Animator m_ExitTransition;
    public string m_SceneName;
    public float m_TransitionDuration = 1.2f;

    public bool boss;
    public bool restart;

    // Start is called before the first frame update
    void Start()
    {
        InitValues();   
    }

    public void Refresh() {
        boss = false;
        restart = false;
        currentMonth = 0;
        quarterMonthTime = 2.5f;
        quarterMonthTimer = 0.0f;

        TabletManager.Instance.InitPieChart(departments);
    }

    void InitValues() {
        currentYear = 0;

        departments[(int)JOB_DEPARTMENT.HR] = 1;
        departments[(int)JOB_DEPARTMENT.MARKETING] = 0;
        departments[(int)JOB_DEPARTMENT.PRODUCTION] = 1;
        departments[(int)JOB_DEPARTMENT.QA] = 0;
        departments[(int)JOB_DEPARTMENT.FINANCE] = 1;
        departments[(int)JOB_DEPARTMENT.RESEARCH] = 0;
        departments[(int)JOB_DEPARTMENT.SECURITY] = 0;
        
        currentMoney = 1000;
        currentSalaries = 50;
        currentMarketSalary = 25.0f;
        numBadHires = 0;

        Refresh();
    }

    // Update is called once per frame
    void Update()
    {
        if (boss) {
            return;
        }

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
        currentYear++;

        if (currentYear == 4) {
            restart = true;
            bossStrings = new string[3]{ "It's a tadpole's turn of events but your lilypad days here are over.", "But worry not! I've penned a croak-tastic recommendation to spice up your next jump.", "Keep on hopping and making waves!" };
        }

        if (currentMoney > 3000) {
            bossStrings = new string[3]{ "Jumpin' jellybugs!", "Your outstanding dedication and hard work have sent our sales soaring to new heights!", "Keep leaping forward, and let's ride this wave of success together!" };
        } else if (currentMoney > 1000) {
            bossStrings = new string[3]{ "Hoppy news!", "Your commitment to improvement hasn't gone unnoticed, and I appreciate your perseverance!", "Keep paddling, keep croaking, and soon you'll be making waves in our pond." };
        } else {
            bossStrings = new string[3]{ "I've noticed a bit of pond stagnation lately.", "We're in a pond, not a snooze-fest!", "Enough with the sluggishness it's time to leap!" };
        }
        boss = true;
        ChangeScene.Instance.NextScene();
    }

    public void Hire(float _hiringCost, JOB_DEPARTMENT _department, int _level) {
        if (_level == 0) { 
            numBadHires++; 
            if (numBadHires == 10) {
                // Too many bad apples, CEO not happy
                bossStrings = new string[3]{ "This is unacceptable!", "Our once-harmonious swamp is now overrun with the cacophony of bad apples!", "Consider this your untimely departure from our froggy realm!" };        
                boss = true;
                restart = true;
                ChangeScene.Instance.NextScene();
            }
        }


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
                if (departments[(int)JOB_DEPARTMENT.MARKETING] + currentDemand - departments[(int)JOB_DEPARTMENT.PRODUCTION] >= 3) {
                    currentProfits -= currentSalaries * ((_level - 1) * -0.2f + 1.0f);
                } else if (departments[(int)JOB_DEPARTMENT.MARKETING] + currentDemand - departments[(int)JOB_DEPARTMENT.PRODUCTION] >= 0) {
                    currentProfits += 2.5f * currentSalaries * ((_level - 1) * 0.2f + 1.0f);
                    if (currentProfits > 2.5f * currentSalaries) {
                        currentProfits = 2.5f * currentSalaries;
                    }
                } else {
                    currentProfits += 5.0f * currentSalaries * ((_level - 1) * 0.2f + 1.0f);
                    if (currentProfits > 5.0f * currentSalaries) {
                        currentProfits = 5.0f * currentSalaries;
                    }
                }
            } break;
            case JOB_DEPARTMENT.PRODUCTION: {
                if (departments[(int)JOB_DEPARTMENT.PRODUCTION] - currentDemand - departments[(int)JOB_DEPARTMENT.MARKETING] >= 3) {
                    currentProfits -= currentSalaries * ((_level - 1) * -0.2f + 1.0f);
                } else if (departments[(int)JOB_DEPARTMENT.PRODUCTION] - currentDemand - departments[(int)JOB_DEPARTMENT.MARKETING] >= 0) {
                    currentProfits += 2.5f * currentSalaries * ((_level - 1) * 0.2f + 1.0f);
                    if (currentProfits > 2.5f * currentSalaries) {
                        currentProfits = 2.5f * currentSalaries;
                    }
                } else {
                    currentProfits += 5.0f * currentSalaries * ((_level - 1) * 0.2f + 1.0f);
                    if (currentProfits > 5.0f * currentSalaries) {
                        currentProfits = 5.0f * currentSalaries;
                    }
                }
            } break;
            case JOB_DEPARTMENT.QA: {
                if (departments[(int)JOB_DEPARTMENT.MARKETING] + departments[(int)JOB_DEPARTMENT.PRODUCTION] > 2 * departments[(int)JOB_DEPARTMENT.QA]) {
                    currentProfits += 2.0f * currentSalaries * ((_level - 1) * 0.2f + 1.0f);
                    if (currentProfits > 2.5f * currentSalaries) {
                        currentProfits = 2.5f * currentSalaries;
                    }
                }
            } break;
            case JOB_DEPARTMENT.FINANCE: {
                if (currentSalaries < 0) {
                    currentSalaries *= 0.67f * ((_level - 1) * 0.2f + 1.0f);
                } else if (currentProfits < currentSalaries * 4) {
                    currentProfits *= 1.67f * currentProfits * ((_level - 1) * 0.2f + 1.0f);
                }
            } break;
            case JOB_DEPARTMENT.RESEARCH: {
            } break;
            case JOB_DEPARTMENT.SECURITY: {
            } break;

            default: break;
        }
    }
    public void Hire(Candidate _candidate) {
        Hire(_candidate.Salary, _candidate.m_Department, _candidate.ExpertiseLevel);
    }

    void UpdateFinances() {
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
                if (currentMonth >= 2) {
                    currentDemand -= 0.25f;
                }
            } break;
            case 2:
            {
                if (currentMonth >= 1) {
                    currentDemand -= 0.25f;
                }
                if (currentMonth >= 3) {
                    currentMarketSalary += 25.0f;
                }
            } break;
            case 3:
            {
                if (currentMonth >= 1) {
                    currentDemand -= 0.25f;
                }
                if (currentMonth >= 1) {
                    currentMarketSalary += 25.0f;
                }
            } break;
            case 4:
            {
                currentMarketSalary += 25.0f;
                currentDemand -= 0.25f;                
            } break;
            default: break;
        }
        
        currentProfits -= currentSalaries;
        if (currentProfits < 0) { currentProfits = 0; }
        currentMoney += currentProfits - currentSalaries;
        
        TabletManager.Instance.AddPointToGraph((int)currentMoney);

        if (currentMoney <= 0) {
            // No more money, CEO not happy
            bossStrings = new string[3]{ "Listen up bucko.", "Your financial acrobatics have turned our thriving swamp into a muck-filled disaster!", "Consider this your leap of shame and get out of my office!" };    
            boss = true;
            restart = true;
            ChangeScene.Instance.NextScene();
        }

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
