using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Slider hpSlider;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private readonly float smallJumpDistance = 3.0f;
    public bool isJumping = false;
    private float jumpCooldown = 0f;
    private readonly float jumpCooldownTime = 1.0f;
    public int hp = 100;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isJumping && jumpCooldown <= float.Epsilon)
        {
                int random_value = Random.Range(0, 3);
                if (random_value == 0) 
                    StartCoroutine(JumpInPlace());
                else
                    StartCoroutine(SmallJump());    
        }
        if (jumpCooldown > float.Epsilon && isJumping == false) jumpCooldown -= Time.deltaTime;
        if (jumpCooldown < 0f) Debug.Log("Jump cooldown is over!");
        jumpCooldown = jumpCooldown <= float.Epsilon ? 0f : jumpCooldown;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            hp--;
            StartCoroutine(DamageEffect());
            hpSlider.value = hp / 100f;
            if (hp < 1)
            {
                Debug.Log("Enemy is dead!");
                Destroy(gameObject);
            }
        }
    }
    IEnumerator DamageEffect()
    {
        audioSource.PlayOneShot(audioSource.clip, 0.25f);
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    IEnumerator SmallJump()
    {
        // 플레이어 방향으로 이동할 벡터 설정
        Vector2 startPosition = transform.position;
        Vector2 targetPosition = startPosition + smallJumpDistance * (Vector2)(playerTransform.position - (Vector3)startPosition).normalized;

        // 점프 관련 변수
        isJumping = true;
        jumpCooldown = jumpCooldownTime;
        float jumpHeight = 1.0f; // 점프 최고 높이
        float totalJumpTime = 0.5f; // 점프 지속 시간
        float elapsedTime = 0f;

        while (elapsedTime < totalJumpTime)
        {
            float t = elapsedTime / totalJumpTime; // 진행 비율 (0~1)
            
            // 수평 이동 (선형 보간)
            Vector2 currentPosition = Vector2.Lerp(startPosition, targetPosition, t);

            // 점프 궤적 적용 (포물선 효과)
            currentPosition.y += jumpHeight * (4 * t * (1 - t));

            transform.position = currentPosition;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition; // 마지막 위치 보정
        Fire();
        isJumping = false;
    }
    IEnumerator JumpInPlace() {
        // 점프 관련 변수
        isJumping = true;
        jumpCooldown = jumpCooldownTime;
        float jumpHeight = 0.6f; // 점프 최고 높이
        float totalJumpTime = 0.25f; // 점프 지속 시간
        float elapsedTime = 0f;

        Vector2 startPosition = transform.position;
        while (elapsedTime < totalJumpTime)
        {
            float t = elapsedTime / totalJumpTime; // 진행 비율 (0~1)
            
            // 수평 이동 (선형 보간)
            Vector2 currentPosition = startPosition;

            // 점프 궤적 적용 (포물선 효과)
            currentPosition.y += jumpHeight * (4 * t * (1 - t));

            transform.position = currentPosition;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = startPosition; // 마지막 위치 보정
        LargeFire();
        elapsedTime = 0f;
        while (elapsedTime < totalJumpTime)
        {
            float t = elapsedTime / totalJumpTime; // 진행 비율 (0~1)
            
            // 수평 이동 (선형 보간)
            Vector2 currentPosition = startPosition;

            // 점프 궤적 적용 (포물선 효과)
            currentPosition.y += jumpHeight * (4 * t * (1 - t));

            transform.position = currentPosition;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = startPosition; // 마지막 위치 보정
        LargeFire();
        elapsedTime = 0f;
        while (elapsedTime < totalJumpTime)
        {
            float t = elapsedTime / totalJumpTime; // 진행 비율 (0~1)
            
            // 수평 이동 (선형 보간)
            Vector2 currentPosition = startPosition;

            // 점프 궤적 적용 (포물선 효과)
            currentPosition.y += jumpHeight * (4 * t * (1 - t));

            transform.position = currentPosition;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = startPosition; // 마지막 위치 보정
        LargeFire();
        isJumping = false;
    }
    private void Fire() {
        // Fire 12 bullets in a circle
        for (int i = 0; i < 12; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.direction = Quaternion.Euler(0, 0, i * 30) * Vector2.right;
            bulletScript.isTriggered = true;
        }
    }
    private void LargeFire() {
        float offset = Random.Range(0, 10);
        // Fire 36 bullets in a circle
        for (int i = 0; i < 36; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.direction = Quaternion.Euler(0, 0, offset + i * 10) * Vector2.right;
            bulletScript.isTriggered = true;
        }
    }
    



}
