using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
{

    public bool hasMoved;

    public override void Awake()
    {
        base.Awake();
        hasMoved = false;
        pieceName = "K";
    }

    public override void updatePosition(Vector3 newPosition)
    {
        base.updatePosition(newPosition);
        if (!hasMoved) hasMoved = true;
    }
}
