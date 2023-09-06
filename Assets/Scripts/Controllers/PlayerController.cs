using System;
using UnityEngine;

[CreateAssetMenu(fileName ="PlayerController",menuName ="InputController/PlayerController")]
public class PlayerController : InputController
{
    public override bool RetrieveJumpInput() {
        Debug.Log("Jump");
        return Input.GetButtonDown("Jump");
    }

    public override float RetrieveMoveInput() {
        return Input.GetAxisRaw("Horizontal");
    }

    public override bool RetrieveDashInput()
    {
        return Input.GetButtonDown("Dash");
    }

    public override bool RetrieveHoldInput()
    {
        return Input.GetButton("Jump");
    }

    public override float RetrieveUpDownInput()
    {
        return Input.GetAxisRaw("Vertical");
    }

    public override bool RetrieveAttackInput()
    {
        return Input.GetButtonDown("Attack");
    }

    public override bool RetrieveMineInput()
    {
        return Input.GetButton("Mine");
    }
}
