using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipableItems : MonoBehaviour
{
    [SerializeField, Range(0.1f, 4f)]
    private float mineRangeX = 0.5f;
    [SerializeField, Range(0.1f, 4f)]
    private float mineRangeY = 0.5f;
    public Vector2 mineVector;
    [Range(1, 200)]
    public int mineSpeed = 50;
    public LayerMask blockLayers;

    [Range(0.1f, 5f)]
    public float attackRate = 2f;
    public float nextAttackTime = 0f;
    public float interactDistance = 2;

    private void Awake()
    {
        mineVector = new Vector2(mineRangeX, mineRangeY);
    }
}
