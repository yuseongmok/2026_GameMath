using TMPro;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class DamageSimulator : MonoBehaviour
{
    public TextMeshProUGUI statusDisplay;
    public TextMeshProUGUI logDisplay;
    public TextMeshProUGUI resultDisplay;
    public TextMeshProUGUI rangeDisplay;

    private int level = 1;
    private float totalDamage = 0, baseDamage = 20f;
    private int attackCount = 0;

    private string weaponName;
    private float stdDevMult, critRate, critMult;

    private int critCount = 0;
    private int weakPointCount = 0;
    private int missCount = 0;
    private float maxDamage = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetWeapon(0);  //시작 시 단검 장착
    }

    private void ResetData()
    {
        totalDamage = 0;
        attackCount = 0;
        critCount = 0;
        weakPointCount = 0;
        missCount = 0;
        maxDamage = 0f;
    }

    public void SetWeapon(int id)
    {
        ResetData();
        if (id == 0)
        {
            SetStats("단검", 0.1f, 0.4f, 1.5f);
        }
        else if (id == 1)
        {
            SetStats("장검", 0.2f, 0.3f, 2.0f);
        }
        else
        {
            SetStats("도끼", 0.3f, 0.2f, 3.0f);
        }

        logDisplay.text = string.Format("{0} 장착!", weaponName);
        UpdateUI();
    }

    private void SetStats(string _name, float _stdDev, float _critRate, float _critMult)
    {
        weaponName = _name;
        stdDevMult = _stdDev;
        critRate = _critRate;
        critMult = _critMult;
    }

    public void LevelUp()
    {
        ResetData(); //레벨업 시 통계 초기화
        totalDamage = 0;
        attackCount = 0;
        level++;
        baseDamage = level * 20f;
        logDisplay.text = string.Format("레벨업! 현재 레벨: {0}", level);
        UpdateUI();
    }

    // 1000회 연속 공격 버튼용 함수
    public void OnAttackX1000()
    {
        for (int i = 0; i < 1000; i++)
        {
            ProcessAttack(false); // 로그 업데이트 없이 계산만 수행
        }
        logDisplay.text = "1000회 연속공격";
        UpdateUI();
    }

    // 단일 공격 버튼용 함수
    public void OnAttack()
    {
        ProcessAttack(true);
        UpdateUI();
    }
    private void ProcessAttack(bool updateLog)
    {
        float sd = baseDamage * stdDevMult;
        float normalDamage = GetNormalStdDevDamage(baseDamage, sd);

        float currentFinalDamage = 0;
        string specialLog = "";

        // 1. 명중 실패 판정 (-2 미만)
        if (normalDamage < baseDamage - (2 * sd))
        {
            missCount++;
            currentFinalDamage = 0;
            specialLog = "<color=gray>[명중 실패]</color> ";
        }
        else
        {
            // 2. 약점 공격 판정 (+2 초과)
            bool isWeakPoint = normalDamage > baseDamage + (2 * sd);
            if (isWeakPoint)
            {
                weakPointCount++;
                normalDamage *= 2.0f; // 데미지 2배
                specialLog += "<color=yellow>[약점 포착]</color> ";
            }

            // 3. 치명타 판정 (약점 공격과 별개로 계산)
            bool isCrit = Random.value < critRate;
            if (isCrit)
            {
                critCount++;
                currentFinalDamage = normalDamage * critMult;
                specialLog += "<color=red>[명치강타]</color> ";
            }
            else
            {
                currentFinalDamage = normalDamage;
            }
        }

        // 통계 누적
        attackCount++;
        totalDamage += currentFinalDamage;
        if (currentFinalDamage > maxDamage) maxDamage = currentFinalDamage;

        if (updateLog)
        {
            logDisplay.text = string.Format("{0}데미지: {1:F1}", specialLog, currentFinalDamage);
        }
    }
    private void UpdateUI()
    {
        statusDisplay.text = string.Format("Level: {0} / 무기: {1}\n기본 데미지: {2} / 치명타: {3}% (x{4})",
            level, weaponName, baseDamage, critRate * 100, critMult);

        // 정규분포 범위 설명 업데이트
        rangeDisplay.text = string.Format("판정 범위: 실패(<{0:F1}) | 약점(>{1:F1})",
            baseDamage - (2 * baseDamage * stdDevMult), baseDamage + (2 * baseDamage * stdDevMult));

        float dpa = attackCount > 0 ? totalDamage / attackCount : 0;
        resultDisplay.text = string.Format(
            "누적 데미지: {0:F1}\n" +
            "공격 횟수: {1} (크리: {2} / 약점: {3} / 실패: {4})\n" +
            "평균 DPA: {5:F2} / 최고 데미지: {6:F1}",
            totalDamage, attackCount, critCount, weakPointCount, missCount, dpa, maxDamage);
    }

    private float GetNormalStdDevDamage(float mean, float stdDev)
    {
        float u1 = 1.0f - Random.value;
        float u2 = 1.0f - Random.value;
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
        return mean + stdDev * randStdNormal;
    }
}
