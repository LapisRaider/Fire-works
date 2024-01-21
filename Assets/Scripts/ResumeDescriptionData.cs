using System.Collections.Generic;
using UnityEngine;

public class ResumeDescriptionData : SingletonBase<ResumeDescriptionData>
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

    [TextArea(15, 20)] public List<string> m_ExpertPRODUCTION = new List<string>();
    [TextArea(15, 20)] public List<string> m_MediumPRODUCTION = new List<string>();
    [TextArea(15, 20)] public List<string> m_BadPRODUCTION = new List<string>();


    public class Expertise
    {
        public List<List<string>> m_expertiseLevels = new List<List<string>>();
    }

    public List<Expertise> jobs = new List<Expertise>();

    public override void Awake()
    {
        base.Awake();
        Expertise hr = new Expertise();
        hr.m_expertiseLevels.Add(m_BadHr);
        hr.m_expertiseLevels.Add(m_MediumHr);
        hr.m_expertiseLevels.Add(m_ExpertHr);


        jobs.Add(hr);

        Expertise marketing = new Expertise();
        marketing.m_expertiseLevels.Add(m_BadMarketing);
        marketing.m_expertiseLevels.Add(m_MediumMarketing);
        marketing.m_expertiseLevels.Add(m_ExpertMarketing);
        jobs.Add(marketing);

        Expertise production = new Expertise();
        production.m_expertiseLevels.Add(m_BadPRODUCTION);
        production.m_expertiseLevels.Add(m_MediumPRODUCTION);
        production.m_expertiseLevels.Add(m_ExpertPRODUCTION);
        jobs.Add(production);

        Expertise qa = new Expertise();
        qa.m_expertiseLevels.Add(m_BadQA);
        qa.m_expertiseLevels.Add(m_MediumQA);
        qa.m_expertiseLevels.Add(m_ExpertQA);
        jobs.Add(qa);

        Expertise finance = new Expertise();
        finance.m_expertiseLevels.Add(m_BadFINANCE);
        finance.m_expertiseLevels.Add(m_MediumFINANCE);
        finance.m_expertiseLevels.Add(m_ExpertFINANCE);
        jobs.Add(finance);

        Expertise research = new Expertise();
        research.m_expertiseLevels.Add(m_BadRESEARCH);
        research.m_expertiseLevels.Add(m_MediumRESEARCH);
        research.m_expertiseLevels.Add(m_ExpertRESEARCH);
        jobs.Add(research);

        Expertise security = new Expertise();
        security.m_expertiseLevels.Add(m_BadSECURITY);
        security.m_expertiseLevels.Add(m_MediumSECURITY);
        security.m_expertiseLevels.Add(m_ExpertSECURITY);
        jobs.Add(security);
    }
}
