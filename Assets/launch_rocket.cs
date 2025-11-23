using UnityEngine;

public class launch_rocket : MonoBehaviour
{
    public GameObject rocket;
    public int counter = 0;
    public int launching_number = 7;
    float time_for_launch;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (time_for_launch < 0)
        {
            if (counter < launching_number)
            {
                counter++;
                launchrocket();
            }
            time_for_launch = 1f;
        }
        time_for_launch = time_for_launch - Time.deltaTime;
    }
    public void launchrocket()
    {
        Instantiate(rocket);
    }
}
