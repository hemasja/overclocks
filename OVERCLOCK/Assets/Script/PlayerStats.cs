using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] public float health, maxHealth = 3f;
    [SerializeField] private Slider slider;
    Rigidbody2D rb;

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        
    }
    void Start()
    {
        health = maxHealth;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void takeDamage(float damageAmount){
        health -= damageAmount;
        UpdateHealthBarPlayer(health, maxHealth);
        if(health <= 0 ){
            Destroy(gameObject);
        }
        
    }
    public void UpdateHealthBarPlayer(float currentValue, float maxValue){
        slider.value = currentValue / maxValue;
    }
}
