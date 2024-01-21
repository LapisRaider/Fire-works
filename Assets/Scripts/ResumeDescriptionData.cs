using System.Collections.Generic;
using UnityEngine;

public class ResumeDescriptionData : MonoBehaviour
{
    public List<string> m_Names  = new List<string>();


    [TextArea(15,20)]public List<string> m_ExpertHr = new List<string>();
    [TextArea(15,20)]public List<string> m_MediumHr = new List<string>();
    [TextArea(15,20)]public List<string> m_BadHr = new List<string>();
    
    [TextArea(15,20)]public List<string> m_ExpertMarketing = new List<string>();
    [TextArea(15,20)]public List<string> m_MediumMarketing = new List<string>();
    [TextArea(15,20)]public List<string> m_BadMarketing = new List<string>();
    
    [TextArea(15,20)]public List<string> m_ExpertQA = new List<string>();
    [TextArea(15,20)]public List<string> m_MediumQA = new List<string>();
    [TextArea(15,20)]public List<string> m_BadQA = new List<string>();
    
    [TextArea(15,20)]public List<string> m_ExpertFINANCE = new List<string>();
    [TextArea(15,20)]public List<string> m_MediumFINANCE = new List<string>();
    [TextArea(15,20)]public List<string> m_BadFINANCE = new List<string>();
    
    [TextArea(15,20)]public List<string> m_ExpertRESEARCH = new List<string>();
    [TextArea(15,20)]public List<string> m_MediumRESEARCH = new List<string>();
    [TextArea(15,20)]public List<string> m_BadRESEARCH = new List<string>();
    
    [TextArea(15,20)]public List<string> m_ExpertSECURITY = new List<string>();
    [TextArea(15,20)]public List<string> m_MediumSECURITY = new List<string>();
    [TextArea(15, 20)] public List<string> m_BadSECURITY = new List<string>();
}
