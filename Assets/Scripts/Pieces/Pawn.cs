using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{

    public override void Awake()
    {
        base.Awake();
        pieceName = "P";
        pieceValue = 1;
    }
}
