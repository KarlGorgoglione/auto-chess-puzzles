using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPiece : MonoBehaviour
{

    protected Camera mainCamera;

    public enum PieceColor { Black, White };

    [SerializeField]
    public PieceColor color;

    protected int pieceValue;

    public string pieceName;

    public bool isHeld;

    public bool isOnBoard;

    public bool shouldMove;

    protected float movementTime;
    protected float elapsedTime;

    public Vector3 desiredPosition;

    private void Awake()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        movementTime = 1f;
        elapsedTime = float.MaxValue;
    }

    void Update()
    {
        if (elapsedTime < movementTime)
        {
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Mathf.SmoothStep(0, 1, elapsedTime / movementTime));
            elapsedTime += Time.deltaTime;
        }
        
    }

    private void OnMouseDrag()
    {
        isHeld = true;
        Vector3 coords = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.transform.position.z - 1));
        if (isOnBoard)
        {
            coords = new Vector3(-MathTools.CustomRound(-0.5f, 0.5f, coords.x), transform.position.y, -MathTools.CustomRound(-0.5f, 0.5f, coords.z) - 10f);
        }
        else
        {
            coords = new Vector3(-coords.x, transform.position.y, -coords.z - 10f);
        }

        transform.position = coords;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Square"))
        {
            isOnBoard = true;
        }
    }

    public virtual void updatePosition(Vector3 newPosition)
    {
        desiredPosition = new Vector3(newPosition.x - 3.5f, transform.position.y, newPosition.z - 3.5f);
        elapsedTime = 0f;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Square"))
        {
            isOnBoard = false;
        }
    }

    private void OnMouseUp()
    {
        isHeld = false;
    }
}
