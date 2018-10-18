using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    public float speed;
    public float impulse;
    public bool running;

    private Transform _trf;
    private Rigidbody2D _rgbd;
    private Vector3 _startingPosition;

    /////////////////////////////////////////////////////////

    private void Start()
    {
        _trf = transform;
        _rgbd = GetComponent<Rigidbody2D>();
        _rgbd.simulated = false;
        _startingPosition = _trf.position;
        running = false;
    }

    private void FixedUpdate()
    {
        if (running)
        {
            Vector2 vel = _rgbd.velocity;
            vel.x = speed;

            _rgbd.velocity = vel;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("Solution"))
            FindObjectOfType<Version>().Compile();
        else if (collision.name.Contains("Laser"))
            collision.GetComponent<Laser>().Fire(Vector3.right);
        else if (collision.name.Contains("Spring"))
            Jump();
    }

    /////////////////////////////////////////////////////////

    public void Run()
    {
        running = true;
        _rgbd.simulated = true;
    }

    public void Jump()
    {
        Vector2 vel = _rgbd.velocity;
        vel.y = 0;
        _rgbd.velocity = vel;
        _rgbd.AddForce(Vector2.up * impulse, ForceMode2D.Impulse);
    }

    public void Restart()
    {
        running = false;
        _rgbd.simulated = false;
        _trf.position = _startingPosition;
    }
}
