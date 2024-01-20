using System;
using TMPro;

[Serializable]
public class ResumeUI
{
    public TextMeshPro m_Name;
    public TextMeshPro m_CandidateType;
    public TextMeshPro m_Description;

    public void Initialize(Candidate data)
    {
        m_Name.text = data.Name;
        m_Description.text = data.Description;
        m_CandidateType.text = Candidate.DEPARTMENT_TEXT[data.m_Department];
    }
}
