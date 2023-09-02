using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] public float health, maxHealth = 3f, maxMana = 100f;
    [SerializeField] public static float mana;
    [SerializeField] private Slider sliderHealth;
    [SerializeField] private Slider sliderMana;
    Rigidbody2D rb;

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        
    }
    void Start()
    {
        health = maxHealth;
        mana = maxMana;

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
        sliderHealth.value = currentValue / maxValue;
    }

    public void DrainMana(float manaDrain){
        mana -= manaDrain;
        UpdateManaBarPlayer(mana, maxMana);

    }
    public void UpdateManaBarPlayer(float currentValue, float maxValue){
        sliderMana.value = currentValue / maxValue;
    }
}
