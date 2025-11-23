using System.Collections;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
public class movement : MonoBehaviour
{
    public launch_rocket script;
    public Rigidbody2D rb;
    private float force;
    public float forcetime;
    public float rocket_direction;
    public Vector2 v;
    public float x ,y;
    public float start_height = -350;
    public float start_position = 813f;
    public Vector2 nForce;
    public Vector2 mg;
    private bool isFree = false;
    public float time = 0f;
    void Start()
    {
        script = GameObject.Find("rocket_launcher").GetComponent<launch_rocket>();
        gameObject.name = "rocket" + script.counter;
        time = 0f;
        force = 12500;
        forcetime = 2f;
        rb.linearVelocity = Vector2.zero * 0;
        rb.angularVelocity = 0f;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("rocket velocity: " + rb.linearVelocity.magnitude);
        }
        time = time + Time.deltaTime;
        if (rb.linearVelocity.magnitude < 75)
        {
            rb.AddForce(transform.up * force, ForceMode2D.Force);
        }
        else
        {
            isFree = true;
        }
        x = transform.position.x;
        y = transform.position.y;
        mg = rb.mass * Physics2D.gravity;
        if (Mathf.Approximately(y,start_height))
        {
            nForce = mg * -1;
        }
        else
        {
            nForce = mg * 0;
        }
        rb.AddForce(Vector2.up * nForce);
        if (isFree)
        {
            rocket_direction = Calculatedirection();
            rb.rotation = rocket_direction - 90;
        }
    }
    private void FixedUpdate()
    {
        v = rb.linearVelocity;
    }
    public float Calculatedirection()
    {
        float direction = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        if (direction < 0)
        {
            direction = direction + 360;
        }
        return direction;
    }
}
