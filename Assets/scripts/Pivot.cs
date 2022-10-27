using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pivot : MonoBehaviour {
	public BitBank bank;
	public int[] indices;
	public int centerIndx;

	public int cardinal;
	public int valence;
	/*toBe Public Static Const later*/
	int NGA = 0;	// Negative
	int EQL = 1;	// Equal: 0
	int GRT = 2;	// Greater
	/*toBe Public Static Const later*/
	int cX = 0;	
	int cY = 1;	
	int cZ = 2;	

	bool lit;
	public Material guideMat;
	float ctr;
	bool requestFulfilled;
	public float litValue;
	public float offValue;
	public float curValue;
	public float lastValue;

	public bool LightRequest;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (LightRequest) {
			ctr = ctr + Time.deltaTime * 5f;
			if (ctr>=1f) {
				lastValue = curValue;
				LightRequest = false;
				ctr = 0;
				if (!lit) {
					for (var i=0; i<this.transform.childCount; i++) {
						this.transform.GetChild(i).GetComponent<RubitBlock>().normalState();
						//bank.rubits[indices[i]].GetComponent<RubitBlock>().normalState();
					}
					bank.returnToBase(this.transform);
					//bank.rubits[centerIndx].GetComponent<RubitBlock>().normalState();
				}
			} else {
				curValue = lit?Mathf.Lerp(lastValue,litValue,ctr):Mathf.Lerp(lastValue,offValue,ctr);
				guideMat.SetFloat("_InvFade",curValue); //SetColor("_InvFade", result); 
			}
		}
	}

	public void turn_CW() {
		/*
		GameObject hold_a = bank.rubits[indices[7]];
		GameObject hold_b = bank.rubits[indices[0]];
		for (var i=0; i<7; i++) {
			bank.rubits[indices[(8-i)%8]] = bank.rubits[indices[(6-i)%8]];
		}
		bank.rubits[indices[1]] = hold_a;
		bank.rubits[indices[2]] = hold_b;
		*/
	}
	public void turn_CCW() {
		/*
		GameObject hold_a = bank.rubits[indices[0]];
		GameObject hold_b = bank.rubits[indices[1]];
		for (var i=0; i<7; i++) {
			bank.rubits[indices[i]] = bank.rubits[indices[(i+2)%8]];
		}
		bank.rubits[indices[6]] = hold_a;
		bank.rubits[indices[7]] = hold_b;
		*/
	}
	public void alit() {
		for (var i=0; i<this.transform.childCount; i++) {
			this.transform.GetChild(i).GetComponent<RubitBlock>().highlight();
			//bank.rubits[indices[i]].GetComponent<RubitBlock>().highlight();
		}
		//this.getChild(centerIndex)
		//bank.rubits[centerIndx].GetComponent<RubitBlock>().highlight();
		//Debug.Log("lights");
		LightRequest = true;
		ctr = 0f;
		lit = true;
	}
	public void dim() {
		//Debug.Log("Dark");
		LightRequest = true;
		ctr = 0f;
		lit = false;
		lastValue = curValue;
	}

	public void iParent() {
		bank.setToPivot(cardinal,valence,this.transform);
		/*
		for (var i=0; i<8; i++) {
			bank.rubits[indices[i]].transform.parent = this.transform;
		}
		bank.rubits[centerIndx].transform.parent = this.transform;
		*/
	}
	public void unParent() {
		bank.returnToBase(this.transform);
	}
	public void deParent() {
		//bank.returnToBase(this.transform);
		/*
		for (var i=0; i<8; i++) {
			bank.rubits[indices[i]].transform.parent = null;
			bank.returnChild(bank.rubits[indices[i]]);
		}
		bank.rubits[centerIndx].transform.parent = null;
		bank.returnChild(bank.rubits[centerIndx]);
		*/
	}

	public void turn(Vector3 axis, float percent) {
		this.transform.Rotate(axis, percent);
	}
	public void turncode() {

	}
}
