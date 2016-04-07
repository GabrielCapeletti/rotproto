using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class KaneShooterController : MonoBehaviour
{
	private const float TOLERANCE = 0.5f;

	public CoverController cover;
	public GameObject gun;
	public GameObject tommyGun;
	public GameObject gunTip;
	public GameObject bullet;
	public GameObject hitCollider;
	public HUDAmmo hudAmmo;
	public HUDLife hudLife;
	public CameraController camera;
	public int initialAmmo;
	[Tooltip ("balas por segundo")]
	public float fireRate;
	public float recoil;
	public float transitionSpeed;
	public float reloadTime;
	public float doubleTapTime;
	public int playerNumber;
	public Sprite[] weaponSprites;

	private int initialLife;
	private int currentLife;

	private LineRenderer gunTipenderer;
	private Vector2 target;
	private Animator animator;
	private Rigidbody2D rigidBody;
	private Vector2 teste = Vector2.zero;
	private float aimAng = 0;
	private float lastBulletTime = 10;
	private GameObject particles;
	private bool invertedAim = false;
	private int currentAmmo;
	private int lookDirection;
	private float currentReloadTime;
	private bool rightCoverSide = false;
	private Collider2D collider;

	private WeaponItemCollider inRangeWeapon;

	private Vector2 movingSpeed;
	private float transitionTime;
	private Vector2 lastPosition;

	private bool onDoubleTap = false;
	private bool noDirection = true;
	private float doubleTapCounter;
	private bool rightPressed;
	private bool weaponOnRange;
	private Weapon currentWeaponOnRange;

	private enum State
	{
		UNDER_COVER,
		SHOOTING,
		TRANSITIONING
	}

	private State currentState;

	public void start ()
	{
		this.Start ();
	}


	void Start ()
	{
		animator = GetComponent<Animator> ();
		collider = GetComponent<Collider2D> ();
		currentAmmo = GameManager.instance.initialAmmo;
		gunTipenderer = gunTip.GetComponent<LineRenderer> ();
		particles = tommyGun.transform.GetChild (0).gameObject;
		currentReloadTime = 0;
		lookDirection = ((this.GetComponent<SpriteRenderer> ().flipX) ? 1 : -1);
		initialLife = GameManager.instance.initialLife;
		currentLife = initialLife;

		this.SetWeapon (Weapon.Pistol);
	}

	public int DecreaseAmmo ()
	{
		hudAmmo.RemoveBullet ();
		currentAmmo--;
		return currentAmmo;
	}

	public int Reload ()
	{
		currentAmmo = initialAmmo;
		hudAmmo.Reload ();
		return initialAmmo;
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
		this.WeaponControl ();

		if (currentAmmo <= 0) {
			currentReloadTime += Time.deltaTime;
			if (currentReloadTime > reloadTime) {
				currentAmmo = Reload ();
				currentReloadTime = 0;
			}
			return;
		}

		//if (Input.GetButtonDown ("ChangeSide" + playerNumber)) {
		//	this.rightCoverSide = (this.rightCoverSide == false);
		//	NewCover (this.cover);
		//}

		if (Input.GetAxisRaw ("Horizontal" + playerNumber) > 0) {
			Quaternion quart = new Quaternion (0, 0, 0, 0);
			this.transform.localRotation = quart;
			invertedAim = false;
			if (Input.GetAxisRaw ("Vertical" + playerNumber) > 0) {
				cover.nextUp.HighLight ();
				if (Input.GetButtonDown ("Jump" + playerNumber)) {
					NewCover (cover.nextUp);
				}
			} else if (Input.GetAxisRaw ("Vertical" + playerNumber) < 0) {
				cover.nextDown.HighLight ();
				if (Input.GetButtonDown ("Jump" + playerNumber)) {
					NewCover (cover.nextDown);
				}
			} else {
				cover.next.HighLight ();
				if (Input.GetButtonDown ("Jump" + playerNumber)) {
					NewCover (cover.next);
				}
			}

			DoubleTapCheck (true);
		} else if (Input.GetAxisRaw ("Horizontal" + playerNumber) != 0) {			
			Quaternion quart = new Quaternion (0, lookDirection * 180, 0, 0);
			this.transform.localRotation = quart;
			invertedAim = true;
			if (Input.GetAxisRaw ("Vertical" + playerNumber) > 0) {
				cover.previousUp.HighLight ();
				if (Input.GetButtonDown ("Jump" + playerNumber)) {
					NewCover (cover.previousUp);
				}
			} else if (Input.GetAxisRaw ("Vertical" + playerNumber) < 0) {
				cover.previousDown.HighLight ();
				if (Input.GetButtonDown ("Jump" + playerNumber)) {
					NewCover (cover.previousDown);
				}
			} else {
				cover.previous.HighLight ();
				if (Input.GetButtonDown ("Jump" + playerNumber)) {
					NewCover (cover.previous);
				}
			}
			DoubleTapCheck (false);
		} else {	
			if (!noDirection) {
				doubleTapCounter = 0;
			}
			if (onDoubleTap && doubleTapCounter < doubleTapTime) {
				onDoubleTap = true;
			} else {
				onDoubleTap = false;
			}
			doubleTapCounter += Time.deltaTime;		
			noDirection = true;
		}

		if (Input.GetAxisRaw ("Aim" + playerNumber) != 0 && Input.GetAxisRaw ("Horizontal" + playerNumber) == 0) {		
			this.GoToShooting ();
		}
	}

	public void OnItemRange (WeaponItemCollider _inRangeWeapon)
	{
		inRangeWeapon = _inRangeWeapon;
		currentWeaponOnRange = _inRangeWeapon.weaponType;
		weaponOnRange = true;
	}

	public void OutOfItemRange (WeaponItemCollider _inRangeWeapon)
	{
		//	if (inRangeWeapon.weaponType == currentWeaponOnRange) {
		weaponOnRange = false;
		//	}
	}

	private void DoubleTapCheck (bool isRighSide)
	{
		if (rightPressed != isRighSide || (isRighSide == rightCoverSide)) {
			onDoubleTap = false;
		}



		rightPressed = isRighSide;
		if (noDirection) {
			doubleTapCounter = 0;
			if (onDoubleTap) {
				this.rightCoverSide = (this.rightCoverSide == false);
				this.NewCover (cover);
			}
		}
		noDirection = false;
		doubleTapCounter += Time.deltaTime;
		if (doubleTapCounter < doubleTapTime) {
			onDoubleTap = true;
		} else {
			onDoubleTap = false;
		}		
	}

	public void GoToShooting ()
	{
		gunTipenderer.SetPosition (0, new Vector3 (transform.position.x, transform.position.y, 0));
		gun.SetActive (true);
		//hitCollider.SetActive (true);
		this.currentState = State.SHOOTING;	
		this.cover.OffCover ();
	}

	public void AimUp ()
	{
		aimAng += 1f;
		if (aimAng >= 85) {
			aimAng = 84;
		} else {
			tommyGun.transform.Rotate (new Vector3 (0, 0, 1));
		}
	}

	public void AimDown ()
	{
		aimAng -= 1f;
		if (aimAng <= -85) {
			aimAng = -84;
		} else {
			tommyGun.transform.Rotate (new Vector3 (0, 0, -1));
		}
	}

	public void SetWeapon (Weapon weapon)
	{
		if (weapon.FireRate == Weapon.Pistol.FireRate) {
			//Muito cansado pra ligar para PERFOMANCE
			this.tommyGun.GetComponent<SpriteRenderer> ().sprite = weaponSprites [0];
		} else {
			this.tommyGun.GetComponent<SpriteRenderer> ().sprite = weaponSprites [1];
		}

		GameManager.instance.initialAmmo = weapon.MagSize;
		this.initialAmmo = weapon.MagSize;
		this.reloadTime = weapon.ReloadTime;
		this.recoil = weapon.Recoil;
		this.fireRate = weapon.FireRate;
		this.hudAmmo.NewWeapon ();
		this.currentAmmo = this.initialAmmo;
	}


	public void ShootingProjectile ()
	{
		particles.SetActive (true);
		if (lastBulletTime > 1 / fireRate && currentAmmo > 0) {
			currentAmmo = DecreaseAmmo ();
			GameObject go = (GameObject)Instantiate (bullet, transform.position, Quaternion.identity);
			go.GetComponent<BulletController> ().SetAng ((invertedAim ? 180 - aimAng : aimAng));
			if (!invertedAim) {
				go.transform.localScale = new Vector3 (-go.transform.localScale.x, go.transform.localScale.y, go.transform.localScale.z);
			}
			lastBulletTime = 0;
			camera.ShakeCamera ();
			aimAng += recoil;
			if (aimAng >= 85) {
				aimAng = 84;
			} else {
				tommyGun.transform.Rotate (new Vector3 (0, 0, recoil));
			}
		}
	}

	public void Shooting ()
	{		
		if (Input.GetAxisRaw ("Aim" + playerNumber) != 0) {			
			if (Input.GetAxisRaw ("Vertical" + playerNumber) > 0) {
				this.AimUp ();
			} else if (Input.GetAxisRaw ("Vertical" + playerNumber) < 0) {
				this.AimDown ();
			}

			if (Input.GetAxisRaw ("Fire1" + playerNumber) > 0) {	
				this.ShootingProjectile ();
			} else {
				particles.SetActive (false);
			}

			if (invertedAim) {
				gunTipenderer.SetPosition (0, new Vector3 (transform.position.x, transform.position.y, 0));
				gunTipenderer.SetPosition (1, new Vector3 (transform.position.x + Mathf.Cos (Mathf.Deg2Rad * (180 - aimAng)) * 10,
					transform.position.y + Mathf.Sin (Mathf.Deg2Rad * (180 - aimAng)) * 10, 0));
			} else {
				gunTipenderer.SetPosition (0, new Vector3 (transform.position.x, transform.position.y, 0));
				gunTipenderer.SetPosition (1, new Vector3 (transform.position.x + Mathf.Cos (Mathf.Deg2Rad * (aimAng)) * 10,
					transform.position.y + Mathf.Sin (Mathf.Deg2Rad * (aimAng)) * 10, 0));
			}
		} else {						
			this.GoUndercover ();
		}

		if (lastBulletTime < 10) {
			lastBulletTime += Time.deltaTime;
		}
	}

	public void GoUndercover ()
	{
		particles.SetActive (false);
		//hitCollider.SetActive (false);
		this.cover.OnCover ();
		this.currentState = State.UNDER_COVER;
		gun.SetActive (false);
	}

	public int DecreaseLife ()
	{
		currentLife--;
		hudLife.RemoveLife ();

		if (currentLife < 0)
			SceneManager.LoadScene (0);

		return currentLife;
	}

	private void WeaponControl ()
	{
		if (weaponOnRange) {
			if (Input.GetButtonDown ("item" + playerNumber)) {
				this.SetWeapon (this.currentWeaponOnRange);
				if (inRangeWeapon != null) {
					Destroy (inRangeWeapon.transform.parent.gameObject);
				}
			}
		}
	}


	public void Transitioning ()
	{		
		//transform.position = Vector2.Lerp (transform.position, this.target, transitionTime);

		transform.position += new Vector3 (this.movingSpeed.x * Time.deltaTime, this.movingSpeed.y * Time.deltaTime);

		animator.Play ("rolling");
		this.WeaponControl ();

		if (Vector2.Distance (transform.position, target) < TOLERANCE) {
			this.currentState = State.UNDER_COVER;
			this.lastPosition = target;
			transform.position = target;
			collider.enabled = true;
			//	hitCollider.SetActive (false);
			this.rigidBody.gravityScale = 1;
		}
	}

	public void NewCover (CoverController _cover)
	{
		if (this.rigidBody == null) {
			this.rigidBody = GetComponent<Rigidbody2D> ();
		}
		this.rigidBody.gravityScale = 0;

		if (this.cover != null) {
			this.cover.OffCover ();
		}
		_cover.OnCover ();

		this.cover = _cover;
		this.currentState = State.TRANSITIONING;
		this.lastPosition = transform.position;
		//	hitCollider.SetActive (true);
		this.collider.enabled = false;

		if (!rightCoverSide) {
			this.target = _cover.PositionOne;
		} else {
			this.target = _cover.PositionTwo;
		}

		this.movingSpeed = this.target - this.lastPosition;
		this.movingSpeed.Normalize ();
		this.movingSpeed *= this.transitionSpeed;

		rigidBody = GetComponent<Rigidbody2D> ();
		rigidBody.velocity = Vector2.zero;
		animator.Play ("idle");
	}

}
