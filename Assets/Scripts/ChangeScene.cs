using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public Animator m_ExitTransition;
    public string m_SceneName;
    public float m_TransitionDuration = 1.2f;

    public void NextScene()
    {
        StartCoroutine(ChangeSceneAnim());
    }
    
    IEnumerator ChangeSceneAnim()
    {
        m_ExitTransition.SetTrigger("Exit");

        yield return new WaitForSeconds(m_TransitionDuration);

        SceneManager.LoadScene(m_SceneName);
    }
}
