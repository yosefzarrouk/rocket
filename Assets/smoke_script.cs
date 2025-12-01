using UnityEngine;

public class smoke_script : MonoBehaviour
{
    public ParticleSystem smoke;
    public int index;
    GameObject gameObject;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject = GameObject.Find("interceptor " + index);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = gameObject.transform.position;
        if (GameObject.Find("interceptor " + index) == null)
            smoke.Stop();
    }
}
