using UnityEngine;
using System.Collections;

public class LevelGrid : MonoBehaviour {

    public bool draw = false;

    public float height;
    public float width;
    public float depth;



    public float gridSizeX;
    public float gridSizeY;
    public float gridSizeZ;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDrawGizmos()
    {
        if (!draw)
            return;

       
                 Vector3 pos = Camera.main.transform.position;


        for (float x = pos.x - gridSizeX; x <= pos.x + gridSizeX; x += width)
        {



            Gizmos.DrawLine(new Vector3(Mathf.Floor(x / width) * width, -gridSizeY, 0.0f),
                            new Vector3(Mathf.Floor(x / width) * width, gridSizeY, 0.0f));
        }

        for (float y = pos.y - (gridSizeY+height); y <= pos.y + (gridSizeY-height); y += height)
        {
            Gizmos.DrawLine(new Vector3(-gridSizeX, Mathf.Floor(y / height) * height, 0.0f),
                            new Vector3(gridSizeX, Mathf.Floor(y / height) * height, 0.0f));
        }
    }
    

}


