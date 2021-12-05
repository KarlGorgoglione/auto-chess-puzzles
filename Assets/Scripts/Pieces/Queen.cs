using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : ChessPiece
{

    public override void Awake() // POLYMORPHISM
    {
        base.Awake();
        pieceName = "Q";
        pieceValue = 9;

    }
}
