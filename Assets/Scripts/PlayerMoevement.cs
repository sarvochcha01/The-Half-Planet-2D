using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoevement : MonoBehaviour
{
    [SerializeField] private Transform crosshairTransform;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector2 input;
    [SerializeField] private float speed;
    [SerializeField] private bool isMoving;
    [SerializeField] private bool isInvincible; 

    [SerializeField] private int health;
    [SerializeField] private int maxHealth;

    [SerializeField] private int extraHealth;

    [SerializeField] private Slider healthSlider;

    [SerializeField] private static PlayerMoevement Instance;

    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private LayerMask collectableMask;

    [SerializeField] private Collider2D[] collectableColliders;

    [SerializeField] private float invincibilityDuration;
    [SerializeField] private float invincibilityTimer;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<GameManager>().SetIsGameOver(false);
        //if (Instance != null)
        //{
        //    Destroy(this.gameObject);
        //    return;
        //}

        //Instance = this;
        //GameObject.DontDestroyOnLoad(this.gameObject);

        rb = GetComponent<Rigidbody2D>();
        crosshairTransform = FindObjectOfType<CrosshairManager>().GetCrosshairTransform();
        healthSlider = GameObject.FindGameObjectWithTag("PlayerHealthSlider").GetComponent<Slider>();
        healthSlider.maxValue = maxHealth;
        health = maxHealth;
        isInvincible = true;
    }

    private void Update()
    {
        if (isInvincible)
        {
            invincibilityTimer += Time.deltaTime;
            if (invincibilityTimer >= invincibilityDuration)
            {
                isInvincible = false;
                FindObjectOfType<UINotificationManager>().SetText("Find the exit");
            }
        }

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        healthSlider.value = health;

        if (input.magnitude > 0) isMoving = true;
        else isMoving = false;

        if (health >= maxHealth)
        {
            health = maxHealth;

        }

        if (health <= 0)
        {
            GameManager gm = FindObjectOfType<GameManager>();
            gm.SetIsGameOver(true);
            gm.status = "Faliure";
            FindObjectOfType<UINotificationManager>().SetGameOverText();
            healthSlider.gameObject.SetActive(false);
            Destroy(gameObject);
        }

        collectableColliders = Physics2D.OverlapCircleAll(transform.position, 30f, collectableMask);

        if (collectableColliders.Length >0)
        {
            foreach (var collectable in collectableColliders)
            {
                collectable.GetComponent<CollectableSpawner>().SpawnCollectable();
            }
        }

    }

    private void FixedUpdate()
    {
        input.Normalize();
        rb.MovePosition(rb.position + input * speed * Time.fixedDeltaTime);

        //rb.velocity = new Vector2(input.x * speed * Time.fixedDeltaTime, input.y * speed * Time.fixedDeltaTime);

        Vector3 mousePos = crosshairTransform.position;
        Vector3 dir = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle - 90); //new Vector3(0, 0, angle + 90);
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            health -= damage;
            FindObjectOfType<GameManager>().totalDamageTaken += damage;
        }
    }

    public void AddHealth(int _health)
    {
        health += _health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetCurrentHealth()
    {
        return health;
    }

    public int GetExtraHealth()
    {
        return extraHealth;
    }

}
