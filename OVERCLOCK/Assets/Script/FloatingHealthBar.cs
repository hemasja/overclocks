using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    // [SerializeField] private Camera cameras;
    // [SerializeField] private Transform target;
    // [SerializeField] private Vector3 offset;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // transform.rotation = cameras.transform.rotation;
        // transform.position = target.position + offset;
    }

    public void UpdateHealthBar(float currentValue, float maxValue){
        slider.value = currentValue / maxValue;
    }
}
