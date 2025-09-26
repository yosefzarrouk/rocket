using System.Collections;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


public class movement : MonoBehaviour
{
    public Rigidbody2D rb;
    private float force = 53000;
    public float forcetime;
    public float direction;
    public Vector2 v;
    public float x ,y;
    public float start_height = -350;
    public float start_position = 813f;
    public Vector2 nForce;
    public Vector2 mg;
    private bool isFree;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        force = 53000f;
        forcetime = 2;
        rb.linearVelocity = Vector2.zero * 0;
        rb.angularVelocity = 0f;
        isFree = false;
        transform.position = new Vector2(start_position, start_height);
        LaunchForXSeconds(forcetime, force);
    }


    // Update is called once per frame
    void Update()
    {
        x = transform.position.x;
        y = transform.position.y;
        mg = rb.mass * Physics2D.gravity;
        if (y < start_height)
        {
            nForce = mg * -1;
        }
        else
        {
            nForce = mg * 0;
        }
        rb.AddForce(Vector2.up * nForce);
        if (Input.GetKeyDown(KeyCode.Space))
        {
        }
        if (isFree)
            rb.rotation = direction;
        //if (time > forcetime)
        //    force = 0f;
    }
    private void FixedUpdate()
    {
        //rb.AddForce(transform.up * force, ForceMode2D.Force);

        v = rb.linearVelocity;
        direction = Mathf.Atan2(v.y,v.x) * Mathf.Rad2Deg - 90;

    }
    public void LaunchForXSeconds(float seconds, float force1)
    {
        float time = 0f;
        while (time < seconds)
        {
             time = time + Time.deltaTime;
             rb.AddForce(transform.up * force1, ForceMode2D.Force);
        }
        force = 0;
        isFree = true;
    }







    //פעולות עזר הקשורות לזמנים          
    public void DosomthingForXSeconds(float seconds)
    {
        float time = 0f;
        {
            while (time < seconds)
            {
                time += Time.deltaTime;
                //do the thing
            }
        }
    }
    public void DosomthingAfterXSeconds(float seconds)
    {
        float time = 0f;
        {
            while (time < seconds)
            {
                time += Time.deltaTime;
            }
            //do the thing
        }
    }
}
