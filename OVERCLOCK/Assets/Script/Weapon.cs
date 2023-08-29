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

    public GameObject bullet;
    public Transform firePoint;
    public float fireRange = 0.5f;
    public LayerMask enemyLayer;

    public float fireforce;


    private bool usingController = false; // Track if the controller is being used

    void Awake()
    {
        controls = new Controller();
        controls.Gameplay.Shoot.performed += ctx => fire();
        controls.Gameplay.Shoot.performed += ctx => Attack();
        controls.Gameplay.Rotate.performed += ctx => usingController = true;
        controls.Gameplay.Rotate.canceled += ctx => usingController = false;

        controls.Gameplay.Enable();

    }

    private void Update()
    {
        if (!usingController && Input.GetMouseButtonDown(0))
        {   if(WeaponManager.keyCounts == 0){
            fire();
            }
            if(WeaponManager.keyCounts == 1){
            Attack();
            }
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
    {
        GameObject projectile = Instantiate(bullet, firePoint.position, firePoint.rotation);
        projectile.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireforce, ForceMode2D.Impulse);
    }

    public void Attack(){
        Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(firePoint.position, fireRange, enemyLayer);
        
        foreach(Collider2D enemy in hitEnemy){
            enemy.gameObject.TryGetComponent<Enemy>(out Enemy component);
            component.takeDamage(1);
            
        }
    }

    void OnDrawGizmosSelected(){
        if(firePoint == null)
        return;

        Gizmos.DrawWireSphere(firePoint.position, fireRange);
    }

    
}
