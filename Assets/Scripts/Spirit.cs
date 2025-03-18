using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Spirit : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    private InputAction move;
    private InputAction mousePosition;
    private InputAction attack;
    private bool isMoving = false;
    private float movingCooldown = 0f;
    private readonly float movingCooldownTime = 1.0f;
    private float attackCooldown = 0f;
    private readonly float attackCooldownTime = 0.5f;
    private readonly float speed = 30.0f;
    [SerializeField] private Transform attackTransform;
    [SerializeField] private GameObject attackHitbox;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        move = inputActions.FindActionMap("Spirit").FindAction("Move");
        mousePosition = inputActions.FindActionMap("Spirit").FindAction("MousePosition");
        attack = inputActions.FindActionMap("Spirit").FindAction("Attack");
        attackHitbox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (move.triggered)
        {
            StartCoroutine(Move());
        }
        if (attack.triggered) {
            StartCoroutine(Attack());
        }
        if (!isMoving)
            movingCooldown = movingCooldown <= float.Epsilon ? 0f : movingCooldown - Time.deltaTime;
        if (movingCooldown < 0f) Debug.Log("Moving cooldown is over!");
        if (!isMoving)
            attackCooldown = attackCooldown <= float.Epsilon ? 0f : attackCooldown - Time.deltaTime;
        if (attackCooldown < 0f) Debug.Log("Attack cooldown is over!");
        
    }

    IEnumerator Move()
    {
        if (isMoving) yield break;
        if (movingCooldown > float.Epsilon) yield break;
        movingCooldown = movingCooldownTime;
        isMoving = true;
        Vector2 position = mousePosition.ReadValue<Vector2>();

        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(position);
        Vector2 direction = worldPosition - (Vector2)transform.position;
        while (direction.magnitude > 0.25f)
        {
            transform.position += speed * Time.deltaTime * (Vector3)direction.normalized;
            yield return null;
            direction = worldPosition - (Vector2)transform.position;
        }
        transform.position = worldPosition;
        isMoving = false;
    }
    IEnumerator Attack()
    {
        if (attackCooldown > float.Epsilon) yield break;
        attackCooldown = attackCooldownTime;
        // Set z rotation according to the mouse position
        Vector2 position = mousePosition.ReadValue<Vector2>();
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(position);
        Vector2 direction = worldPosition - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        attackTransform.rotation = Quaternion.Euler(0, 0, angle);
        attackHitbox.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        attackHitbox.SetActive(false);
    }
}
