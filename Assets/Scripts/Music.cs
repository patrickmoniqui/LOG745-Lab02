using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour
{
    public Slider musicSlider, effectSlider;
    public AudioSource audioSourceMusic, audioSourceEffect;
    public static float musicVolume = 1f, effectVolume = 1f;

    // Start is called before the first frame update
    void Start()
    {
        musicSlider.value = musicVolume;
        effectSlider.value = effectVolume;
    }

    void OnEnable()
    {
        musicSlider.onValueChanged.AddListener(delegate { 
            musicVolume = musicSlider.value;
            if (audioSourceMusic != null)
            {
                audioSourceMusic.volume = musicVolume;
            }
        });
        effectSlider.onValueChanged.AddListener(delegate {
            effectVolume = effectSlider.value;
            if (audioSourceEffect != null)
            {
                audioSourceEffect.volume = musicVolume;
            }
        });
    }

    private void OnDisable()
    {
        musicSlider.onValueChanged.RemoveAllListeners();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
