using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ResumeManager : SingletonBase<ResumeManager>
{
    [Header("Resumes")]
    public GameObject m_ResumePrefab;
    public int m_ResumeObjPoolerSpawnCount = 10;

    [Header("Spawning")]
    public Collider m_TableSpawnBounds;
    public Collider m_SpawnBounds;
    public Vector2 m_MinMaxFlySpeed = new Vector2(3.0f, 7.0f);
    private Vector3 m_InitialSpawnPos;

    private Vector3 m_ResumeSize; 
    private List<Vector3> m_ResumeSpawnPositions = new List<Vector3>();
    private List<Resume> m_Resumes = new List<Resume>();


    [Header("Spawn math")]
    // Average number of applicants in a batch
    public int average;

    // Chance for the special event to arrive [ Superior HR applicant to replace the player ]
    public float replacementChance;


    void AddResumesToPooler(int spawnNo)
    {
        for (int i = 0; i < spawnNo; ++i)
        {
            GameObject resumeObj = Instantiate(m_ResumePrefab, m_ResumePrefab.transform.position, m_ResumePrefab.transform.rotation);
            resumeObj.SetActive(false);
            resumeObj.transform.SetParent(transform, false);
            m_Resumes.Add(resumeObj.GetComponent<Resume>());
        }
    }
    Resume GetInActiveResume()
    {
        foreach (Resume resume in m_Resumes)
        {
            if (resume.gameObject.activeSelf)
                continue;

            return resume;
        }

        AddResumesToPooler(m_ResumeObjPoolerSpawnCount);
        return GetInActiveResume();
    }

    Candidate GenerateCandidateData()
    {
        //TODO: generate data
        return null;
    }

    public void SpawnBatch(int batchAmt)
    {
        // cache new spawn positions so it does not have to be recalculated
        if (m_ResumeSpawnPositions.Count < batchAmt)
        {
            int spawnPosToBeAdded = batchAmt - m_ResumeSpawnPositions.Count;
            for (int i = 0; i < spawnPosToBeAdded; ++i)
            {
                Vector3 lastPos = m_ResumeSpawnPositions.Last();
                Vector3 newSpawnPos = Vector3.zero;
                if (m_SpawnBounds.bounds.max.x < lastPos.x + m_ResumeSize.x * 1.5)
                {
                    newSpawnPos = new Vector3(m_InitialSpawnPos.x, lastPos.y, lastPos.z + m_ResumeSize.z);
                }
                else
                {
                    newSpawnPos = lastPos + new Vector3(m_ResumeSize.x, 0, 0);
                }

                m_ResumeSpawnPositions.Add(newSpawnPos);
            }
        }

        for (int i = 0; i < batchAmt; ++i)
        {
            Resume resume = GetInActiveResume();
            Vector3 landPos = new Vector3(
                m_ResumeSpawnPositions[i].x + Random.Range(-m_ResumeSize.x, m_ResumeSize.x),
                m_TableSpawnBounds.bounds.center.y,
                Random.Range(m_TableSpawnBounds.bounds.min.z, m_TableSpawnBounds.bounds.max.z)
                );

            // randomize speed
            float randomSpeed = Random.Range(m_MinMaxFlySpeed.x, m_MinMaxFlySpeed.y);
            resume.gameObject.SetActive(true);
            resume.Initialize(GenerateCandidateData(), m_ResumeSpawnPositions[i], landPos, randomSpeed);
        }
    }

    void NewBatch() {
        Debug.Log("NewBatch");

        // Check if replacement will be spawning along with the batch
        if (Random.Range(0.0f, 1.0f) <= replacementChance) {
            // TODO : Spawn the special resume
            Debug.Log("Special resume");
        }

        int numToSpawn = CalculateNumberToSpawn(GameManager.Instance.departments[(int)JOB_DEPARTMENT.HR]);
        SpawnBatch(numToSpawn);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.onNewMonth += NewBatch;

        m_ResumeSize = m_ResumePrefab.GetComponent<Renderer>().bounds.size;
        m_InitialSpawnPos = new Vector3(m_SpawnBounds.bounds.min.x + m_ResumeSize.x * 0.5f, 
            m_SpawnBounds.bounds.center.y, 
            m_SpawnBounds.bounds.min.z + m_ResumeSize.z * 0.5f);
        m_ResumeSpawnPositions.Add(m_InitialSpawnPos);
    }

    int CalculateNumberToSpawn(int _num) {
        int n = 0;
        int i = 1;
        for (; n < _num; i++) {
            n += i;
        }

        float r = Random.Range(0.0f, 1.0f);
        if (r < 0.333f) {
            return i;
        } else if (r > 0.666f){
            return 2 + i;
        } else {
            return 1 + i;
        }
    }
}
