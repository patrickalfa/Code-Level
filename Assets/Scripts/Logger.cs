using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public static Logger instance;

    public Transform _cursor;

    private List<GameObject> characters;

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

    private void Start()
    {
        characters = new List<GameObject>();
    }

    /////////////////////////////////////////////////////////

    public void LogMessage(string msg)
    {
        ShowMessage(msg, Color.gray);
    }

    public void LogError(string msg)
    {
        ShowMessage(msg, Color.red);
    }

    private void ShowMessage(string msg, Color color)
    {
        HideMessage();

        foreach (char c in msg)
            AddCharacter(c, color);

        Invoke("HideMessage", 3f);
    }

    private void HideMessage()
    {
        CancelInvoke("HideMessage");

        while (characters.Count > 0)
            DeleteCharacter();
    }

    private void AddCharacter(char character, Color color)
    {
        GameObject c = new GameObject(character.ToString());
        c.AddComponent<SpriteRenderer>().sprite = Typer.instance.sprites[(int)character];
        c.GetComponent<SpriteRenderer>().color = color;
        c.transform.parent = transform;
        c.transform.position = _cursor.position;
        characters.Add(c);

        _cursor.Translate(Vector3.right);
    }

    private void DeleteCharacter()
    {
        int lastIndex = characters.Count - 1;
        GameObject last = characters[lastIndex];

        characters.RemoveAt(lastIndex);
        Destroy(last);

        _cursor.Translate(Vector3.left);
    }
}
