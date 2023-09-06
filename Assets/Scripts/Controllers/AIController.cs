using UnityEngine;

[CreateAssetMenu(fileName = "AIController", menuName = "InputController/AIController")]
public class AIController : InputController
{
    public override bool RetrieveJumpInput() {
        return true;
    }

    public override float RetrieveMoveInput() {
        return 1f;
    }

    public override bool RetrieveDashInput()
    {
        return false;
    }

    public override bool RetrieveHoldInput()
    {
        return false;
    }

    public override bool RetrieveAttackInput()
    {
        return false;
    }

    public override float RetrieveUpDownInput()
    {
        return 1;
    }

    public override bool RetrieveMineInput()
    {
        return false;
    }
}
