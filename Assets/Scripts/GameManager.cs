using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Inventory inv;
    public GameObject playerPrefab;
    public InventoryItemData ironOreDrop;
    public InventoryItemData diamondOreDrop;
    public InventoryItemData stoneDrop;

    public TextMeshProUGUI ironText;
    public TextMeshProUGUI diamondText;
    public TextMeshProUGUI stoneText;
    [Header("Timer Settings")]
    public TextMeshProUGUI timerText;
    public float timer = 600f;

    [SerializeField]
    private bool isHost = false;

    void Start()
    {
        playerPrefab = GameObject.FindGameObjectWithTag("Player");
        if (isHost)
        {
            NetworkManager.Singleton.StartHost();
        }
        else
        {
            NetworkManager.Singleton.StartClient();
        }
        
        inv = GameObject.FindFirstObjectByType<Inventory>();
        ironText.text = "0";
        diamondText.text = "0";
        stoneText.text = "0";
        timerText.text = "";
    }

    
    void Update()
    {
        if (playerPrefab == null)
        {
            playerPrefab = GameObject.FindGameObjectWithTag("Player");
        }
        if (inv == null)
        {
            inv = GameObject.FindFirstObjectByType<Inventory>(); ;
        }
        if (inv.Get(ironOreDrop) != null)
        {
            ironText.text = inv.Get(ironOreDrop).stackSize.ToString();

        }
        if (inv.Get(diamondOreDrop) != null)
        {
            diamondText.text = inv.Get(diamondOreDrop).stackSize.ToString();

        }
        if (inv.Get(stoneDrop) != null)
        {
            stoneText.text = inv.Get(stoneDrop).stackSize.ToString();

        }
    }

    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        float seconds = Mathf.FloorToInt(timer % 60);
        float minutes = Mathf.FloorToInt(timer / 60);
        timerText.text = "Time: " + minutes + ":" + seconds;
    }

}


