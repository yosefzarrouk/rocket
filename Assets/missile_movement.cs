using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using JetBrains.Annotations;


public class missile_movement : MonoBehaviour
{
    public Vector2 v;
    public Position interceptionposition = new Position(0,0);
    public Position rocketposition = new Position(0, 0);
    public float dircetion_to_interception_position;
    public float dircetion_to_rocket_position;
    public Rigidbody2D rb;
    public movement script;
    public float starting_position = -810;
    public Position[] rocket_positions = new Position[20];
    public float missile_direction;
    public bool isLaunched = false;
    public float forward_force = 3000;
    public float side_force = 0;
    public float max_force = 3000;
    public float time_for_interception = 5;
    public float distance_to_interception_position;
    public float distance_to_rocket_position;
    public float vsize;
    public enum side
    {
        left, right
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.transform.position = new Vector3(starting_position, -351, 0);
    }
    // Update is called once per frame
    void Update()
    {
        time_for_interception = time_for_interception - Time.deltaTime;
        distance_to_interception_position = Calculate_Distance_to_interception_position();
        distance_to_rocket_position = Calculate_Distance_to_rocket();
        vsize = rb.linearVelocity.magnitude;
        rocketposition = new Position(script.transform.position.x, script.transform.position.y);
        interceptionposition = new Position(0, 0);
        missile_direction = Calculatedirection_ofMissile();
        v = rb.linearVelocity;
        dircetion_to_interception_position = CalculateDirectionToPosition(interceptionposition);
        dircetion_to_rocket_position = CalculateDirectionToPosition(rocketposition);
        //dircetion_to_interception_position = Mathf.Atan2(interceptionposition.y - transform.position.y, interceptionposition.x - transform.position.x) * Mathf.Rad2Deg;
        if (script.v.y < 0)
        {
            isLaunched = true;
        }
        if (isLaunched)
        {
            forward_force = rb.mass * 2 * ((distance_to_interception_position - vsize * time_for_interception) /(time_for_interception * time_for_interception));
            if (forward_force < 0)
            {
                forward_force = 0;
            }
            if (forward_force > max_force)
            {
                setIntetceptionPosition(time_for_interception + 1);
                forward_force = max_force;
            }
            if (WhichSideIsInterceptionPosition() == side.left)
            {
                Turnleft();
            }
            else
            {
                Turnright();
            }
            Moveforward(forward_force);
            if (Input.GetKey(KeyCode.A))
            {
                rb.AddForce(Vector2.left * side_force);
            }
            if (Input.GetKey(KeyCode.D))
            {
                rb.AddForce(Vector2.right * side_force);
            }
        }
    }

    public Position Calculate_Position_of_rocket_in(float time)
    {
        float x = script.transform.position.x + script.v.x * time;
        float y = script.transform.position.y + script.v.y * time + Physics2D.gravity.y * time * time / 2;
        return new Position(x, y);
    }
    public float Calculatedirection_ofMissile()
    {
        float direction = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        if (direction < 0)
        {
            direction = direction + 360;
        }
        if (!isLaunched)
        {
            direction = (360 - transform.eulerAngles.z) * -1 + 90;
        }
        return direction;
    }

    public float CalculateDirectionToPosition(Position p)
    {
        float direction = Mathf.Atan2(p.y - transform.position.y, p.x - transform.position.x) * Mathf.Rad2Deg;
        if (direction < 0)
        {
            direction = direction + 360;
        }
        return direction;
    }
    public side WhichSideIsDirection2(float direction1, float direction2)
    {
        float edge = 0;
        if (direction1 > 180)
        {
            edge = missile_direction - 180;
            if (direction2 < direction1 && direction2 > edge)
                return side.right;
            else
                return side.left;
        }
        else
        {
            edge = missile_direction + 180;
            if (direction2 > direction1 && direction2 < edge)
            {
                return side.left;
            }
            else
                return side.right;
        }
    }
    public side WhichSideIsPosition(Position position)
    {
        float direction_to_position = CalculateDirectionToPosition(position);
        return WhichSideIsDirection2(missile_direction, direction_to_position);
    }
    public side WhichSideIsInterceptionPosition()
    {
        return WhichSideIsPosition(interceptionposition);
    }
    public side WhichSideIsRocket()
    {
        return WhichSideIsPosition(rocketposition);
    }
    public void Moveforward(float force)
    {
        rb.AddForce(transform.up * force);
    }
    public void Turnleft()
    {
        rb.AddForce(transform.right * -side_force);
    }
    public void Turnright()
    {
        rb.AddForce(transform.right * side_force);
    }
    public class Position
    {
        public float x;
        public float y;
        public Position(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }
    public float Calculate_Distance_Between_Two_Positions(Position p1, Position p2)
    {
        return Mathf.Sqrt((p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y));
    }
    public float Calculate_Distance_to_interception_position()
    {
        return Calculate_Distance_Between_Two_Positions(new Position(transform.position.x, transform.position.y), interceptionposition);
    }
    public float Calculate_Distance_to_rocket()
    {
        return Calculate_Distance_Between_Two_Positions(new Position(transform.position.x, transform.position.y), rocketposition);
    }
    public void setIntetceptionPosition(float time)
    {
        time_for_interception = time;
        interceptionposition = Calculate_Position_of_rocket_in(time);
    }
}

