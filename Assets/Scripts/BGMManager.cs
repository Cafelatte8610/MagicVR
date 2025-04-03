using UnityEngine;
using DG.Tweening;

public class BGMManager : MonoBehaviour
{
    private AudioSource audioSource;
    private float initVal;
    public float fadeTime = 0.5f;
    private AudioClip nextBGM;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        initVal = audioSource.volume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeBGM(AudioClip audioClip)
    {
        nextBGM = audioClip;
        DOTween.To (()=>audioSource.volume, (value)=>audioSource.volume = value, 0, fadeTime).SetEase(Ease.InOutSine).OnComplete(ChangeBGMCallback);
    }

    private void ChangeBGMCallback()
    {
        audioSource.clip = nextBGM;
        audioSource.Play();
        DOTween.To (()=>audioSource.volume, (value)=>audioSource.volume = value, initVal, fadeTime).SetEase(Ease.InOutSine);

    }
}
