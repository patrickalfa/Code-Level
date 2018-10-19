using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct COMMAND
{
    public string name;
    public int minArgs;
    public int maxArgs;
}

public class Typer : MonoBehaviour
{
    public static Typer instance;

    public string prompt;
    public int maxLength;
    public Sprite[] sprites;
    public COMMAND[] commands;

    public Transform _cursor;

    private List<GameObject> characters;
    private bool isCommand;
    private string lastPrompt;
    private int index;

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
        _cursor.GetComponent<SpriteRenderer>();
        lastPrompt = "";
        prompt = "";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            UpdatePrompt(lastPrompt);
        if (Input.GetKeyDown(KeyCode.DownArrow))
            ClearPrompt();
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            MoveCursor(-1);
        if (Input.GetKeyDown(KeyCode.RightArrow))
            MoveCursor(1);

        CheckKeys();
        CheckCommand();
	}

    /////////////////////////////////////////////////////////

    private void MoveCursor(int direction)
    {
        direction = Mathf.Clamp(direction, -1, 1);
        index = Mathf.Clamp(index + direction, 0, prompt.Length);

        _cursor.transform.localPosition = Vector3.right * (-12 + index);
    }

    private void UpdatePrompt(string newPrompt)
    {
        if (newPrompt.Length == 0)
            return;

        ClearPrompt();

        foreach (char c in newPrompt)
            AddCharacter(c);
    }

    private void ClearPrompt()
    {
        prompt = "";
        index = 0;

        while (characters.Count > 0)
        {
            GameObject g = characters[0];

            characters.RemoveAt(0);
            Destroy(g);
        }

        MoveCursor(0);
    }

    private void CheckKeys()
    {
        if (Input.inputString.Length == 0 || Input.inputString[0] > 127)
            return;

        if (Input.inputString[0] == 8) // BACKSPACE
            DeleteCharacter();
        else if (Input.inputString[0] == 13) // ENTER
            CheckBash();
        else
        {
            if (index < prompt.Length)
            {
                int lastIndex = index;
                UpdatePrompt(prompt.Insert(index, Input.inputString[0].ToString()));
                index = lastIndex + 1;
                MoveCursor(0);
            }
            else
                AddCharacter(Input.inputString[0]);
        }
    }

    private void AddCharacter(char character)
    {
        if (prompt.Length >= maxLength)
            return;

        string s = character.ToString();
        prompt += s;

        GameObject c = new GameObject(s);
        c.AddComponent<SpriteRenderer>().sprite = sprites[(int)character];
        c.transform.parent = transform;
        c.transform.position = _cursor.position;
        characters.Add(c);

        MoveCursor(1);
    }

    private void DeleteCharacter()
    {
        if (prompt.Length <= 0 || index < 1)
            return;

        int lastIndex = index - 1;
        GameObject last = characters[lastIndex];

        prompt = prompt.Remove(lastIndex, 1);
        characters.RemoveAt(lastIndex);
        Destroy(last);

        if (index < (prompt.Length + 1))
        {
            UpdatePrompt(prompt);
            index = lastIndex + 1;
        }

        MoveCursor(-1);
    }

    private COMMAND? GetCommandByName(string name)
    {
        foreach (COMMAND c in commands)
        {
            if (c.name == name)
                return c;
        }

        return null;
    }

    private void CheckCommand()
    {
        string[] ss = prompt.Split(' ');

        isCommand = (GetCommandByName(ss[0]) != null);

        for (int i = 0; i < ss[0].Length; i++)
            characters[i].GetComponent<SpriteRenderer>().color = (isCommand ? Color.green : Color.white);
    }

    private void CheckBash()
    {
        prompt = prompt.TrimEnd(' ');
        lastPrompt = prompt;
        string[] ss = prompt.Split(' ');

        ClearPrompt();

        // SYNTAX CHECKS -----------------------------------

        if (!isCommand)
        {
            Logger.instance.LogError("Unknown command: " + ss[0] + ".");
            return;
        }
        else
        {
            COMMAND? cmd = GetCommandByName(ss[0]);

            if (ss.Length < cmd.Value.minArgs + 1)
                Logger.instance.LogError("Missing arguments.");
            else if (ss.Length > cmd.Value.maxArgs + 1)
                Logger.instance.LogError("Invalid syntax.");
            else
            {
                int length = 1;
                if (ss.Length == 3 && (!int.TryParse(ss[2], out length) || ss[1].Length == 0))
                    Logger.instance.LogError("Invalid value.");
                else
                {
                    char direction = '\n';

                    if (ss.Length > 1)
                    {
                        if (ss[1].Length > 1)
                        {
                            Logger.instance.LogError("Invalid direction: " + ss[1] + ".");
                            return;
                        }

                        direction = ss[1][0];
                    }

                    Commander.instance.Execute(ss[0], direction, length);
                }
            }
        }
    }
}
