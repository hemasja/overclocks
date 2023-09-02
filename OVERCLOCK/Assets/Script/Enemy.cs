using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health, maxHealth = 3f;
    [SerializeField] FloatingHealthBar healthBar;
    [SerializeField] float moveSpeed = 5f;
    EnemyVisionZone enemyVisionZone;
    
    Rigidbody2D rb;
    Transform target;
    Vector2 moveDirection;

    public PlayerMovement playerMovement;

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        playerMovement = GetComponent<PlayerMovement>();
        enemyVisionZone = GetComponentInChildren<EnemyVisionZone>();
    }
    private void Start(){
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
        target = GameObject.Find("Player").transform;
    }

    private void Update(){
        if(enemyVisionZone.IsInRange){
            if(target){
                Vector3 direction = (target.position - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                rb.rotation = angle;
                moveDirection = direction;
            }
        }else{
            rb.velocity = Vector2.zero;
        }
    }

    private void FixedUpdate(){
        if(enemyVisionZone.IsInRange){
            if(target){
            rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
            }
        }else{
            rb.velocity = Vector2.zero;
        }
    }

    public void takeDamage(float damageAmount){
        health -= damageAmount;
        healthBar.UpdateHealthBar(health, maxHealth);
        if(health <= 0 ){
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.TryGetComponent<PlayerStats>(out PlayerStats playerComponent)){
            playerComponent.takeDamage(1);
        }
    }

    // Tambahkan method baru untuk mengejar pemain
   

    
}
