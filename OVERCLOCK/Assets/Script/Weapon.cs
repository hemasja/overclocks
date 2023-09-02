using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;

public class Weapon : MonoBehaviour
{
    Controller controls;
    [SerializeField] Transform weaponTransform;
    [SerializeField] float rotationSpeed = 10f;
    
    public float chargeSpeed = 2f;
    float chargeTime = 0f;
    public float chargeRange = 10f;
    private bool isCharging;

    public GameObject bullet;
    public Transform firePoint;
    public float fireRange = 0.5f;
    public LayerMask enemyLayer;

    public float fireforce;
    [SerializeField] private PlayerStats playerStats;
    public Animator animator;


    private bool usingController = false; // Track if the controller is being used

    void Awake()
    {
        controls = new Controller();
        controls.Gameplay.Shoot.performed += ctx => fire();
        controls.Gameplay.Shoot.performed += ctx => Attack();
        controls.Gameplay.Rotate.performed += ctx => usingController = true;
        controls.Gameplay.Rotate.canceled += ctx => usingController = false;

        controls.Gameplay.Enable();
        playerStats = FindObjectOfType<PlayerStats>();

    }

    private void Update()
    {
        if (!usingController && Input.GetMouseButtonDown(0))
        {   if(WeaponManager.keyCounts == 0){
            fire();
            }
            if(WeaponManager.keyCounts == 1){
            Attack();
            animator.SetBool("Attack",true);
            StartCoroutine(ResetIsAttacking());
            }
        }

        if (!usingController && Input.GetMouseButton(0) && chargeTime < 2)
        {
            isCharging = true;
            if (isCharging)
            {
                chargeTime += Time.deltaTime * chargeSpeed;
                Debug.Log("das");
            }
        }

        if (!usingController && Input.GetMouseButtonUp(0) && chargeTime >= 2)
        {
            if(PlayerStats.mana >= 20){
            ChargeAttack();
            isCharging = false;
            playerStats.DrainMana(20);
            animator.SetBool("Attack",true);
            StartCoroutine(ResetIsAttacking());
            }else if(PlayerStats.mana < 20){
                Debug.Log("OutOfMana");
            }
        }
        else if (!usingController && Input.GetMouseButtonUp(0) && chargeTime < 2)
        {
            chargeTime = 0f;
            isCharging = false;
        }
        
    }

    private void FixedUpdate()
    {
        AimWeapon();
    }

    private void AimWeapon()
    {
        Vector2 aimDirection = Vector2.zero;

        // Determine aiming input based on controller or mouse
        if (usingController && controls.Gameplay.Rotate.ReadValue<Vector2>() != Vector2.zero)
        {
            aimDirection = controls.Gameplay.Rotate.ReadValue<Vector2>();
        }
        else if (!usingController)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 weaponToMouseDirection = mousePosition - (Vector2)weaponTransform.position;
            aimDirection = weaponToMouseDirection;
        }

        if (aimDirection != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            float currentAngle = weaponTransform.rotation.eulerAngles.z;
            float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
            weaponTransform.rotation = Quaternion.Euler(0f, 0f, newAngle);
        }
    }

    public void fire()
    {   if(PlayerStats.mana > 0){
        GameObject projectile = Instantiate(bullet, firePoint.position, firePoint.rotation);
        projectile.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireforce, ForceMode2D.Impulse);
        playerStats.DrainMana(5);
        }else if(PlayerStats.mana <= 0){
            Debug.Log("outOfMana");
        }
    }

    public void Attack(){
        Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(firePoint.position, fireRange, enemyLayer);
        
        Debug.Log("test hit");
        foreach(Collider2D enemy in hitEnemy){
            enemy.gameObject.TryGetComponent<Enemy>(out Enemy component);
            component.takeDamage(1);

            // Calculate knockback direction
            Vector2 knockbackDirection = (firePoint.position - enemy.transform.position).normalized;

            // Apply knockback force to the enemy's rigidbody
            Rigidbody2D enemyRigidbody = enemy.GetComponent<Rigidbody2D>();
            if (enemyRigidbody != null)
            {   Debug.Log("knock");
                enemyRigidbody.AddForce(knockbackDirection * fireforce, ForceMode2D.Impulse);
            }
            
        }
    }

    void OnDrawGizmosSelected(){
        if(firePoint == null)
        return;

        Gizmos.DrawWireSphere(firePoint.position, fireRange);
    }

    private void ChargeAttack()
    {
        // currentFacingTime = facing;

        // Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(new Vector2(attackPoint.position.x +
        // transform.localScale.x * chargeRange / 2 * currentFacingTime * (1 - upSlash), attackPoint.position.y +
        // transform.localScale.y * chargeRange / 2 * upSlash),
        // new Vector2(transform.localScale.x * chargeRange * (1 - upSlash), 1 + (transform.localScale.y * chargeRange * upSlash)),
        // 0, enemyLayers);
        
            Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(firePoint.position, fireRange, enemyLayer);
            
            
            foreach(Collider2D enemy in hitEnemy){
                enemy.gameObject.TryGetComponent<Enemy>(out Enemy component);
                component.takeDamage(3);
            
            }
        
    }
        private IEnumerator ResetIsAttacking()
        {
            // Menunggu beberapa detik sesuai dengan durasi animasi serangan
            yield return new WaitForSeconds(0.3f);

            // Mengatur kembali bool "IsAttacking" ke false setelah serangan selesai
            animator.SetBool("Attack", false);
        }
    
}
