using UnityEngine;

public class force : MonoBehaviour
{
    public Rigidbody2D rb;
    public void moveforward_byforce (float force)
    {
        rb.AddForce(transform.forward *  force);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveforward_byforce(50000000);
    }
}
