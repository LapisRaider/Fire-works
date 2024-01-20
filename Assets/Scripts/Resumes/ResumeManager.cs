using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeManager : SingletonBase<ResumeManager>
{
    [Header("Resumes")]
    public GameObject m_ResumePrefab;
    public int m_ResumeObjPoolerSpawnCount = 10;
    public Bounds m_AreaResumesWillFlyTo;
    public Vector3 m_SpawnPos;


    private List<Vector3> m_ResumeSpawnPositions;
    private List<Resume> m_Resumes;
    


    // Average number of applicants in a batch
    public int average;

    // Chance for the special event to arrive [ Superior HR applicant to replace the player ]
    public float replacementChance;


    void AddResumesToPooler(int spawnNo)
    {
        for (int i = 0; i < spawnNo; ++i)
        {
            GameObject resumeObj = Instantiate(m_ResumePrefab);
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

    void CreateResume()
    {
        Resume resume = GetInActiveResume();

        //TODO: generate data
        resume.gameObject.SetActive(true);
    }

    void NewBatch() {
        Debug.Log("NewBatch");

        // Check if replacement will be spawning along with the batch
        if (Random.Range(0.0f, 1.0f) <= replacementChance) {
            // TODO : Spawn the special resume
            Debug.Log("Special resume");
        }

        int numToSpawn = CalculateNumberToSpawn(GameManager.Instance.departments[(int)JOB_DEPARTMENT.HR]);
        // TODO : spawn batch of X resumes
        Debug.Log(numToSpawn);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.onNewMonth += NewBatch;
    }

    // Update is called once per frame
    void Update()
    {
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
