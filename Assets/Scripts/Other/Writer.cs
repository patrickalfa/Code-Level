using System.Collections;
using UnityEngine;

public class Writer : MonoBehaviour
{
    public float timeBetweenCharacters;
    public Transform _cursor;
    public GameObject[] _characters;
    public bool finished;

    private int index;

    private void Start()
    {
        _cursor.position = _characters[0].transform.position;
        foreach (GameObject c in _characters)
            c.SetActive(false);

        index = 0;
        finished = false;

        StartCoroutine(WriteCharacters());
    }

    public void Skip()
    {
        finished = true;

        StopAllCoroutines();
        
        for (int i = index; i < _characters.Length; i++)
            _characters[i].SetActive(true);

        _cursor.gameObject.SetActive(false);

        SoundManager.PlaySound("bug");
    }

    private IEnumerator WriteCharacters()
    {
        while (index < _characters.Length)
        {
            yield return new WaitForSeconds(timeBetweenCharacters);

            _characters[index].SetActive(true);
            index++;

            if (index == _characters.Length)
                _cursor.gameObject.SetActive(false);
            else
                _cursor.position = _characters[index].transform.position;
        }

        finished = true;
    }
}
