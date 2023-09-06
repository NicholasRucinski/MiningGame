using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Block : NetworkBehaviour
{

    public GameObject hover;
    public GameObject north;
    public GameObject south;
    public GameObject east;
    public GameObject west;
    public GameObject northEast;
    public GameObject southWest;
    public GameObject southEast;
    public GameObject northWest;
    public GameObject selectSprite;
    public GameObject cover;

    private GameObject player;
    private float blockStartingHealth = 100;
    [SerializeField]
    private float blockHealth = 100;
    NetworkVariable<bool> hasNorth;
    NetworkVariable<bool> hasSouth;
    NetworkVariable<bool> hasWest;
    NetworkVariable<bool> hasEast;
    [SerializeField]
    private Sprite[] blockStates;
    public SpriteRenderer breakingTexture;

    /*
    public bool hasNorth = false;
    public bool hasSouth = false;
    public bool hasWest = false;
    public bool hasEast = false;
    */
    private MapGenerator map;
    private int xPos;
    private int yPos;

    public GameObject itemDrop;

    private void Awake()
    {
        hasNorth = new NetworkVariable<bool>(false);
        hasSouth = new NetworkVariable<bool>(false);
        hasWest = new NetworkVariable<bool>(false);
        hasEast = new NetworkVariable<bool>(false);
        
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        map = GameObject.FindGameObjectWithTag("Map").GetComponent<MapGenerator>();
        updateNeighbors();
    }


    private void OnMouseEnter()
    {
        selectSprite.SetActive(true);
    }
    private void OnMouseExit()
    {
        selectSprite.SetActive(false);
    }


    public void Mine(int mineSpeed, float playerMineDistance)
    {
        if (Vector3.Distance(player.transform.position, this.transform.position) <= playerMineDistance)
        {
            if (blockHealth <= 0)
            {
                map.updateBlock(xPos, yPos, 0);

                DropContents();

                //map.updateGameObjects(xPos, yPos);
                map.updateMap(xPos, yPos);
            }
            else
            {
                blockHealth -= mineSpeed;
                updateBlockState();
            }
        }
        

    }

    private void updateBlockState()
    {
        if (blockHealth/blockStartingHealth >= 0.8f)
        {
            breakingTexture.sprite = blockStates[0];
        } else if (blockHealth / blockStartingHealth >= 0.6f)
        {
            breakingTexture.sprite = blockStates[1];
        } else if (blockHealth / blockStartingHealth >= 0.4f)
        {
            breakingTexture.sprite = blockStates[2];
        } else if (blockHealth / blockStartingHealth >= 0.2f)
        {
            breakingTexture.sprite = blockStates[3];
        } else if (blockHealth / blockStartingHealth >= 0f)
        {
            breakingTexture.sprite = blockStates[4];
        }
    }
    public void updateNeighbors()
    {
        hasNorth.Value = map.getNorth(xPos,yPos);
        hasSouth.Value = map.getSouth(xPos, yPos);
        hasWest.Value = map.getWest(xPos, yPos);
        hasEast.Value = map.getEast(xPos, yPos);

        if (!hasNorth.Value || !hasSouth.Value || !hasWest.Value || !hasEast.Value)
        {
            cover.SetActive(false);
        }

        if (!hasNorth.Value)
        {
            north.SetActive(true);
        }
        
        if (!hasSouth.Value)
        {
            south.SetActive(true);
        }
        
        if (!hasWest.Value)
        {
            west.SetActive(true);
        }
        
        if (!hasEast.Value)
        {
            east.SetActive(true);
        }

        if (hasWest.Value && !hasNorth.Value)
        {
            northWest.SetActive(true);
        }

        if (hasEast.Value && !hasNorth.Value)
        {
            northEast.SetActive(true);
        }

        if (hasWest.Value && !hasSouth.Value)
        {
            southWest.SetActive(true);
        }

        if (hasEast.Value && !hasSouth.Value)
        {
            southEast.SetActive(true);
        }

        if (hasSouth.Value && !hasWest.Value)
        {
            southWest.SetActive(true);
        }

        if (hasSouth.Value && !hasEast.Value)
        {
            southEast.SetActive(true);
        }

        if (hasNorth.Value && !hasWest.Value)
        {
            northWest.SetActive(true);
        }

        if (hasNorth.Value && !hasEast.Value)
        {
            northEast.SetActive(true);
        }


    }
    public void setPos(int x, int y)
    {
        xPos = x;
        yPos = y;
    }
    private void DropContents()
    {
        if (itemDrop != null)
        {
            Instantiate(itemDrop, gameObject.transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
    public void SetSelected(bool sel)
    {
        selectSprite.SetActive(sel);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //this.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //this.gameObject.SetActive(false);
        }
    }
}
