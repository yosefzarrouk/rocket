using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using JetBrains.Annotations;
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
public class missile_movement : MonoBehaviour
{
    public Position interceptionposition = new Position(0,0);
    public float dircetion_to_interception_position;
    public Rigidbody2D rb;
    public movement script;
    public float starting_position = -810;
    public Position[] rocket_positions = new Position[20];
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.transform.position = new Vector3(starting_position, -351, 0);
        for (int time = 0; time < rocket_positions.Length; time++)
        {
            rocket_positions[time] = Calculate_Position_in(time);
        }
    }
    // Update is called once per frame
    void Update()
    {
        interceptionposition.x = script.transform.position.x;
        interceptionposition.y = script.transform.position.y;
        dircetion_to_interception_position = Mathf.Atan2(interceptionposition.y - transform.position.y, interceptionposition.x - transform.position.x) * Mathf.Rad2Deg;
        if (script.v.y < 0)
        {
            rb.AddForce(transform.up * 3000);
            if (Input.GetKey(KeyCode.A))
            {
                rb.AddForce(Vector2.left * 3000);
            }
            if (Input.GetKey(KeyCode.D))
            {
                rb.AddForce(Vector2.right * 3000);
            }
        }
        Debug.Log("the direction is: " + dircetion_to_interception_position);
    }
    void LateUpdate()
    {
    }

    public Position Calculate_Position_in(float time)
    {
        float x = script.transform.position.x + script.v.x * time;
        float y = script.transform.position.y + script.v.y * time - Physics2D.gravity.y * time * time / 2;
        return new Position(x,y);
    }
}

