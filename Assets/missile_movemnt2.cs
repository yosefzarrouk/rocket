using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using JetBrains.Annotations;


public class missile_movement2 : MonoBehaviour
{
    public GameObject targeted_rocket;
    public launch_missiles launcher_script;
    public int id;
    public Position[] rocket_positions = new Position[15];
    public float starting_direction = 60;
    public Vector2 v;
    public Position interceptionposition = new Position(0, 0);
    public Position rocketposition = new Position(0, 0);
    public float dircetion_to_interception_position;
    public float dircetion_to_rocket_position;
    public Rigidbody2D rb;
    public movement script;
    public float starting_positionx = -500;
    private float missile_direction;
    public bool isLaunched = false;
    public float forward_force;
    public float side_force = 5000;
    public float max_force = 30000;
    public float time_for_interception = 5;
    public float distance_to_interception_position;
    public float distance_to_rocket_position;
    public float vsize;
    public float starting_force = 3000;
    public side rocket_side;
    public bool a = false;
    float time = 0;
    public Position starting_position;
    public float starting_distance_to_interception;
    public float max_force_needed;

    public enum flight_stage
    {
        not_launched,
        getting_higher,
        aiming,
        on_way,
        intercepting
    }
    public flight_stage missile_stage = flight_stage.not_launched;
    public side start_side;
    public enum side
    {
        left, right
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        id = launcher_script.missiles_launched;
        gameObject.name = "missile" + id;
        targeted_rocket = GameObject.Find("rocket" + (id));
        script = GameObject.Find("rocket" + (id)).GetComponent<movement>();
        rb.transform.position = new Vector3(starting_positionx, -350, 0);
        starting_position = new Position(rb.transform.position.x, rb.transform.position.y);
    }
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -350)
        {
            rb.AddForce(Vector2.up * -1 * Physics2D.gravity.y * rb.mass);
        }
        if (isLaunched)
        {
            time = time + Time.deltaTime;
            time_for_interception = time_for_interception - Time.deltaTime;
        }
        if (time > 0.1)
        {
            missile_direction = Calculatedirection_ofMissile();
            rb.rotation = missile_direction - 90;
        }
        rocket_side = WhichSideIsRocket();
        distance_to_interception_position = Calculate_Distance_to_interception_position();
        distance_to_rocket_position = Calculate_Distance_to_rocket();
        vsize = rb.linearVelocity.magnitude;
        rocketposition = new Position(targeted_rocket.transform.position.x, targeted_rocket.transform.position.y);
        v = rb.linearVelocity;
        dircetion_to_interception_position = CalculateDirectionToPosition(interceptionposition);
        dircetion_to_rocket_position = CalculateDirectionToPosition(rocketposition);
        //dircetion_to_interception_position = Mathf.Atan2(interceptionposition.y - transform.position.y, interceptionposition.x - transform.position.x) * Mathf.Rad2Deg;
        if (missile_stage == flight_stage.not_launched)
        {
            for (int i = 1; i < rocket_positions.Length; i++)
            {
                rocket_positions[i] = Calculate_Position_of_rocket_in(i);
                starting_distance_to_interception = Calculate_Distance_Between_Two_Positions(rocket_positions[i], starting_position) -35;
                max_force_needed = (rb.mass * 2 * (starting_distance_to_interception)) / (i * i);
                if (max_force_needed < max_force)
                {
                    time_for_interception = i - 2;
                    break;
                }
            }
            interceptionposition = Calculate_Position_of_rocket_in(time_for_interception);
            missile_stage = flight_stage.getting_higher;

        }
        if (missile_stage == flight_stage.getting_higher)
        {
            Moveforward(starting_force);
            if (rb.transform.position.y > -345)
            {
                start_side = WhichSideIsInterceptionPosition();
                missile_stage = flight_stage.aiming;
            }
            isLaunched = true;
        }
        if (missile_stage == flight_stage.aiming)
        {
            Moveforward(starting_force / 7);
            if (start_side == side.left)
            {
                Turnleft();
            }
            else
            {
                Turnright();
            }
            if (start_side == side.left && WhichSideIsInterceptionPosition() == side.right)
            {
                missile_stage = flight_stage.on_way;
            }
            if (start_side == side.right && WhichSideIsInterceptionPosition() == side.left)
            {
                missile_stage = flight_stage.on_way;
            }
        }
        if (missile_stage == flight_stage.on_way)
        {
            forward_force = (rb.mass * 2 * (distance_to_interception_position - vsize * time_for_interception)) / (time_for_interception * time_for_interception);
            if (forward_force < 0)
            {
                forward_force = 0;
            }
            if (forward_force > max_force)
            {
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
            if (distance_to_interception_position < 25)
            {
                missile_stage = flight_stage.intercepting;
            } 
            Moveforward(forward_force);
        }
        if (missile_stage == flight_stage.intercepting)
        {
            if (WhichSideIsRocket() == side.left)
            {
                Turnleft();
            }
            else
            {
                Turnright();
            }
            Moveforward(max_force);
            if (distance_to_rocket_position < 5)
            {
                Destroy(gameObject);
                Destroy(script.gameObject);
            }
        }
    }
    void FixedUpdate()
    {


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

