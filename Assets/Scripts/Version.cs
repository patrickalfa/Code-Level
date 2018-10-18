using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Version : MonoBehaviour
{
    public Bot _bot;
    public Bug[] _bugs;
    public int fixes;

    public void Compile()
    {
        if (fixes == _bugs.Length)
            Success();
        else
            Logger.instance.LogMessage("Must fix all bugs.");

        Restart();
    }

    public void Success()
    {
        GameManager.instance.NextVersion();
    }

    public void Restart()
    {
        fixes = 0;
        _bot.Restart();
        foreach (Bug b in _bugs)
            b.Restart();

        Commander.instance.ResetTiles();
    }
}
