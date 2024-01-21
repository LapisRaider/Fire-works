using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : SingletonBase<ChangeScene>
{
    public Animator m_ExitTransition;
    public string m_SceneName;
    public float m_TransitionDuration = 1.2f;

    public void NextScene()
    {
        GameManager.Instance.Refresh();
        StartCoroutine(ChangeSceneAnim());
    }
    public void FinalScene()
    {
        m_SceneName = "FinalScene";
        StartCoroutine(ChangeSceneAnim());
    }
    
    IEnumerator ChangeSceneAnim()
    {
        m_ExitTransition.SetTrigger("Exit");

        yield return new WaitForSeconds(m_TransitionDuration);

        SceneManager.LoadScene(m_SceneName);
    }
}
