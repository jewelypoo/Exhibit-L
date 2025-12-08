using UnityEngine;

public class AudioOneShot : MonoBehaviour
{
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayClip(AudioClip clip, float pitch)
    {
        source.pitch = pitch;
        source.clip = clip;
        source.Play();

        Destroy(gameObject, clip.length / Mathf.Abs(pitch));
    }
}
