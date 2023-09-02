using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{

    public Slider backgroundSlider, soundEffectSlider;
    private float backgroundFloat, soundEffectFloat;
    public AudioSource backgroundAudio;
    public AudioSource[] soundEffectAudio;
    public static bool soundLoad = false;
    

    

    void Start()
    {
        
        backgroundFloat = .125f;
        soundEffectFloat = .50f;        
        backgroundSlider.value = backgroundFloat;
        soundEffectSlider.value = soundEffectFloat;
        
        
    }
    public void UpdateSound(){
        backgroundAudio.volume = backgroundSlider.value;

        for(int i = 0; i< soundEffectAudio.Length; i++){
            soundEffectAudio[i].volume = soundEffectSlider.value;
        }
    }

    

}
