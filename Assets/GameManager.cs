using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public int default_width = 10;
    public int default_length = 10;
    public float default_size = 1.0f; 

    List<GameObject> tiles = new List<GameObject>();

    private GameObject selectedPlayerUnit;

	// Use this for initialization
	void Start () {
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
    void Update()
    {
        CameraMotionUpdate();

        // This is the ray casting bit to detect which object is hit
        if (Input.GetMouseButtonDown(0))
        {
            hoverTile.GetComponent<Renderer>().material.color = Color.cyan;
            mouseDown = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            mouseDown = false;
            Vector3 from = selectedPlayerUnit.transform.position;
            Vector3 to = hoverTile.transform.position;
            to.y = from.y;
            selectedPlayerUnit.transform.position = Vector3.Lerp(from,
                                                      to,
                                                      1.0F);

        }
        
        if(!mouseDown)
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
                //Mesh mesh = hitObj.GetComponent<MeshFilter>().mesh;
                //Vector3[] verts = mesh.vertices;
                //Color[] colors = new Color[verts.Length];

                //for (var i = 0; i < verts.Length; i++)
                //{
                //    colors[i] = new Color(1.0F, 0.0F, 1.0F, 1.0F);
                //}
                //mesh.colors = colors;
                //mesh.RecalculateNormals();

                hitObj.GetComponent<Renderer>().material.color = Color.red;
                hoverTile = hitObj;
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