using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health, maxHealth = 3f;
    [SerializeField] FloatingHealthBar healthBar;
    [SerializeField] float moveSpeed = 5f;
    
    Rigidbody2D rb;
    Transform target;
    Vector2 moveDirection;

    public PlayerMovement playerMovement;

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }
    private void Start(){
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
        target = GameObject.Find("Player").transform;
    }

    private void Update(){
        if(target){
            Vector3 direction = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
            moveDirection = direction;
        }
    }

    private void FixedUpdate(){
        if(target){
            rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
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
            playerMovement.KBcounter = playerMovement.KBTotalTime;
            if(collision.transform.position.x <= transform.position.x){
                playerMovement.knockFromRight = true;
            }
            if(collision.transform.position.x > transform.position.x){
                playerMovement.knockFromRight = false;
            }
            
        }

    }
}
