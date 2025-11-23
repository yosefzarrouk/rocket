using JetBrains.Annotations;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
[System.Serializable]
public class rocket_info
{
    public Vector2 velocity;
    public Vector2 position;
    public Vector2 predicted_position;
    public float time_to_interception;
    public bool isDangerous = false;
    public bool isfalling = false;
    public bool isfree = false;
    public bool isintercepted = false;
    public bool exsists = false;
    public rocket_info()
    {
        exsists = false;
        isDangerous = false;
        isfalling = false;
        isfree = false;
        isintercepted = false;
    }
}
public class detect_rockets_and_launch_interceptors : MonoBehaviour
{
    public GameObject interceptor;
    public GameObject[] rockets = new GameObject[101];
    public rocket_info[] rockets_info = new rocket_info[101];
    public int rocket_index = 1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 1; i < rockets_info.Length; i++)
        {
            rockets_info[i].exsists = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Search_a_rocket();
        Copy_all_rockets_infos();
        Check_dangerous_rockets();
        //if (Keyboard.current.spaceKey.wasPressedThisFrame)
        //{
        //Debug.Log(GameObject.Find("rocket 2").GetComponent<Rigidbody2D>().linearVelocity.y);
        //}
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log(rockets_info[2].time_to_interception);
        }
            //Deb ug.Log(rocket_index);
    }
    public void Search_a_rocket()
    {
        rockets[rocket_index] = GameObject.Find("rocket " + rocket_index);
        if (GameObject.Find("rocket " + rocket_index) != null)
        {
            rockets_info[rocket_index] = new rocket_info();
            rockets_info[rocket_index].exsists = true;
            rocket_index++;
        }
    }
    public void Copy_all_rockets_infos()
    {
        for (int i = 1; i < rockets_info.Length; i++)
        {
            Copy_rockets_info(i);
        }
    }
    public void Copy_rockets_info(int index)
    {
        if (rockets_info[index].exsists == true)
        {
            rockets_info[index].position = rockets[index].transform.position;
            rockets_info[index].velocity = rockets[index].GetComponent<Rigidbody2D>().linearVelocity;
            rockets_info[index].predicted_position = calculate_interception_position(index, rockets_info[index].time_to_interception);
            if (rockets[index].GetComponent<rocket_script>().flight_stage == rocket_script.rocket_flight_stages.released)
            {
                rockets_info[index].isfree = true;
            }
            if (rockets_info[index].velocity.y < 0 && rockets_info[index].position.y > 20)
            {
                rockets_info[index].isfalling = true;
            }
        }
    }
    public void Launch_an_interceptor(int index)
    {
        Instantiate(interceptor);
        GameObject.Find("interceptor(Clone)").GetComponent<interceptor_movement>().index = index;
        GameObject.Find("interceptor(Clone)").name = "interceptor " + index;
        Debug.Log("Interceptor launched for rocket " + index);
    }
    public Vector2 calculate_interception_position(int index, float time)
    {
        rocket_info rocket = rockets_info[index];
        float x = rocket.position.x + rocket.velocity.x * time;
        float y = rocket.position.y + rocket.velocity.y * time + Physics2D.gravity.y * time * time / 2;
        return new Vector2(x, y);
    }
    public void Check_dangerous_rockets()
    {
        for (int index = 1; index < rocket_index; index++)
        {
            if (rockets_info[index] != null)
            {
                if (rockets_info[index].isfalling == true && rockets_info[index].isintercepted == false && rockets_info[index].isfree == true)
                {
                    Launch_an_interceptor(index);
                    rockets_info[index].isintercepted = true;
                }
            }
        }
    }
}
