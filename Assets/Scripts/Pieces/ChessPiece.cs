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

    Vector3 startPos;

    Square activeSquare;

    MeshRenderer meshRenderer;

    [SerializeField]
    bool isMovable;


    private void Awake()
    {
        movementTime = 0.7f;
        elapsedTime = float.MaxValue;
        plane = new Plane(Vector3.up, Vector3.zero); // ground plane
        meshRenderer = GetComponent<MeshRenderer>();
        if(!isMovable && color == PieceColor.White)
        {
            meshRenderer.material.SetFloat("_ShnIntense", 0.5f);
            meshRenderer.material.SetFloat("_ShnRange", 0.5f);
        }
    }

    private void Start()
    {
    }

    void Update()
    {
        if (elapsedTime < movementTime)
        {
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Mathf.SmoothStep(0, 1, elapsedTime / movementTime));
            elapsedTime += Time.deltaTime;
        }
        
    }

    public void resetColor()
    {
        meshRenderer.material.SetFloat("_ShnIntense", 0f);
        meshRenderer.material.SetFloat("_ShnRange", 0f);
    }


    private void OnMouseDrag()
    {
        if (color == PieceColor.White && isMovable)
        {
            // with Raycast
            Debug.Log(plane.normal);
            isHeld = true;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance; // the distance from the ray origin to the ray intersection of the plane

            if (plane.Raycast(ray, out distance))
            {
                Vector3 dest = ray.GetPoint(distance);
                if (isOnBoard)
                {
                    transform.position = new Vector3(
                        MathTools.CustomRound(-0.5f, 0.5f, dest.x),
                        dest.y,
                        MathTools.CustomRound(-0.5f, 0.5f, dest.z)); // distance along the ray
                }
                else
                {
                    transform.position = dest;
                }
            }
        }
        
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
