using UnityEngine;

public class boom : MonoBehaviour
{
    public float time = 0f;
    public float explosion_time = 0.05f;
    public AudioSource audioSource;
    public AudioClip explosion_sound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource.PlayOneShot(explosion_sound);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > explosion_time)
        {
            Destroy(gameObject);
        }
    }
}
