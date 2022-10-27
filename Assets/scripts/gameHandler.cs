using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameHandler : MonoBehaviour {
	public Transform pivots;
	Transform[] xPivots;
	Transform[] yPivots;
	Transform[] zPivots;

	Vector3[] refRot;

	/*toBe Public Static Const later*/
	int cX = 0;	
	int cY = 1;	
	int cZ = 2;	

	public Quaternion startRot;
	public Quaternion endRot;
	public Pivot targetPivot;
	public bool newTurn;
	public bool turning;
	public float ctr;
	int cardinal;

	int scrambleSteps = 6;
	int stepCtr;

    public bool firstPerson;

	public bool setGame;
	public GameObject camera;
	public GameObject nextCamera;

	// Use this for initialization
	void Start () {
        firstPerson = false;

		xPivots = new Transform[3];
		yPivots = new Transform[3];
		zPivots = new Transform[3];
		int type = 0;
		for (int i=0; i<9; i++) {
			type = (i-(i%3))/3;
			if (type==cX) xPivots[(i%3)] = pivots.GetChild(i).transform;
			else if (type==cY) yPivots[(i%3)] = pivots.GetChild(i).transform;
			else if (type==cZ) zPivots[(i%3)] = pivots.GetChild(i).transform;
		}

		Cursor.visible = false;
		newTurn = true;

		refRot = new Vector3[3];
		refRot[0] = Vector3.right;
		refRot[1] = Vector3.up;
		refRot[2] = Vector3.forward;
	}
	
	// Update is called once per frame
	void Update () {
        Cursor.visible = false;
        Scramble();
	}

	void RandomTurn() {
		int pivotI = Mathf.FloorToInt(Random.Range(0,8));
		int dir = Mathf.FloorToInt(Random.Range(0,1.9f));
		if (dir == 0) dir = -1;
		targetPivot = pivots.GetChild(pivotI).GetComponent<Pivot>();
		targetPivot.iParent();

		cardinal = (pivotI-(pivotI%3))/3;

		startRot = targetPivot.transform.rotation;

		endRot = startRot * Quaternion.Euler(refRot[cardinal].x*90*dir,refRot[cardinal].y*90*dir,refRot[cardinal].z*90*dir);

		ctr = 0;
		turning = true;
		newTurn = false;
	}
	void Scramble() {
        if (!setGame && stepCtr < scrambleSteps)
        {
            if (newTurn)
            {
                RandomTurn();
            }
            else if (turning)
            {
                if (ctr >= 1f)
                {
                    ctr = 0f;
                    //targetPivot.dim();
                    targetPivot.unParent();
                    targetPivot = null;
                    newTurn = true;
                    turning = false;
                    stepCtr++;
                }
                else
                {
                    ctr = ctr + Time.deltaTime * 2f;
                    targetPivot.transform.rotation = Quaternion.Slerp(startRot, endRot, ctr);
                }
            }
        }
        else
        {
            setGame = true;
        }
        if (setGame)
        {
            if (firstPerson)
            {
                camera.SetActive(false);
                nextCamera.SetActive(true);
            }
            else
            {
                Cursor.visible = true;
            }
        }
    }
}
