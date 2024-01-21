using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : SingletonBase<ChangeScene>
{
    public Animator m_ExitTransition;
    public string m_SceneName;
    public float m_TransitionDuration = 1.2f;
    public bool m_isDoneTransition = false;

    public void NextScene()
    {
        StartCoroutine(ChangeSceneAnim());
    }
    public void FinalScene()
    {
        m_SceneName = "FinalScene";
        StartCoroutine(ChangeSceneAnim());
    }
    
    public void RestartGame()
    {
        m_SceneName = "MainMenu";
        StartCoroutine(ChangeSceneAnim());
    }
    IEnumerator ChangeSceneAnim()
    {
        m_ExitTransition.SetTrigger("Exit");

        yield return new WaitForSeconds(m_TransitionDuration);

        if (m_SceneName == "GameScene") {
            if (GameManager.Instance.restart) {
                GameManager.Instance.InitValues();
            } else {
                GameManager.Instance.Refresh();
            }
        }
        SceneManager.LoadScene(m_SceneName);
        
    }
}
