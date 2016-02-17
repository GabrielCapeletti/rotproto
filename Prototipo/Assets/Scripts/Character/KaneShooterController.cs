using UnityEngine;
using System.Collections;

public class KaneShooterController : MonoBehaviour
{
	public CoverController cover;
	public GameObject gun;
	public GameObject tommyGun;
	public GameObject gunTip;
	public GameObject bullet;
	public CameraController camera;
	[Tooltip ("balas por segundo")]
	public float fireRate;
	public float recoil;


	private LineRenderer gunTipenderer;
	private Vector2 target;
	private Animator animator;
	private Rigidbody2D rigidBody;
	private Vector2 teste = Vector2.zero;
	private float aimAng = 0;
	private float lastBulletTime = 10;
	private GameObject particles;
	private bool invertedAim = false;

	private enum State
	{
		UNDER_COVER,
		SHOOTING,
		TRANSITIONING
	}

	private State currentState;

	void Start ()
	{
		gunTipenderer = gunTip.GetComponent<LineRenderer> ();
		particles = tommyGun.transform.GetChild (0).gameObject;
	}

	void Update ()
	{
		switch (currentState) {
		case State.UNDER_COVER:
			UnderCover ();
			break;
		case State.SHOOTING:
			Shooting ();
			break;
		case State.TRANSITIONING:
			Transitioning ();
			break;
		default:
			break;
		}
	}

	void UnderCover ()
	{
		if (Input.GetKey (KeyCode.RightArrow)) {
			Quaternion quart = new Quaternion (0, 0, 0, 0);
			this.transform.localRotation = quart;
			invertedAim = false;
			if (Input.GetKey (KeyCode.UpArrow)) {
				cover.nextUp.HighLight ();
				if (Input.GetKey (KeyCode.Space)) {
					NewCover (cover.nextUp);
				}
			} else {
				cover.next.HighLight ();
				if (Input.GetKey (KeyCode.Space)) {
					NewCover (cover.next);
				}
			}
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			cover.previous.HighLight ();
			Quaternion quart = new Quaternion (0, 180, 0, 0);
			this.transform.localRotation = quart;
			invertedAim = true;
			if (Input.GetKey (KeyCode.Space)) {
				NewCover (cover.previous);
			}
		} else if (Input.GetKey (KeyCode.LeftShift)) {			
			gunTipenderer.SetPosition (0, new Vector3 (transform.position.x, transform.position.y, 0));
			this.currentState = State.SHOOTING;
			gun.SetActive (true);
		}
	}

	public void Shooting ()
	{		
		if (Input.GetKey (KeyCode.LeftShift)) {			
			if (Input.GetKey (KeyCode.UpArrow)) {
				aimAng += 1f;
				if (aimAng >= 85) {
					aimAng = 84;
				} else {
					tommyGun.transform.Rotate (new Vector3 (0, 0, 1));
				}

			} else if (Input.GetKey (KeyCode.DownArrow)) {
				aimAng -= 1f;
				if (aimAng <= -85) {
					aimAng = -84;
				} else {
					tommyGun.transform.Rotate (new Vector3 (0, 0, -1));
				}
			}

			if (Input.GetKey (KeyCode.Space)) {	
				particles.SetActive (true);
				if (lastBulletTime > 1 / fireRate) {					
					GameObject go = (GameObject)Instantiate (bullet, transform.position, Quaternion.identity);
					go.GetComponent<BulletController> ().SetAng ((invertedAim ? 180 - aimAng : aimAng));
					lastBulletTime = 0;
					camera.ShakeCamera ();
					aimAng += recoil;
					if (aimAng >= 85) {
						aimAng = 84;
					} else {
						tommyGun.transform.Rotate (new Vector3 (0, 0, recoil));
					}
				}
			} else {
				particles.SetActive (false);
			}

			if (invertedAim) {
				gunTipenderer.SetPosition (1, new Vector3 (transform.position.x + Mathf.Cos (Mathf.Deg2Rad * (180 - aimAng)) * 10,
					transform.position.y + Mathf.Sin (Mathf.Deg2Rad * (180 - aimAng)) * 10, 0));
			} else {
				gunTipenderer.SetPosition (1, new Vector3 (transform.position.x + Mathf.Cos (Mathf.Deg2Rad * (aimAng)) * 10,
					transform.position.y + Mathf.Sin (Mathf.Deg2Rad * (aimAng)) * 10, 0));
			}
		} else {						
			particles.SetActive (false);
			this.currentState = State.UNDER_COVER;
			gun.SetActive (false);
		}
		if (lastBulletTime < 10) {
			lastBulletTime += Time.deltaTime;
		}


	}

	private void ShootProjectil ()
	{
		
	}

	public void Transitioning ()
	{		
		transform.position = Vector2.Lerp (transform.position, this.target, 0.1f);
		if (Vector2.Distance (transform.position, target) < 0.1f) {
			this.currentState = State.UNDER_COVER;
			this.rigidBody.gravityScale = 1;
		}
	}

	public void NewCover (CoverController _cover)
	{
		if (this.rigidBody == null) {
			this.rigidBody = GetComponent<Rigidbody2D> ();
		}
		this.rigidBody.gravityScale = 0;
		
		this.cover = _cover;
		this.currentState = State.TRANSITIONING;
		this.target = new Vector2 (cover.transform.position.x, cover.transform.position.y);

		rigidBody = GetComponent<Rigidbody2D> ();
		rigidBody.velocity = Vector2.zero;
		animator = GetComponent<Animator> ();
		animator.PlayInFixedTime ("idle");
	}

}
