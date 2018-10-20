using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
        GetComponent<Collider2D>().enabled = false;

        GameManager.instance.FreezeFrame(.15f);
        _trf.DOShakePosition(.15f, 1f, 50).SetUpdate(true).OnComplete(() =>
        {
            gameObject.SetActive(false);
            FindObjectOfType<Version>().fixes++;
        });

        SoundManager.PlaySound("bug");
    }

    public void Restart()
    {
        _trf.position = _startingPosition;
        gameObject.SetActive(true);
        GetComponent<Collider2D>().enabled = true;
    }
}
