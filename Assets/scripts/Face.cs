using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : MonoBehaviour {

	public int cardinal;
	public int valence;
	public int type;

	public Pivot xPivot;
	public Pivot yPivot;
	public Pivot zPivot;
	public Pivot[] pivots;

	/*toBe Public Static Const later*/
	int NEG = 0;	// Negative
	int POS = 1;	// Positive
	/*toBe Public Static Const later*/
	int cX = 0;	
	int cY = 1;	
	int cZ = 2;	

	// Use this for initialization
	void Start () {
		type = 3*valence + cardinal;
		pivots = new Pivot[3];
		pivots[cX] = xPivot;
		pivots[cY] = yPivot;
		pivots[cZ] = zPivot;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
