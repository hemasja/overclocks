using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    PlayerStats health;
    public GameObject Meele;
    public GameObject Gun;
    public static int keyCounts = 1;

    void Awake()
    {
        // Inisialisasi komponen health sebelum digunakan
        health = GetComponent<PlayerStats>();
        
        changeWeapon(); // Memanggil changeWeapon() saat Awake
    }

    void Update()
    {
        changeWeaponCounts(); // Memanggil changeWeaponCounts() dalam Update
        changeWeapon(); // Memanggil changeWeapon() dalam Update
    }

    void changeWeaponCounts()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (keyCounts < 1)
            {
                keyCounts += 1;
            }
            else
            {
                keyCounts = 0;
            }
        }
        StopAllCoroutines();
    }

    public void changeWeapon()
    {   

            if (keyCounts == 0)
            {   
                Gun.SetActive(true);
                Meele.SetActive(false);
                
            }
            else if (keyCounts == 1)
            {
                Meele.SetActive(true);
                Gun.SetActive(false);
                
            }
          
    }
}
