using UnityEngine;

public class Enemigo1 : MonoBehaviour
{
    [Header("Detección y Movimiento")]
    [SerializeField] private float detectionRange = 6f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private bool canFly = false;

    [Header("Daño")]
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private float attackCooldown = 1f;

    [Header("Drop de Item")]
    [SerializeField] private GameObject itemDropPrefab; // Prefab del item que va a soltar

    private Transform playerTransform;
    private float lastAttackTime;

    public float vida = 50f; //
    public GameObject Enemigo; //[cite: 1]

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= detectionRange)
        {
            FollowPlayer();
        }

        if (vida <= 0) //[cite: 1]
        {
            Muerte();
        }
    }

    private void FollowPlayer()
    {
        Vector2 targetPosition;

        if (canFly)
        {
            targetPosition = playerTransform.position;
        }
        else
        {
            targetPosition = new Vector2(playerTransform.position.x, transform.position.y);
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (playerTransform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (playerTransform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                JugadorVida playerHealth = collision.gameObject.GetComponent<JugadorVida>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageAmount);
                    lastAttackTime = Time.time;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    public void Muerte() //[cite: 1]
    {
        // Genera el item en la posición actual del enemigo antes de destruirlo
        if (itemDropPrefab != null)
        {
            Instantiate(itemDropPrefab, transform.position, Quaternion.identity);
        }

        GameObject.Destroy(Enemigo); //[cite: 1]
    }
}