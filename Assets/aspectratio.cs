using UnityEngine;

public class aspectratio : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Camera.main.aspect = 16f / 9f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
