using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Version : MonoBehaviour
{
    public Transform _solution;
    public Bot _bot;
    public Bug[] _bugs;
    public int fixes;

    public void Compile()
    {
        if (fixes == _bugs.Length)
            Success();
        else
            Failure();
    }

    public void Success()
    {
        GameManager.instance.FreezeFrame(.25f);
        GameManager.instance.ScreenShake(.25f, 1f, 50);

        _bot.GetComponent<Collider2D>().enabled = false;
        _bot.GetComponent<Rigidbody2D>().simulated = false;

        _solution.GetComponent<SpriteRenderer>().sortingOrder = 6;
        _solution.transform.DOScale(15f, .25f).SetUpdate(true);
        _solution.transform.DOBlendableRotateBy(Vector3.forward * 180f, .25f)
            .SetUpdate(true).OnComplete(() =>
            {
                GameManager.instance.NextVersion();
                Restart();
            });

        SoundManager.PlaySound("solution", false, .7f);
    }

    public void Failure()
    {
        GameManager.instance.FreezeFrame(.1f);
        GameManager.instance.ScreenShake(.1f, 1f, 50);

        Logger.instance.LogMessage("Must fix all bugs.");

        Restart();

        SoundManager.PlaySound("reset");
    }

    public void Restart()
    {
        fixes = 0;

        _bot.Restart();
        foreach (Bug b in _bugs)
            b.Restart();

        Commander.instance.ResetTiles();

        Laser[] lasers = GetComponentsInChildren<Laser>(true);
        foreach (Laser l in lasers)
            l.Restart();

        _solution.GetComponent<SpriteRenderer>().sortingOrder = -1;
    }
}
