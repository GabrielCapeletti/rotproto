using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

	public GameObject robot;
	public bool hasEntrance = false;

	private float currentTime = 0;
	private bool shaking = false;
	private float shakeRange = 0.02f;
	private Camera camera;

	void Start ()
	{
		this.camera = this.GetComponent<Camera> ();
		if (hasEntrance)
			this.camera.orthographicSize = 0.05f;
	}

	void Update ()
	{	
		Vector3 target = new Vector3 (robot.transform.position.x, robot.transform.position.y, -10);
		if (shaking) {
			target += new Vector3 (Random.Range (-shakeRange, shakeRange), Random.Range (-shakeRange, shakeRange), 0);
			currentTime += Time.deltaTime;	
			if (currentTime > 0.2f) {
				shaking = false;
			}
		}
		this.transform.position = target;

		if (hasEntrance) {		
			StartCoroutine (EnteringScene ());
		}		
	}

	public IEnumerator EnteringScene ()
	{
		for (int i = 99; i > 0; i--) {
			camera.rect = new Rect (0.5f, (float)(i / 100f), 1, 1);
			this.camera.orthographicSize = 5 * ((100 - i) / 100f);
			yield return this;
		}
	}

	public void ShakeCamera ()
	{
		currentTime = 0;
		shaking = true;

	}

}
