using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
	public GameConstants gameConstants; //reference to the ScriptableObject container gameConstants
	private Vector3 rotator;
	// Update is called once per frame
	void Update()
	{
		//Rotate
		rotator = new Vector3(0, gameConstants.rotatorRotateSpeed, 0);
		this.transform.rotation = Quaternion.Euler(this.transform.eulerAngles - rotator);
	}
}