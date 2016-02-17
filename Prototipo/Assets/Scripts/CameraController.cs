using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

	public GameObject robot;

	private float currentTime = 0;
	private bool shaking = false;
	private float shakeRange = 0.02f;

	void Start ()
	{
	}

	void Update ()
	{
		Vector3 target = new Vector3 (robot.transform.position.x, robot.transform.position.y, -10);
		if (shaking) {
			target += new Vector3 (Random.Range (-shakeRange, shakeRange), Random.Range (-shakeRange, shakeRange), 0);
			currentTime += Time.deltaTime;
			//if (currentTime > 0.05f) {
			//	shakeRange = 0.01f;
			//}
			if (currentTime > 0.2f) {
				shaking = false;
			}
		}
		this.transform.position = target;
	}

	public void ShakeCamera ()
	{
		currentTime = 0;
		shaking = true;

	}

}
