using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct ARGUMENTS
{
    public Vector3 direction;
    public int length;

    public ARGUMENTS(Vector3 direction, int length)
    {
        this.direction = direction;
        this.length = length;
    }
}

public class Commander : MonoBehaviour
{
    public static Commander instance;

    public Transform _cursor;

    [Header("Prefabs")]
    [SerializeField] private GameObject pfGround;

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

    public void Execute(string command, char direction, int length)
    {
        // Check if direction is valid.
        if (direction != 'l' && direction != 'r' && direction != 'd' && direction != 'u')
        {
            Logger.instance.LogError("Incorrect direction: " + direction + ".");
            return;
        }

        // Check negative length
        if (length < 0)
        {
            Logger.instance.LogError("Invalid value.");
            return;
        }

        // Get direction vector.
        Vector3 dir = Vector3.zero;
        switch (direction)
        {
            case 'l': dir = Vector3.left; break;
            case 'r': dir = Vector3.right; break;
            case 'u': dir = Vector3.up; break;
            case 'd': dir = Vector3.down; break;
        }

        // Check there is space for cursor to move
        Vector3 nextCursorPos = _cursor.position + (dir * length);
        if (Mathf.Abs(nextCursorPos.x) > 12 || Mathf.Abs(nextCursorPos.y) > 6)
        {
            Logger.instance.LogMessage("Out of bounds.");
            return;
        }

        // Execute command
        ARGUMENTS args = new ARGUMENTS(dir, length);
        StartCoroutine(command, args);
    }

    // COMMANDS /////////////////////////////////////////////

    private IEnumerator ground(ARGUMENTS args)
    {
        // Build ground
        for (int i = 0; i < args.length; i++)
        {
            yield return new WaitForEndOfFrame();

            Instantiate(pfGround, _cursor.position, Quaternion.identity, transform);
            _cursor.Translate(args.direction);            
        }
    }

    private IEnumerator move(ARGUMENTS args)
    {
        // Build ground
        for (int i = 0; i < args.length; i++)
        {
            yield return new WaitForEndOfFrame();

            _cursor.Translate(args.direction);
        }
    }
}
