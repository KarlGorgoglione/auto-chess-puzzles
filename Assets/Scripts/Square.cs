using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    string squareName;

    public ChessPiece squarePiece;

    public Vector3 squarePosition;

    public (int, int) gridPosition;


    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Piece") && GameManager.Instance.mode == GameManager.Mode.Placement)
        {
            ChessPiece piece = other.GetComponent<ChessPiece>();
            squarePiece = piece;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Piece") && GameManager.Instance.mode == GameManager.Mode.Placement)
        {
            squarePiece = null;
        }
    }

    public void assignPiece(ChessPiece piece)
    {
        squarePiece = piece;
        piece.updatePosition(squarePosition);
    }

    public void removePiece()
    {
        squarePiece.gameObject.SetActive(false);
        squarePiece = null;
    }

}
