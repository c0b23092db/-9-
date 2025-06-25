using UnityEngine;
public class AudioPanel : MonoBehaviour
{
    [Header("オーディオ設定")]
    [SerializeField] AudioClip[] audioClips;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(string seName)
    {
        switch (seName)
        {
            case "移動":
                audioSource.PlayOneShot(audioClips[0]);
                break;
            case "クリック":
                audioSource.PlayOneShot(audioClips[1]);
                break;
        }
    }
}