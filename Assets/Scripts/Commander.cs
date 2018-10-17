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
    public Transform _tilesParent;

    [Header("Prefabs")]
    [SerializeField] private GameObject pfGround;
    [SerializeField] private GameObject pfSpring;
    [SerializeField] private GameObject pfLaser;


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
        // Check null or negative length
        if (length < 1)
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
            case '\n': dir = Vector3.zero; break;
            default:
                Logger.instance.LogError("Incorrect direction: " + direction + ".");
                return;
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

    private void DestroyTileOnCursor()
    {
        Collider2D hit = Physics2D.OverlapCircle(_cursor.position, .25f, LayerMask.GetMask("Tile"));

        if (hit)
            Destroy(hit.gameObject);
    }

    // COMMANDS /////////////////////////////////////////////

    private IEnumerator ground(ARGUMENTS args)
    {
        // Build ground
        for (int i = 0; i < args.length; i++)
        {
            yield return new WaitForEndOfFrame();

            DestroyTileOnCursor();

            Instantiate(pfGround, _cursor.position, Quaternion.identity, _tilesParent);
            _cursor.Translate(args.direction);            
        }
    }

    private IEnumerator spring(ARGUMENTS args)
    {
        // Build spring
        yield return new WaitForEndOfFrame();

        DestroyTileOnCursor();
        Instantiate(pfSpring, _cursor.position, Quaternion.identity, _tilesParent);
    }

    private IEnumerator laser(ARGUMENTS args)
    {
        // Build laser
        yield return new WaitForEndOfFrame();

        DestroyTileOnCursor();
        Instantiate(pfLaser, _cursor.position, Quaternion.identity, _tilesParent);
    }

    private IEnumerator move(ARGUMENTS args)
    {
        // Move cursor
        for (int i = 0; i < args.length; i++)
        {
            yield return new WaitForEndOfFrame();

            _cursor.Translate(args.direction);
        }
    }

    private IEnumerator delete(ARGUMENTS args)
    {
        // Delete tiles
        for (int i = 0; i < args.length; i++)
        {
            yield return new WaitForEndOfFrame();
                       
            DestroyTileOnCursor();
            _cursor.Translate(args.direction);            
        }
    }

    private IEnumerator clear(ARGUMENTS args)
    {
        // Clear tiles
        while (_tilesParent.childCount > 1)
        {
            yield return new WaitForEndOfFrame();

            Destroy(_tilesParent.GetChild(0).gameObject);
        }
    }

    private IEnumerator reset(ARGUMENTS args)
    {
        yield return new WaitForEndOfFrame();
    }

    private IEnumerator build(ARGUMENTS args)
    {
        yield return new WaitForEndOfFrame();
    }
}
