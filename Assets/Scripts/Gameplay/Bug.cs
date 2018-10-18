using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug : MonoBehaviour
{
    private Transform _trf;
    private Vector3 _startingPosition;

    /////////////////////////////////////////////////////////

    private void Start()
    {
        _trf = transform;
        _startingPosition = _trf.position;
    }

    /////////////////////////////////////////////////////////

    public void Fix()
    {
        gameObject.SetActive(false);
        FindObjectOfType<Version>().fixes++;
    }

    public void Restart()
    {
        _trf.position = _startingPosition;
        gameObject.SetActive(true);
    }
}
