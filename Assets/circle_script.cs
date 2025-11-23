using UnityEngine;
public class circle_script : MonoBehaviour
{
    public movement script;
    public movement missile_script;
    public float time;
    public Position positioninseconds;
    bool once = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (script.v.y < 0 && once == false)
        {
            positioninseconds = Calculate_Position_of_rocket_in(7f);
            transform.position = new Vector3(positioninseconds.x, positioninseconds.y, 0);
            once = true;

        }
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
    public Position Calculate_Position_of_rocket_in(float time)
    {
        float x = script.transform.position.x + script.v.x * time;
        float y = script.transform.position.y + script.v.y * time + Physics2D.gravity.y * time * time / 2;
        return new Position(x, y);
    }
}

