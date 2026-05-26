using UnityEngine;

public class JunkratController : MonoBehaviour
{
    [Header("Mine Settings")]
    public GameObject minePrefab;       // 던질 지뢰 프리팹
    public Transform throwPoint;       // 지뢰가 스폰되어 날아갈 위치 (카메라 앞 등)
    public float throwForce = 15f;      // 지뢰를 던지는 세기

    private GameObject currentMine;    // 현재 필드에 설치된 지뢰 저장 변수

    void Update()
    {
        // 1. Left Shift 키를 누르면 지뢰 던지기 (설치)
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // 이미 지뢰가 있다면 기존 지뢰를 없애거나, 새로 던지지 못하게 막을 수 있습니다.
            // 여기서는 기존 지뢰가 있으면 파괴하고 새로 던지도록 처리했습니다.
            if (currentMine != null)
            {
                Destroy(currentMine);
            }

            ThrowMine();
        }

        // 2. 마우스 우클릭(Fire2) 시 지뢰가 존재하면 폭파
        if (Input.GetButtonDown("Fire2"))
        {
            if (currentMine != null)
            {
                // 지뢰 스크립트를 가져와서 Explode 함수 실행
                RemoteMine mineScript = currentMine.GetComponent<RemoteMine>();
                if (mineScript != null)
                {
                    mineScript.Explode();
                }

                // 폭파 완료 후 변수 비우기
                currentMine = null;
            }
        }
    }

    void ThrowMine()
    {
        if (minePrefab == null || throwPoint == null) return;

        // 지뢰 생성 및 앞으로 던지기
        currentMine = Instantiate(minePrefab, throwPoint.position, throwPoint.rotation);
        Rigidbody rb = currentMine.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // 던지는 순간 플레이어의 현재 이동 속도를 더해주면 훨씬 자연스럽습니다.
            Rigidbody playerRb = GetComponent<Rigidbody>();
            Vector3 inheritVelocity = (playerRb != null) ? playerRb.linearVelocity : Vector3.zero;

            rb.AddForce(throwPoint.forward * throwForce + inheritVelocity, ForceMode.VelocityChange);
        }
    }
}