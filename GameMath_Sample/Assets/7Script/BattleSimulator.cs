using UnityEngine;
using TMPro;
using System.Text;

public class BattleSimulator : MonoBehaviour
{
    public TextMeshProUGUI resultText; // 화면에 표시될 텍스트
    
    // 시뮬레이션 데이터
    private int totalTurns = 0;
    private float currentRareChance = 5f; // 시작 확률 5%
    private int normalItems = 0;
    private int rareItems = 0;

    public void OnClickSimulation()
    {
        // 초기화
        totalTurns = 0;
        currentRareChance = 5f; 
        normalItems = 0;
        rareItems = 0;
        
        //레어 아이템 나올 때까지 진행
        while (rareItems < 1)
        {
            totalTurns++;
            
            // 확률 판정
            float randomVal = Random.Range(0f, 100f);
            
            if (randomVal <= currentRareChance)
            {
                rareItems++;
                Debug.Log($"<color=blue>{totalTurns}턴째에 레어 아이템 획득!</color>");
            }
            else
            {
                normalItems++;
                //턴마다 레어 아이템 획득 확률 5%씩 상승
                currentRareChance += 5f;
            }
        }

        //결과 출력 및 정렬
        DisplayResults();
    }

    void DisplayResults()
    {
        StringBuilder sb = new StringBuilder();
        
        sb.AppendLine("<color=#FFD700>전투 결과</color>");
        sb.AppendLine($"총 진행 턴 수 : {totalTurns}");
        sb.AppendLine($"최종 레어 확률 : {currentRareChance}%");
        sb.AppendLine("---------------------------");
        sb.AppendLine("<color=#FFD700>획득한 아이템</color>");
        sb.AppendLine($"일반 아이템 : {normalItems}개");
        sb.AppendLine($"레어 아이템 : {rareItems}개");
        
        // 콘솔 출력
        Debug.Log(sb.ToString());
        
        // 화면 Text 출력
        if (resultText != null)
            resultText.text = sb.ToString();
    }
}