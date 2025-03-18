using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private InputAction move;
    private InputAction jump;
    private SpriteRenderer spriteRenderer;
    public int hp = 3;
    private bool isJumping = false;
    private float jumpingCooldown = 0f;
    private readonly float jumpingCooldownTime = 3.0f;
    private readonly float jumpingSpeed = 20.0f;
    private float damageCooldown = 1.0f;
    private readonly float damageCooldownTime = 1.0f;
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private Transform spiritTransform;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private GameObject GameOverScreen;
    private AudioSource audioSource;
    private readonly float speed = 3.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        move = inputActions.FindActionMap("Player").FindAction("Move");
        jump = inputActions.FindActionMap("Player").FindAction("Jump");
        spriteRenderer = GetComponent<SpriteRenderer>();
        hpText.text = hp.ToString();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = move.ReadValue<Vector2>();
        transform.position += speed * Time.deltaTime * (Vector3)direction;
        if (jump.triggered)
        {
            StartCoroutine(Jump());
        }
        if (!isJumping)
            jumpingCooldown = jumpingCooldown <= float.Epsilon ? 0f : jumpingCooldown - Time.deltaTime;
        if (jumpingCooldown < 0f) Debug.Log("Jumping cooldown is over!");
        if (hp < 1)
        {
            Debug.Log("Player is dead!");
            GameOverScreen.SetActive(true);
            Destroy(gameObject);
        }
        damageCooldown = damageCooldown <= float.Epsilon ? 0f : damageCooldown - Time.deltaTime;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (damageCooldown > float.Epsilon) return;
        if (other.CompareTag("Enemy"))
        {
            if (!other.GetComponent<Enemy>().isJumping) {
                hp--;
                hpText.text = hp.ToString();
                damageCooldown = damageCooldownTime;
                StartCoroutine(DamageEffect());
            }
        }
        else if (other.CompareTag("Bullet"))
        {
            hp--;
            hpText.text = hp.ToString();
            damageCooldown = damageCooldownTime;
            StartCoroutine(DamageEffect());
        }
        
        Debug.Log(hp);
    }

    IEnumerator Jump()
    {
        if (isJumping) yield break;
        if (jumpingCooldown > float.Epsilon) yield break;
        jumpingCooldown = jumpingCooldownTime;
        isJumping = true;
        // Jump logic here Move toward the spirit position until the distance is less than 0.25f
        Vector2 direction = spiritTransform.position - transform.position;
        while (direction.magnitude > 0.25f)
        {
            transform.position += jumpingSpeed * Time.deltaTime * (Vector3)direction.normalized;
            yield return null;
            direction = spiritTransform.position - transform.position;
        }
        isJumping = false;
        
        yield return null;
    }
    IEnumerator DamageEffect()
    {
        audioSource.PlayOneShot(audioSource.clip);
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
}
