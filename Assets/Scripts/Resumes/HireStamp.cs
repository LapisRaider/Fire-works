using UnityEngine;

public class HireStamp : MonoBehaviour
{
    public GameObject m_HoverStamp;
    public GameObject m_Stamp;
    public Collider m_Collider;

    public Resume m_ResumeAttachedTo;

    [HideInInspector] public bool m_AllowHire = false;
    private bool m_IsClicked = false;

    private void Awake()
    {
        m_Collider = GetComponent<Collider>();
        SetAllowHire(false);
    }
    public void SetAllowHire(bool allowHire)
    {
        m_AllowHire = allowHire;
        m_Collider.enabled = allowHire;
    }

    public void ResetStamp()
    {
        m_IsClicked = false;
        m_HoverStamp.SetActive(false);
        m_Stamp.SetActive(false);
        SetAllowHire(false);
    }

    private void OnMouseEnter()
    {
        if (!m_Stamp.activeSelf && m_AllowHire)
            m_HoverStamp.SetActive(true);
    }

    private void OnMouseExit()
    {
        m_HoverStamp.SetActive(false);
        m_IsClicked = false;
    }

    private void OnMouseDown()
    {
        if (m_AllowHire)
            m_IsClicked = true;
    }

    private void OnMouseUp()
    {
        if (m_IsClicked && m_AllowHire)
        {
            m_Stamp.SetActive(true);
            m_HoverStamp.SetActive(false);

            m_ResumeAttachedTo.Hired();
        }
    }
}
