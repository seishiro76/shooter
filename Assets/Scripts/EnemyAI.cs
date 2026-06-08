using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform target; // игрок; если пусто — найдётся по тегу Player

    [Header("Detection")]
    [SerializeField] private float detectionRange = 20f;
    [SerializeField] private float attackRange = 2f;

    [Header("Attack")]
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float attackHitDelay = 0.4f; // задержка до момента урона внутри замаха

    [Header("Activation")]
    [SerializeField] private bool startActive = true; // false для врагов 2-й комнаты (разбудить через Activate)

    [Header("Animator Parameters")]
    [SerializeField] private string speedParam = "Speed";
    [SerializeField] private string attackTrigger = "Attack";
    [SerializeField] private string dieTrigger = "Die";

    private NavMeshAgent agent;
    private PlayerHealth targetHealth;
    private float lastAttackTime;
    private bool isActive;
    private bool isDead;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (enemyHealth == null)
        {
            enemyHealth = GetComponent<EnemyHealth>();
        }

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    private void OnEnable()
    {
        if (enemyHealth != null)
        {
            enemyHealth.Died += OnDied;
        }
    }

    private void OnDisable()
    {
        if (enemyHealth != null)
        {
            enemyHealth.Died -= OnDied;
        }
    }

    private void Start()
    {
        ResolveTarget();

        isActive = startActive;

        if (!isActive && agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }
    }

    // Повесить на onWaveCleared первой комнаты, чтобы враги второй комнаты "проснулись"
    public void Activate()
    {
        isActive = true;

        if (agent != null && agent.isOnNavMesh && !isDead)
        {
            agent.isStopped = false;
        }
    }

    private void ResolveTarget()
    {
        if (target == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                target = playerObject.transform;
            }
        }

        if (target != null)
        {
            targetHealth = target.GetComponent<PlayerHealth>();
        }
    }

    private void Update()
    {
        if (isDead || !isActive || target == null || agent == null || !agent.isOnNavMesh)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= attackRange)
        {
            agent.isStopped = true;
            FaceTarget();
            UpdateSpeedAnim(0f);
            TryAttack();
        }
        else if (distance <= detectionRange)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
            UpdateSpeedAnim(agent.velocity.magnitude);
        }
        else
        {
            agent.isStopped = true;
            UpdateSpeedAnim(0f);
        }
    }

    private void TryAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown)
        {
            return;
        }

        lastAttackTime = Time.time;

        if (animator != null && !string.IsNullOrEmpty(attackTrigger))
        {
            animator.SetTrigger(attackTrigger);
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(SoundType.EnemyAttack);
        }

        // урон наносим не мгновенно, а в момент "удара" внутри анимации
        Invoke(nameof(DealDamage), attackHitDelay);
    }

    private void DealDamage()
    {
        if (isDead || targetHealth == null || target == null)
        {
            return;
        }

        // если игрок успел отбежать во время замаха — урон не проходит
        if (Vector3.Distance(transform.position, target.position) <= attackRange + 0.5f)
        {
            targetHealth.TakeDamage(attackDamage);
        }
    }

    private void FaceTarget()
    {
        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
        {
            return;
        }

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 8f);
    }

    private void UpdateSpeedAnim(float speed)
    {
        if (animator != null && !string.IsNullOrEmpty(speedParam))
        {
            animator.SetFloat(speedParam, speed);
        }
    }

    private void OnDied(EnemyHealth source)
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        CancelInvoke(nameof(DealDamage));

        if (agent != null)
        {
            if (agent.isOnNavMesh)
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
            }

            agent.enabled = false; // отключаем агента, чтобы тело могло осесть на пол
        }

        if (animator != null && !string.IsNullOrEmpty(dieTrigger))
        {
            animator.SetTrigger(dieTrigger);
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(SoundType.EnemyDeath);
        }

        // сам объект уничтожит EnemyHealth с задержкой (чтобы доиграла анимация смерти)
        enabled = false;
    }
}