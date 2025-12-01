using JetBrains.Annotations;
using UnityEngine;

public class launch_rockets : MonoBehaviour
{
    public GameObject rocket;
    public int number_of_rockets;
    public float delay_between_rockets;
    public float time_to_next_launch;
    public int rocket_count;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        number_of_rockets = 45;
        delay_between_rockets = 1f;
        time_to_next_launch = 0f;
        rocket_count = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (number_of_rockets > 0 && time_to_next_launch < 0)
        {
            Launch_a_rocket();
            time_to_next_launch = delay_between_rockets;
            number_of_rockets--;
            rocket_count++;
        }
        time_to_next_launch = time_to_next_launch - Time.deltaTime;

    }
    public void Launch_a_rocket()
    {
        Instantiate(rocket);
        GameObject.Find("rocket(Clone)").name = "rocket " + rocket_count;
    }
}
