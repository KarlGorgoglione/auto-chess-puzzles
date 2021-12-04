using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessPiece
{

    public override void Awake()
    {
        base.Awake();
        pieceName = "N";
        pieceValue = 3;
    }
}
