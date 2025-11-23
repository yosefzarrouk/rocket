using JetBrains.Annotations;
using System;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class interceptor_movement : MonoBehaviour
{
    public Rigidbody2D rb;
    public int index;
    public Vector2 interception_position;
    public Vector2 rocket_position;
    public float rising_force;
    public float rising_max_height;
    public float max_force;
    public float rotation_force;
    public float time_for_interception;
    public float distance_to_interception_position;
    public float distance_to_rocket;
    public float missile_direction;
    public float direction_to_interception;
    public float needed_force;
    public float startingXPosition;
    public float direction_to_rocket;
    public float starting_direction;
    public flight_stages flight_stage;
    public sides interception_position_side;
    public sides starting_interception_position_side;
    public sides rocket_side;
    public enum sides
    {
        left,
        right
    }
    public enum flight_stages
    {
        rising,
        rotating,
        on_way,
        full_force
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flight_stage = flight_stages.rising;
        rising_max_height = 30;
        rising_force = 20000;
        max_force = 40000;
        rotation_force = 10000;
        startingXPosition = -400;
        transform.position = new Vector2(startingXPosition, 0);
        time_for_interception = Choose_interception_position();
        starting_direction = 75;
        GameObject.Find("rocket_detector").GetComponent<detect_rockets_and_launch_interceptors>().rockets_info[index].time_to_interception = time_for_interception;
        Turnto(starting_direction);
    }

    // Update is called once per frame
    void Update()
    {
        interception_position = GameObject.Find("rocket_detector").GetComponent<detect_rockets_and_launch_interceptors>().rockets_info[index].predicted_position;
        time_for_interception = time_for_interception - Time.deltaTime;
        GameObject.Find("rocket_detector").GetComponent<detect_rockets_and_launch_interceptors>().rockets_info[index].time_to_interception = time_for_interception;
        missile_direction = Calculate_missile_direction();
        direction_to_interception = Calculat_direction_to_intercption();
        distance_to_interception_position = Calculate_distance_to_interception_position();
        interception_position_side = Calculate_interception_position_side();
        if (flight_stage == flight_stages.rising)
        {
            Move_forward(rising_force);
            if (transform.position.y > rising_max_height)
            {
                flight_stage = flight_stages.rotating;
                starting_interception_position_side = interception_position_side;
            }
        }
        if (flight_stage == flight_stages.rotating)
        {
            Turnto(missile_direction);
            if (interception_position_side == sides.left)
            {
                Turn_left();
            }
            else
            {
                Turn_right();
            }
            if (interception_position_side == starting_interception_position_side)
            {
                flight_stage = flight_stages.on_way;
            }
        }
        if (flight_stage == flight_stages.on_way)
        {
            Turnto(missile_direction);
            needed_force = Calculate_needed_force();
            Move_forward(needed_force);
            if (interception_position_side == sides.left)
            {
                Turn_left();
            }
            else
            {
                Turn_right();
            }
            rocket_position = GameObject.Find("rocket " + index).transform.position;
            distance_to_rocket = Calculate_distance_to_rocket();
            if (distance_to_rocket < 20)
            {
                flight_stage = flight_stages.full_force;
            }
        }
        if (flight_stage == flight_stages.full_force)
        {
            rocket_position = GameObject.Find("rocket " + index).transform.position;
            distance_to_rocket = Calculate_distance_to_rocket();
            Move_forward(max_force);
            direction_to_rocket = Calculat_direction_to_rocket();
            rocket_side = Calculate_what_side(missile_direction, direction_to_rocket);
            if (rocket_side == sides.left)
            {
                Turn_left();
            }
            else
            {
                Turn_right();
            }
            if (distance_to_rocket < 5)
            {
                 Destroy(GameObject.Find("rocket " + index));
                GameObject.Find("rocket_detector").GetComponent<detect_rockets_and_launch_interceptors>().rockets_info[index].exsists = false;
                Destroy(gameObject);
            }
        }
    }
    public sides Calculate_interception_position_side()
    {
        return Calculate_what_side(missile_direction, direction_to_interception);
    }
    public sides Calculate_what_side(float direction1, float direction2)
    {
        if (direction1 < 180)
        {
            if (direction2 < direction1 || direction2 > direction1 + 180)
            {
                return sides.right;
            }
            else
            {
                return sides.left;
            }
        }
        else
        {
            if (direction2 > direction1 || direction2 < direction1 - 180)
            {
                return sides.left;
            }
            else
            {
                return sides.right;
            }
        }
    }
    public float Calculate_missile_direction()
    {
        float vx = rb.linearVelocity.x;
        float vy = rb.linearVelocity.y;
        return Mathf.Atan2(vy, vx) * Mathf.Rad2Deg;
    }
    public float Calculat_direction_to_intercption()
    {
        return Mathf.Atan2(interception_position.y - transform.position.y, interception_position.x - transform.position.x) * Mathf.Rad2Deg;
    }
    public float Calculat_direction_to_rocket()
    {
        return Mathf.Atan2(rocket_position.y - transform.position.y, rocket_position.x - transform.position.x) * Mathf.Rad2Deg;
    }
    public void Move_forward(float force)
    {
        rb.AddForce(transform.up * force);
    }
    public void Turn_left()
    {
        rb.AddForce(transform.right * -rotation_force);
    }
    public void Turn_right()
    {
        rb.AddForce(transform.right * rotation_force);
    }
    public float Calculate_needed_force()
    {
        float force = (2 * rb.mass * (distance_to_interception_position - rb.linearVelocity.magnitude * time_for_interception)) / (time_for_interception * time_for_interception) + math.sin(missile_direction) * rb.mass * Physics2D.gravity.magnitude;
        if (force > max_force)
        {
            force = max_force;
        }
        if (force < 0)
        {
            force = 0;
        }
        return force;
    }
    public float Calculate_distance_to_interception_position()
    {
        return MathF.Sqrt((interception_position.x - transform.position.x) * (interception_position.x - transform.position.x) + (interception_position.y - transform.position.y) * (interception_position.y - transform.position.y));
    }
    public float Calculate_distance_to_rocket()
    {
        return MathF.Sqrt((rocket_position.x - transform.position.x) * (rocket_position.x - transform.position.x) + (rocket_position.y - transform.position.y) * (rocket_position.y - transform.position.y));
    }
    public void Turnto(float direction)
    {
        rb.rotation = direction - 90;
    }
    public Vector2 calculate_interception_position(int index, float time)
    {
        rocket_info rocket = GameObject.Find("rocket_detector").GetComponent<detect_rockets_and_launch_interceptors>().rockets_info[index];
        float x = rocket.position.x + rocket.velocity.x * time;
        float y = rocket.position.y + rocket.velocity.y * time + Physics2D.gravity.y * time * time / 2;
        return new Vector2(x, y);
    }
    public int Choose_interception_position()
    {
        int chosen_time = 5;
        Vector2[] rocket_positions = new Vector2[16];
        for (int t = 1; t <= 15; t++)
        {
            rocket_positions[t] = calculate_interception_position(index, t);
        }
        for (int t = 1; t <= 15; t++)
        {
            if (Is_interception__position_possible(rocket_positions[t], t) == true)
            {
                chosen_time = t;
            }
        }
        chosen_time = chosen_time + 2;
        return chosen_time;
    }
    public float Calculate_distance_to_position(Vector2 position)
    {
        return MathF.Sqrt((position.x - transform.position.x) * (position.x - transform.position.x) + (position.y - transform.position.y) * (position.y - transform.position.y));
    }
    public bool Is_interception__position_possible(Vector2 interception_position, float time)
    {
        float distance = Calculate_distance_to_position(interception_position);
        float needed_force = (2 * rb.mass * distance) / (time_for_interception * time_for_interception) + math.sin(Calculate_direction_to_position(interception_position)) * rb.mass * Physics2D.gravity.magnitude;
        if (needed_force > max_force)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public float Calculate_direction_to_position(Vector2 position)
    {
        return Mathf.Atan2(position.y - transform.position.y, position.x - transform.position.x) * Mathf.Rad2Deg;
    }
}
