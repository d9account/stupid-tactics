using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum GameState { INSPECTING, PLAYER_MOVING };

public class GameManager : MonoBehaviour {

    public int default_width = 10;
    public int default_length = 10;
    public float default_size = 1.0f; 

    List<GameObject> tiles = new List<GameObject>();

    private GameObject selectedPlayerUnit;
    private GameState state;

	// Use this for initialization
	void Start () {

        // This is to be used with the final level
        //GameObject[] tileBlocks = GameObject.FindGameObjectsWithTag("tile");
            
        //foreach (GameObject tile in tileBlocks) {
        //    tiles.Add(tile);
        //}
        

        state = GameState.INSPECTING;

        Object TilePrefab = Resources.Load("Tile", typeof(GameObject));
        Object PlayerUnitPrefab = Resources.Load("PlayerUnit", typeof(GameObject));

        if (TilePrefab == null)
        {
            Debug.Log("Tile is Null :(");
            return;
        }

	
		GameObject[] tileBlocks = GameObject.FindGameObjectsWithTag("tile");
			
		foreach (GameObject tile in tileBlocks) {
			tiles.Add(tile);
		}


        // create the default map
        /*for (int l = 0; l < default_length; ++l)
        {
            for (int w = 0; w < default_width; ++w)
            {
                GameObject tile = Instantiate(TilePrefab, 
                                  new Vector3(w * default_size , 0.0f, l * default_size),
                                  Quaternion.identity) as GameObject;
                tiles.Add(tile);
            }
        }
		*/
        // place the player at 50, 50
        GameObject playerUnit = Instantiate(PlayerUnitPrefab,
                                            new Vector3(5.0F, 1.2F, 5.0F),
                                            Quaternion.identity) as GameObject;

        selectedPlayerUnit = playerUnit;

	}


    private GameObject hoverTile;
    private bool mouseDown;

    private Vector3 playerFrom;
    private Vector3 playerTo;
    private float playerMovingTime;

    void Update()
    {
        CameraMotionUpdate();


        switch (state)
        {
            case GameState.INSPECTING:
            {
                // This is the ray casting bit to detect which object is hit
                if (Input.GetMouseButtonDown(0))
                {
                    hoverTile.GetComponent<Renderer>().material.color = Color.cyan;
                    mouseDown = true;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    mouseDown = false;
                    state = GameState.PLAYER_MOVING;

                    playerFrom = selectedPlayerUnit.transform.position;
                    playerTo = new Vector3(hoverTile.transform.position.x, selectedPlayerUnit.transform.position.y, hoverTile.transform.position.z);
                }

                if (!mouseDown)
                {
                    Ray hitRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(hitRay, out hit))
                    {
                        if (hoverTile)
                        {
                            hoverTile.GetComponent<Renderer>().material.color = Color.white;
                        }

                        GameObject hitObj = hit.collider.gameObject;
                        hitObj.GetComponent<Renderer>().material.color = Color.red;
                        hoverTile = hitObj;
                    }
                }

                break;
            }
            case GameState.PLAYER_MOVING:
            {
                playerMovingTime += Time.deltaTime;
                selectedPlayerUnit.transform.position = Vector3.Lerp(playerFrom,
                                                                     playerTo,
                                                                     playerMovingTime);

                if (playerMovingTime >= 1.0F)
                {
                    playerMovingTime = 0.0F;
                    state = GameState.INSPECTING;
                }

                break;
            }
        }
	}

    private Vector3 LastMousePos;
    private bool mousePanning;

    private Vector3 LookatPos;
    private bool mouseRotating;

    void CameraMotionUpdate()
    {
        if (Input.GetMouseButtonDown(2))
        {
            mousePanning = true;
            LastMousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(2))
        {
            if (mousePanning)
                mousePanning = false;
        }

        if (Input.GetMouseButtonDown(1))
        {
            mouseRotating = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            if(mouseRotating)
                mouseRotating = false;
        }

        if (mousePanning)
        {
            Vector3 newMousePos = Input.mousePosition;
            Vector3 mm = (LastMousePos/50 - newMousePos/50);

            Camera.main.transform.position = Camera.main.transform.position + new Vector3(mm.x, 0.0F, mm.y);
            LastMousePos = newMousePos;
        }

        if (mouseRotating)
        {

        }
    }

}