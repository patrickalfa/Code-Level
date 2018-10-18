using System.Collections;
using UnityEngine;

public class Label : MonoBehaviour
{
    [TextArea]
    public string text;
    public Color color;
    public int sortingOrder;
    public Sprite[] _sprites;

    private string lateText = "";
    private int index = 0;

    /////////////////////////////////////////////////////////

    private void Update()
    {
        if (text != lateText)
            ChangeText(text);

        lateText = text;
    }

    /////////////////////////////////////////////////////////

    private void ChangeText(string txt)
    {
        ClearText();

        foreach (char c in txt)
            AddCharacter(c, color);
    }

    private void ClearText()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            for (int i = transform.childCount; i > 0; --i)
                DestroyImmediate(transform.GetChild(0).gameObject);
        }
        else
        {
            foreach (Transform t in transform)
                Destroy(t.gameObject);
        }

        index = 0;
    }

    private void AddCharacter(char character, Color color)
    {
        GameObject c = new GameObject(character.ToString());
        c.AddComponent<SpriteRenderer>().sprite = _sprites[(int)character];
        c.GetComponent<SpriteRenderer>().color = color;
        c.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
        c.transform.parent = transform;
        c.transform.localPosition = Vector3.right * index;

        index++;
    }

    public void UpdateText()
    {
        ChangeText(text);
    }
}
