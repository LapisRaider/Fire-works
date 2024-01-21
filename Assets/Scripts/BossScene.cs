using System.Collections;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class BossScene : SingletonBase<BossScene>
{
    public List<string> TextToSay = new List<string>();
    private string CurrStrSaid = "";
    public TextMeshProUGUI text = null;
    public float TypeSpeed = 0.2f;
    private int CurrIndex = 0;
    private int CurrSentence = 0;

    private bool isTypeing = false;

    public void SayText()
    {
        StartCoroutine(SayTextIterate());
    }

    public void NextSentence()
    {
        if (isTypeing)
            return;

        ++CurrSentence;
        if (CurrSentence >= TextToSay.Count)
            return;

        CurrIndex = 0;
        CurrStrSaid = "";
        text.SetText("");

        StartCoroutine(SayTextIterate());
    }

    IEnumerator SayTextIterate()
    {
        isTypeing = true;
        while (CurrStrSaid.Length != TextToSay[CurrSentence].Length)
        {
            CurrStrSaid += TextToSay[CurrSentence][CurrIndex];
            text.SetText(CurrStrSaid);
            ++CurrIndex;

            yield return new WaitForSeconds(TypeSpeed);
        }

        isTypeing = false;
    }
}
