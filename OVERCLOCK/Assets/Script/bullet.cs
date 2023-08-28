using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rb;

    private void OnCollisionEnter2D(Collision2D collision){

        if(collision.gameObject.TryGetComponent<Enemy>(out Enemy enemyComponent)){
            enemyComponent.takeDamage(1);
        }

        Destroy(gameObject);
    }
}
