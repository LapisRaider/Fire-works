using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class ResumeUI
{
    public TextMeshPro m_Name;
    public TextMeshPro m_CandidateType;
    public TextMeshPro m_Description;

    [Header("Image")]
    public PeopleImgData.ImgData m_CurrImgData;
    public PeopleImgData m_ImgDataTotal;

    public void Initialize(Candidate data)
    {
        m_Name.text = ResumeDescriptionData.Instance.m_Names[UnityEngine.Random.Range(0, ResumeDescriptionData.Instance.m_Names.Count)];

        List<string> expertise = ResumeDescriptionData.Instance.jobs[(int)data.m_Department].m_expertiseLevels[data.ExpertiseLevel];
        m_Description.text = expertise[UnityEngine.Random.Range(0, expertise.Count)];
        m_CandidateType.text = Candidate.DEPARTMENT_TEXT[data.m_Department];

        if (m_CurrImgData != null)
        {
            if (m_CurrImgData.m_Animal != null)
            {
                m_CurrImgData.m_Animal.SetActive(false);
            }

            if (m_CurrImgData.m_Clothe != null)
            {
                m_CurrImgData.m_Clothe.SetActive(false);
            }
        }

        m_CurrImgData = m_ImgDataTotal.m_People[data.m_AnimalPhotoIndex];
        if (m_CurrImgData != null)
        {
            if (m_CurrImgData.m_Animal != null)
            {
                m_CurrImgData.m_Animal.SetActive(true);
            }

            if (m_CurrImgData.m_Clothe != null)
            {
                m_CurrImgData.m_Clothe.SetActive(data.m_HaveCloth);
            }
        }
    }
}
