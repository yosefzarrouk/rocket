using UnityEngine;

public class launch : MonoBehaviour
{
    public GameObject rocket;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Launch();
        }
    }
    public void Launch()
    {
        Instantiate(rocket);
    }
}
