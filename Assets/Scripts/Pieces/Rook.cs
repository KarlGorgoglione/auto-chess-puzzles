using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : ChessPiece
{

    public override void Awake()
    {
        base.Awake();
        pieceName = "R";
        pieceValue = 5;
    }
}
