using UnityEngine;
public class Audio_Player : MonoBehaviour
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
            case "SE0":
                audioSource.PlayOneShot(audioClips[0]);
                break;
            case "SE1":
                audioSource.PlayOneShot(audioClips[1]);
                break;
            case "SE2":
                audioSource.PlayOneShot(audioClips[2]);
                break;
        }
    }
}