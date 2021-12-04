using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : ChessPiece
{

    public override void Awake()
    {
        base.Awake();
        pieceName = "B";
        pieceValue = 3;
    }
}
