using UnityEngine;

public class ResumeController : SingletonBase<ResumeController>
{
    [HideInInspector] public Resume m_CurrResumeFocused = null;

    [Header("Grabbin resume")]
    [SerializeField] private Collider m_TableCollider;
    [SerializeField] private float m_GrabResumeOffset = 10.0f;
    [HideInInspector] public float m_GrabYMinPos = 20.0f;

    [Header("ZoomResumePos")]
    public Transform m_FinalZoomPos;

    [Header("Trash Bin")]
    public Transform m_TrashBinPos;

    public override void Awake()
    {
        base.Awake();
        m_GrabYMinPos = m_TableCollider.bounds.max.y + m_GrabResumeOffset;
    }

    public bool SetCurrResumeFocused(Resume newResume)
    {
        if (m_CurrResumeFocused != null && !m_CurrResumeFocused.ZoomOut())
            return false;

        // succesffully changed
        m_CurrResumeFocused = newResume;
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        // If currently zoomed into a resume, get out
        if (m_CurrResumeFocused != null && Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out hit, 300.0f))
                return;

            if (hit.transform != m_CurrResumeFocused.transform && hit.transform.tag != "HireButton")
            {
                // if able to zoom out successfully
                if (m_CurrResumeFocused.ZoomOut())
                    m_CurrResumeFocused = null;
            }
        }
    }

}
