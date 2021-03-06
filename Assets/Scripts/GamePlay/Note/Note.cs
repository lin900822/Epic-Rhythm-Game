using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
	// Keep a reference of the conductor.
	public Conductor conductor;

	public Transform startPos;

	public Transform endPos;

	public float beat = 0f;

	public bool moving = true;

	public void Initialize(Conductor conductor, Transform startPoint, Transform endPoint, float beat)
	{
		this.conductor = conductor;
		this.startPos = startPoint;
		this.endPos = endPoint;
		this.beat = beat;

		// Set to initial position.
		transform.position = startPoint.position;
		moving = true;
	}

	void Update()
	{
		//this.transform.localScale = new Vector3(1,1,1);
		if (moving)
		{
			transform.position = startPos.position + (endPos.position - startPos.position) * (1f - ((beat - conductor.songPosInBeats) / conductor.BeatsShownInAdvance));
		}
	}
}
