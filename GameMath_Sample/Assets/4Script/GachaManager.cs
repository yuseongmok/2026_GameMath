using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GachaManager : MonoBehaviour
{
    public TextMeshProUGUI resultDisplay;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SimulateGachaSingle()
    {
        string res = Simulate();
        resultDisplay.text = "단차 결과: " + res;
    }

    public void SimulateGachaTenTime()
    {
        List<string> results = new List<string>();

        for (int i = 0; i < 9; i++)
        {
            results.Add(Simulate());
        }

        // 10회차 확정 로직 (A:S = 2:1 비율)
        float r2 = Random.value;
        string result2 = (r2 < 2f / 3f) ? "한조의 문신" : "한조의 활";
        results.Add(result2);

        resultDisplay.text = "10연차 결과:\n" + string.Join(", ", results);
    }

    string Simulate()
    {
        float r = Random.value;
        if (r < 0.4f) return "한조의 젖꼭지";
        else if (r < 0.7f) return "한조의 콧수염";
        else if (r < 0.9f) return "한조의 문신";
        else return "한조의 활";
    }

}
