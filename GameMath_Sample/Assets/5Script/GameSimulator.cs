using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameSimulator : MonoBehaviour
{
    [Header("Player & Enemy Settings")]
    public float playerAtk = 30f;
    public float critChance = 0.3f; 
    public float enemyMaxHp = 300f;
    private float currentEnemyHp;

    [Header("Item Drop Rates (Initial)")]
    private float[] baseRates = { 0.50f, 0.30f, 0.15f, 0.05f };
    private float[] currentRates = new float[4];
    private string[] rankNames = { "한조의 왼쪽 젖꼭지", "한조의 문신", "한조각", "전설의레전드" };

    [Header("UI References")]
    public TextMeshProUGUI statusText;    // 체력 및 치명타 정보
    public TextMeshProUGUI probabilityText; // 현재 아이템 확률 정보
    public TextMeshProUGUI logText;       // 획득 로그

    private int totalHits = 0;
    private int critHits = 0;

    void Awake()
    {
        ResetEnemy();
        ResetRates();
    }

    // 초기 확률 설정
    void ResetRates()
    {
        for (int i = 0; i < baseRates.Length; i++)
            currentRates[i] = baseRates[i];
        UpdateUI();
    }

    // 적 리스폰
    void ResetEnemy()
    {
        currentEnemyHp = enemyMaxHp;
        UpdateUI();
    }

    public void OnAttackButton()
    {
        bool isCrit = RollCrit();
        float damage = isCrit ? playerAtk * 2 : playerAtk;

        currentEnemyHp -= damage;

        string critMsg = isCrit ? "<color=red>명치강타! </color>" : "일반 공격";
        logText.text = $"데미지 {damage} 입힘 ({critMsg})";

        if (currentEnemyHp <= 0)
        {
            currentEnemyHp = 0;
            DropItem();
            ResetEnemy();
        }

        UpdateUI();
    }

    // 기존 CriticalManager 로직 활용
    public bool RollCrit()
    {
        totalHits++;
        float currentRate = (totalHits > 1) ? (float)critHits / totalHits : 0f;

        bool isCrit = false;

        if (currentRate < critChance && (float)(critHits + 1) / totalHits <= critChance)
        {
            isCrit = true;
        }
        else if (currentRate > critChance && (float)critHits / totalHits >= critChance)
        {
            isCrit = false;
        }
        else
        {
            isCrit = Random.value < critChance;
        }

        if (isCrit) critHits++;
        return isCrit;
    }

    // 아이템 드랍 로직 (전설 확률 보정 포함)
    void DropItem()
    {
        float rand = Random.value;
        float cumulative = 0f;
        int selectedIndex = -1;

        for (int i = 0; i < currentRates.Length; i++)
        {
            cumulative += currentRates[i];
            if (rand <= cumulative)
            {
                selectedIndex = i;
                break;
            }
        }

        if (selectedIndex == 3) // 전설 획득 성공
        {
            logText.text = "<color=yellow>★ 전설의레전드 획득! ★</color>";
            ResetRates(); // 확률 초기화
        }
        else // 전설 획득 실패
        {
            logText.text = $"{rankNames[selectedIndex]} 아이템 획득.";
            AdjustRates(); // 확률 보정
        }
    }

    // 전설 확률 1.5% 증가 및 나머지 0.5% 감소
    void AdjustRates()
    {
        currentRates[3] += 0.015f; // 전설 +1.5%
        for (int i = 0; i < 3; i++)
        {
            currentRates[i] -= 0.005f; 
            if (currentRates[i] < 0) currentRates[i] = 0; // 마이너스 방지
        }
    }

    void UpdateUI()
    {
        // 상단 상태 정보
        statusText.text = $"한조 HP: {currentEnemyHp} / {enemyMaxHp}\n" +
                          $"연타: {totalHits} | 명치강타: {critHits}\n" +
                          $"치명타 보정: {(totalHits > 0 ? ((float)critHits / totalHits * 100f).ToString("F1") : "0")}%";

        // 확률 정보
        probabilityText.text = "<b>현재 드랍 확률</b>\n" +
                               $"한조의 젖꼭지: {currentRates[0] * 100f:F1}% | " +
                               $"한조의 문신: {currentRates[1] * 100f:F1}%\n" +
                               $"한조각: {currentRates[2] * 100f:F1}% | " +
                               $"<color=yellow>전설의레전드: {currentRates[3] * 100f:F1}%</color>";
    }
}