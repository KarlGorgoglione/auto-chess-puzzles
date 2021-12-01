using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPiece : MonoBehaviour
{

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

    Plane plane;

    Vector3 dist;
    Vector3 startPos;
    float posX;
    float posZ;
    float posY;

    Square activeSquare;


    private void Awake()
    {
        movementTime = 1f;
        elapsedTime = float.MaxValue;
    }

    private void Start()
    {
        plane = new Plane(Vector3.up, Vector3.up * GameManager.Instance.groundLevel); // ground plane
    }

    void Update()
    {
        if (elapsedTime < movementTime)
        {
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Mathf.SmoothStep(0, 1, elapsedTime / movementTime));
            elapsedTime += Time.deltaTime;
        }
        
    }

    void OnMouseDown()
    {
        if (color == PieceColor.White)
        {
            startPos = transform.position;
            dist = Camera.main.WorldToScreenPoint(transform.position);
            posX = Input.mousePosition.x - dist.x;
            posY = Input.mousePosition.y - dist.y;
            posZ = Input.mousePosition.z - dist.z;
        }
    }

    private void OnMouseDrag()
    {
        if (color == PieceColor.White)
        {
            float disX = Input.mousePosition.x - posX;
            float disY = Input.mousePosition.y - posY;
            float disZ = Input.mousePosition.z - posZ;
            Vector3 lastPos = Camera.main.ScreenToWorldPoint(new Vector3(disX, disY, disZ));
            if (isOnBoard)
            {
                transform.position = new Vector3(
                    MathTools.CustomRound(-0.5f, 0.5f, lastPos.x),
                    startPos.y,
                    MathTools.CustomRound(-0.5f, 0.5f, lastPos.z));
            }
            else
            {
                transform.position = new Vector3(lastPos.x, startPos.y, lastPos.z);
            }
        }

        /*
        // with Raycast
        Debug.Log(plane);
        isHeld = true;
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.transform.position.z - 0.1f);
        Debug.Log(ray);
        float distance; // the distance from the ray origin to the ray intersection of the plane

        if (plane.Raycast(ray, out distance))
        {
            Debug.Log("HIT");
            Vector3 dest = ray.GetPoint(distance);
            if (isOnBoard)
            {
                transform.position = new Vector3(
                    -MathTools.CustomRound(-0.5f, 0.5f, dest.x),
                    dest.y,
                    -MathTools.CustomRound(-0.5f, 0.5f, dest.z)); // distance along the ray
            }
            else
            {
                transform.position = dest;
            }
        }*/


        /*Vector3 coords = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.transform.position.z - 1));
        if (isOnBoard)
        {
            coords = new Vector3(-MathTools.CustomRound(-0.5f, 0.5f, coords.x), transform.position.y, -MathTools.CustomRound(-0.5f, 0.5f, coords.z) - 10f);
        }
        else
        {
            coords = new Vector3(-coords.x, transform.position.y, -coords.z - 10f);
        }

        transform.position = coords;*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Square"))
        {
            isOnBoard = true;
            activeSquare = other.GetComponent<Square>();
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
        if (color == PieceColor.White)
        {
            isHeld = false;
            if (activeSquare.squarePiece != this) transform.position = startPos;
        }
    }
}
