﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Version[] _versions;
    public Label _labelVersion;

    private int currentVersion;

    /////////////////////////////////////////////////////////

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    /////////////////////////////////////////////////////////

    public void NextVersion()
    {
        _versions[currentVersion].gameObject.SetActive(false);

        Commander.instance.ClearTiles();

        currentVersion++;
        if (currentVersion < _versions.Length)
        {
            _versions[currentVersion].gameObject.SetActive(true);
            _labelVersion.text = _versions[currentVersion].name;
        }
        else
            Debug.Break(); // DEBUG
    }
}