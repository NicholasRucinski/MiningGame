
using UnityEngine;

public abstract class InputController : ScriptableObject
{
    public abstract float RetrieveMoveInput();

    public abstract bool RetrieveJumpInput();

    public abstract bool RetrieveDashInput();

    public abstract bool RetrieveHoldInput();

    public abstract bool RetrieveAttackInput();

    public abstract bool RetrieveMineInput();

    public abstract float RetrieveUpDownInput();
}
