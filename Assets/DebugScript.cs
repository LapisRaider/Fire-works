using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugScript : MonoBehaviour
{
    public TMP_Text moneyText;
    void Update() {
        moneyText.text = GameManager.Instance.currentMoney.ToString("R");
    }

    public void HireHR() {
        GameManager.Instance.Hire(100, JOB_DEPARTMENT.HR);
    }
    public void HireFin() {
        GameManager.Instance.Hire(100, JOB_DEPARTMENT.FINANCE);
    }
    public void HireMar() {
        GameManager.Instance.Hire(100, JOB_DEPARTMENT.MARKETING);
    }
    public void HirePro() {
        GameManager.Instance.Hire(100, JOB_DEPARTMENT.PRODUCTION);
    }
    public void HireRes() {
        GameManager.Instance.Hire(100, JOB_DEPARTMENT.RESEARCH);
    }
    public void HireQA() {
        GameManager.Instance.Hire(100, JOB_DEPARTMENT.QA);
    }
    public void HireSec() {
        GameManager.Instance.Hire(100, JOB_DEPARTMENT.SECURITY);
    }
}
