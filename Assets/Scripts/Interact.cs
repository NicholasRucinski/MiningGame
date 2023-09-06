using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Interact : NetworkBehaviour
{

    public GameObject currentlyEquipedItem;
    private EquipableItems equiped;
    [SerializeField]
    private Transform minePoint;

    [SerializeField]
    private InputController input = null;
    
    [SerializeField]
    private Camera mainCamera;

    private void Awake()
    {
        currentlyEquipedItem.GetComponent<EquipableItems>();
    }

    void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        Vector3 newPosition = new Vector3(input.RetrieveMoveInput(), input.RetrieveUpDownInput());

        if (newPosition != Vector3.zero)
        {
            minePoint.transform.position = newPosition + transform.position;
        }

        if (Time.time >= equiped.nextAttackTime)
        {
            if (input.RetrieveMineInput() && enabled)
            {
                doMine();
                equiped.nextAttackTime = Time.time + 1f / equiped.attackRate;
            }
        }


    }

    void doMine()
    {
        //Collider2D hitBlock = Physics2D.OverlapCapsule(minePoint.position, mineVector, CapsuleDirection2D.Vertical, blockLayers);
        Collider2D hitBlock = Physics2D.OverlapPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
        if (hitBlock.GetComponent<Block>() != null)
        {
            hitBlock.GetComponent<Block>().Mine(equiped.mineSpeed, equiped.interactDistance);
        }

    }
}
