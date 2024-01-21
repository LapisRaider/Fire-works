using System.Collections;
using TMPro;
using UnityEngine;

public class BossScene : SingletonBase<BossScene>
{
    public string TextToSay = "fhuiwerhuiferhuifer";
    private string CurrStrSaid = "";
    public TextMeshProUGUI text = null;
    public float TypeSpeed = 0.2f;
    private int CurrIndex = 0;

    public void SayText()
    {
        StartCoroutine(SayTextIterate());
    }

    IEnumerator SayTextIterate()
    {
        while (CurrStrSaid.Length != TextToSay.Length)
        {
            CurrStrSaid += TextToSay[CurrIndex];
            text.SetText(CurrStrSaid);
            ++CurrIndex;

            yield return new WaitForSeconds(TypeSpeed);
        }
    }
}
