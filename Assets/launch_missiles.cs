using JetBrains.Annotations;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class launch_missiles : MonoBehaviour
{
    public int missiles_launched = 0;
    public GameObject missile_prefab;
    public float ground_height = -350f;
    public launch_rocket script;
    public int numberofrockets;
    public Rocket_information[] rockets_info = new Rocket_information[100];
    public Protected_range the_city = new Protected_range(-520,-170);
    public class Protected_range
    {
        public float start;
        public float end;
        public Protected_range(float start, float end)
        {
            this.start = start;
            this.end = end;
        }
    }
    public class Rocket_information
    {
        string rocket_name;
        public Vector2 position;
        public Vector2 velocity;
        public float velocityx;
        public float velocityy;
        public bool isDangerous;
        public bool missile_sent = false;
        public bool treated_once = false;
        public Rocket_information(Vector2 position, Vector2 velocity)
        {
            this.position = position;
            this.velocity = velocity;
            velocityx = velocity.x;
            velocityy = velocity.y;
        }
    }
    public bool IsDangerous(Rocket_information rocket)
    {
        float time_till_hit = -rocket.velocityy + Mathf.Sqrt(rocket.velocityy * rocket.velocityy - 2 * Physics2D.gravity.y * (ground_height - rocket.position.y)) / Physics2D.gravity.y;
        float x_at_hit = rocket.position.x + rocket.velocityx * time_till_hit;
        if (x_at_hit >= the_city.start && x_at_hit <= the_city.end)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        numberofrockets = script.launching_number;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 1; i < numberofrockets; i++)
        {
            Vector2 pos = GameObject.Find("rocket" + (i)).transform.position;
            movement rocket_script = GameObject.Find("rocket" + (i)).GetComponent<movement>();
            Vector2 vel = rocket_script.v;
            rockets_info[i] = new Rocket_information(pos, vel);
        }
        for (int i = 1; i < numberofrockets; i++)
        {
            if (rockets_info[i].treated_once == false)
            {
                if (rockets_info[i].velocityy < 0)
                {
                    rockets_info[i].treated_once = true;
                    rockets_info[i].isDangerous = IsDangerous(rockets_info[i]);
                    missiles_launched++;
                    Instantiate(missile_prefab);
                }
            }
        }
    }
}
