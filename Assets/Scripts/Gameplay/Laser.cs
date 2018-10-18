using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed;

    private Transform _trf;
    private Vector3 _startingPosition;

    private bool fired;
    private Vector2 direction;

    /////////////////////////////////////////////////////////

    private void Start()
    {
        _trf = transform;
        _startingPosition = _trf.position;
        fired = false;
    }

    private void Update()
    {
        if (fired)
            _trf.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!fired)
            return;

        if (collision.name.Contains("Bug"))
            collision.GetComponent<Bug>().Fix();

        if (!collision.name.Contains("Bot"))
            gameObject.SetActive(false);
    }

    /////////////////////////////////////////////////////////

    public void Fire(Vector2 dir)
    {
        fired = true;
        direction = dir;
        _trf.localScale = new Vector3(1f, .25f, 1f);
    }

    public void Restart()
    {
        fired = false;
        _trf.position = _startingPosition;
        _trf.localScale = Vector3.one;
        gameObject.SetActive(true);
    }
}