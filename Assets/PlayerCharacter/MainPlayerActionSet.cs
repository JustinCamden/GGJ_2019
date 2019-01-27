using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class MainPlayerActionSet : PlayerActionSet {

    public PlayerAction moveLeft;
    public PlayerAction moveRight;
    public PlayerAction moveUp;
    public PlayerAction moveDown;
    public PlayerTwoAxisAction moveAxis;
    public PlayerAction jump;
    public PlayerAction interact;

    public MainPlayerActionSet()
    {
        moveLeft = CreatePlayerAction("Move Left");
        moveRight = CreatePlayerAction("Move Right");
        moveUp = CreatePlayerAction("Move Up");
        moveDown = CreatePlayerAction("Move Down");
        moveAxis = CreateTwoAxisPlayerAction(moveLeft, moveRight, moveDown, moveUp);
        jump = CreatePlayerAction("Jump");
        interact = CreatePlayerAction("Interact");
    }
}
