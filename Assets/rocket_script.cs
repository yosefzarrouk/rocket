using UnityEngine;

public class rocket_script : MonoBehaviour
{
    public Rigidbody2D rb;
    public float startingXposition;
    public float starting_direction;
    public float launch_force;
    public float calculated_rocket_direction;
    public float rocket_final_speed;
    public enum rocket_flight_stages
    {
        not_launched,
        being_launched,
        released
    }
    public rocket_flight_stages flight_stage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingXposition = 635f;
        starting_direction = 110f;
        launch_force = 15000;
        rocket_final_speed = Random.Range(75, 120f);
        transform.position = new Vector3(startingXposition, 0, 0);
        rb.rotation = starting_direction - 90;
        flight_stage = rocket_flight_stages.being_launched;
    }

    // Update is called once per frame
    void Update()
    {
        if (flight_stage == rocket_flight_stages.being_launched)
        {
            rb.AddForce(transform.up * launch_force);
            if (rb.linearVelocity.magnitude > rocket_final_speed)
            {
                flight_stage = rocket_flight_stages.released;
            }
        }
        if (flight_stage == rocket_flight_stages.released)
        {
            calculated_rocket_direction = Calculate_rocket_direction();
            rb.rotation = calculated_rocket_direction - 90;
        }
    }
    public float Calculate_rocket_direction()
    {
        return Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
    }
}
