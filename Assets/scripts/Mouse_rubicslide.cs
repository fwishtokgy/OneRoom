using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse_rubicslide : MonoBehaviour {
	/*toBe Public Static Const later*/
	int cX = 0;	
	int cY = 1;	
	int cZ = 2;	

	// Use this for initialization
	void Start () {
		
	}
	
	public Pivot[] yPivots;
	public Pivot[] xPivots;
	public Pivot[] zPivots;

	public bool dragStart;
	public bool onFace;			//is the mouse across a face?
	public bool turnabout;		//is there enough dragging to specify an axis of rotation?
	public bool turnVerified;	//is full autoturn confirmed?
	public bool turning;
	public bool turned;
	public bool locked;
	public bool turnBack;
	public Face targetFace;
	public Transform highlighter;
	public Transform cursor;
	public Pivot targetPivot;

	public GameObject lastObj;
	public GameObject obj;

	public Vector3 curPosition;
	public Vector3 lastPosition;
	public Vector3 initPosition;
	public Vector3 deltaDist;
	public float velocity;
	public float maxVelocity;
	public float drag;
	public float acc;

	public int potentialPivot = 0;
	public int crdTag_Dist = 0;			//the direction that holds the greatest mouse coverage
	public float pivotDist = .1f;		//distance that must be covered for pivot designation
	public float turnDist = 5;			//distance to cover for full rotation

	public Quaternion startRot;
	public Quaternion endRot;
	public float turnCtr;

	public int valence;
	public float degrees;
	public float minDegree;
	public Vector3 turnDirection;	//originally (0,0,0): later shifts to -1 or 1 for an axis
	public Vector3 faceNormal;
	public Vector3 rotVector;
	public Vector3 modDirection;
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonUp(0)) {
			dragStart = false;
			onFace = false;
			if (turnabout) {
				if (!turned) {
					turnBack = true;
					turning = false;
					turnCtr = 0f;
					targetPivot.dim();
					//targetPivot.deParent();
					//targetPivot.transform.rotation = Quaternion.Slerp(targetPivot.transform.rotation, startRot,(minDegree/90f)+turnCtr);
				}
				//turnVerified = false;
				//turnabout = false;
				//updateTarget(false);
				
				//turning = false;
			} 
			if (turned) {
				turned = false;
				turnVerified = false;
				turnabout = false;
				turning = false;
				locked = false;
			}
		}
		if (turnBack) {
			if (turnCtr >=1f) {
				degrees = 0;
				turnBack = false;
				turnabout = false;
				onFace = false;
				turnCtr = 0f;
				//targetPivot.dim();
				targetPivot.deParent();
				targetPivot = null;
			} else {
				turnCtr = turnCtr + Time.deltaTime * 5f;
				targetPivot.transform.rotation = Quaternion.Slerp(targetPivot.transform.rotation, startRot,turnCtr);
			}
		}
		if (turning) {
			//transform.rotation = Quaternion.Slerp(from.rotation, to.rotation, Time.time * speed)
			if (turnCtr>=1f) {//targetPivot.transform.rotation==endRot ) {
				//targetPivot.turn_CW();
    			degrees = 0;
    			targetPivot.dim();
				targetPivot.deParent();
				targetPivot = null;
				turnCtr = 0f;
				turning = false;
				turned = true;
				locked = true;
			} else {
				turnCtr = turnCtr + Time.deltaTime * 4f;
				if(turnCtr>1f) turnCtr = 1f;
				//Debug.Log(turnCtr);
				//targetPivot.turn(turnDirection,turnCtr);
				targetPivot.transform.rotation = Quaternion.Slerp(startRot, endRot,(minDegree/90f)+turnCtr);
				//on complete
				//
			}
		}

        RaycastHit hit;
        int x = Screen.width / 2;
        int y = (Screen.height/10) * 6;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);// new Vector2(x,y));//Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) {
        	if (hit.collider==null) {	//in space: drag cube around
        		if (dragStart) {
        			
        		}
        		if (onFace) {
        			onFace = false;
        			lastObj = obj;
        			obj = null;
        			updateTarget(false);
        		} if (turnabout) {
        			targetPivot.dim();
        			targetPivot.deParent();
        			targetPivot = null;
        			turnabout = false;
        		}
        	} else {	// Some object is being hit by cursor
        		lastObj = obj;
        		obj = hit.collider.gameObject;
        		if (obj.GetComponent("Face")) { //Focus on Face
        			cursor.position = hit.point;

        			if (turning || locked) return;
        			if (Input.GetMouseButtonDown(0)) { //initial mouse press for dragging
        				curPosition = cursor.position;//obj.transform.position;
        				initPosition = cursor.position;//obj.transform.position;//curPosition;
        				dragStart = true;
        				faceNormal = hit.normal;
        				cleanVector(faceNormal);
        			}
        			if (Input.GetMouseButton(0) && !Input.GetMouseButtonUp(0)) { //drag effect
        				lastPosition = curPosition;
        				curPosition = cursor.position;//obj.transform.position;
        			//	deltaDist = new Vector3(curPosition.x -initPosition.x, curPosition.y-initPosition.y, curPosition.z-initPosition.z );
                        deltaDist = new Vector3(initPosition.x - curPosition.x, initPosition.y - curPosition.y, initPosition.z - curPosition.z);
                        if (!turnabout) { //mouse dragging face: what pivot will be chosen?
        					
        					float largerDist = 0;
        					float a;
        					float b;
        					if (faceNormal.x==1 || faceNormal.x==-1) {//targetFace.cardinal == cX) {
        						a = Mathf.Abs(deltaDist.y);
        						b = Mathf.Abs(deltaDist.z);
        						largerDist = (a>b)?a:b;
        						crdTag_Dist = (a>b)?cY:cZ;
        						potentialPivot = (a>b)?cZ:cY;
	        				} else if (faceNormal.y==1 || faceNormal.y==-1) {//else if (targetFace.cardinal == cY) {
	        					a = Mathf.Abs(deltaDist.x);
        						b = Mathf.Abs(deltaDist.z);
        						largerDist = (a>b)?a:b;	
        						crdTag_Dist = (a>b)?cX:cZ;
        						potentialPivot = (a>b)?cZ:cX;
	        				} else if (faceNormal.z==1 || faceNormal.z==-1) {//(targetFace.cardinal == cZ) {
	        					a = Mathf.Abs(deltaDist.x);
        						b = Mathf.Abs(deltaDist.y);
        						largerDist = (a>b)?a:b;
        						crdTag_Dist = (a>b)?cX:cY;
        						potentialPivot = (a>b)?cY:cX;
	        				}
	        				if (largerDist>pivotDist) {
	        					turnabout = true;
	        					highlighter.gameObject.SetActive(false);
	        					
	        					//turnDirection = (potentialPivot==cX)?Vector3.right:((potentialPivot==cY)?Vector3.up:Vector3.forward);
	        					int index = 0;
	        					if (potentialPivot==cX) {
	        						turnDirection = Vector3.right;
	        						index = (obj.transform.position.x>5f)?2:((obj.transform.position.x<-5f)?0:1);
	        						targetPivot = xPivots[index];
	        					} else if (potentialPivot==cY) {
	        						turnDirection = Vector3.up;
	        						index = (obj.transform.position.y>5f)?2:((obj.transform.position.y<-5f)?0:1);
	        						targetPivot = yPivots[index];
	        					} else {
	        						turnDirection = Vector3.forward;
	        						index = (obj.transform.position.z>5f)?2:((obj.transform.position.z<-5f)?0:1);
	        						targetPivot = zPivots[index];
	        					}

	        					targetPivot.iParent();
	        					targetPivot.alit();

	        					rotVector = Vector3.Cross(faceNormal,turnDirection).normalized;

	        					if (crdTag_Dist==cX) {
	        						valence = ((deltaDist.x>0)?1:-1) * Mathf.FloorToInt(rotVector.x);
	        						modDirection = turnDirection * rotVector.x;
	        					} else if (crdTag_Dist==cY) {
	        						valence = ((deltaDist.y>0)?1:-1) * Mathf.FloorToInt(rotVector.y);
	        						modDirection = turnDirection * rotVector.y;
	        					} else {
	        						valence = ((deltaDist.z>0)?1:-1) * Mathf.FloorToInt(rotVector.z);
	        						modDirection = turnDirection * rotVector.z;
	        					}
	        					startRot = targetPivot.transform.rotation;
	        				}
    					} else {	//mouse dragging along axis: is the turn sufficient?
    						/*
    						float largerDist = (crdTag_Dist==cX)?deltaDist.x:((crdTag_Dist==cY)?deltaDist.y:deltaDist.z);
    						if (Mathf.Abs(largerDist)<pivotDist) {
	        					turnabout = false;
	        					targetPivot.dim();
	        					targetPivot.deParent();
	        					targetPivot = null;
	        				}
	        				*/
    					}
    					if (turnabout) {
    						if (!turnVerified) {
    							//float dist = (crdTag_Dist==cX)?(highlighter.position.x-curPosition.x):((crdTag_Dist==cY)?(curPosition.y-highlighter.position.y):(highlighter.position.z - curPosition.z));
    							float dist = (crdTag_Dist==cX)?deltaDist.x:((crdTag_Dist==cY)?deltaDist.y:deltaDist.z);
    							//acc = acc * dist;
    							//velocity = velocity + acc;
    							//if (velocity>maxVelocity) velocity = maxVelocity;
    							float amt = dist * Time.deltaTime * 8;
    							//targetPivot.rotate
    							targetPivot.turn(modDirection, amt);
    							degrees += amt;
    							//Debug.Log(degrees);
    							if (degrees>=minDegree) {
    								//Debug.Log("Clockwise");
    								turnVerified = true;
    								//targetPivot.dim();
    								endRot = startRot * Quaternion.Euler(modDirection.x*90,modDirection.y*90,modDirection.z*90);
    								//targetPivot.turn(turnDirection,90f-degrees);
    							} else if (degrees<=(minDegree*-1)) {
    								//Debug.Log("CounterClockwise");
    								turnVerified = true;
    								//targetPivot.dim();
    								endRot = startRot * Quaternion.Euler(modDirection.x*-90,modDirection.y*-90,modDirection.z*-90);
    								//targetPivot.turn(turnDirection,90f-degrees);
    								//targetPivot.turn_CCW();
    								//degrees = 0;
    							}
    						} else {
    							turning = true;
    							turnCtr = 0f;
    						}
    					}

    				} else {	//hover effect, no drag

						if (!onFace) {
	        				updateTarget(true);
	        				onFace = true;
	        			} else {
	        				if (obj!=lastObj || Input.GetMouseButtonUp(0)) { 
	        					updateTarget(true);	//reassign targetFace
	        				} 
	        				if (Input.GetMouseButtonUp(0)) {
	        					dragStart = false;
	        					if (turnabout) {
	        						if (!turnVerified) {
	        							Debug.Log("Return to original Position");
	        							targetPivot.turn(turnDirection,-degrees);
	        							degrees = 0;
	        						} else {
	        							turnVerified = false;
	        						}
	        						//lerp the pivot back to its original position;
	        						//either render cube untouchable until lerp complete
	        						// or add another if-else branch for during-lerp
	        						Debug.Log("279");
	        						targetPivot.dim();
	        						turnabout = false;
	        						//targetPivot.deParent();
	        						//needToKnow lerp stuffs
	        						//does it have velocity?
	        						// if so, then in Update velocity should be checked until 0
	        						// then determine turnVerified
	        					}
	        				}
	        			}
    				}
        		}
        	}

        }
	}

	void updateTarget(bool toActivate) {
		highlighter.gameObject.SetActive(toActivate);
		if (toActivate) {
			targetFace = obj.GetComponent<Face>();
			highlighter.position = targetFace.transform.position;
			highlighter.rotation = targetFace.transform.rotation;
		} else {
			targetFace = null;
			highlighter.position = Vector3.zero;
		}
	}
	void cleanVector(Vector3 subject) {
		subject.x = setToTarget(subject.x,0f);
		subject.y = setToTarget(subject.y,0f);
		subject.z = setToTarget(subject.z,0f);
		subject.x = setToTarget(subject.x,1f);
		subject.y = setToTarget(subject.y,1f);
		subject.z = setToTarget(subject.z,1f);
		subject.x = setToTarget(subject.x,90f);
		subject.y = setToTarget(subject.y,90f);
		subject.z = setToTarget(subject.z,90f);
	}
	float setToTarget(float subject, float target) {
		if (subject!=target && subject<(target+.001) && subject>(target-.001)) {
			subject = target;
			Mathf.Floor(subject);
		}
		return subject;
	}
}
