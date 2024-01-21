using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Candidate
{
    public int ExpertiseLevel;
    public float Salary;
    public string Name;
    public string Description;
    public JOB_DEPARTMENT m_Department;

    public bool m_HaveCloth;
    public int m_AnimalPhotoIndex;

    public static Dictionary<JOB_DEPARTMENT, string> DEPARTMENT_TEXT = new Dictionary<JOB_DEPARTMENT, string>()
    {
        { JOB_DEPARTMENT.HR, "Hr" },
        { JOB_DEPARTMENT.MARKETING, "Marketing" },
        { JOB_DEPARTMENT.PRODUCTION, "Production" },
        { JOB_DEPARTMENT.QA, "QA" },
        { JOB_DEPARTMENT.FINANCE, "Finance" },
        { JOB_DEPARTMENT.RESEARCH, "Research" },
        { JOB_DEPARTMENT.SECURITY, "Security" }
    };
}

