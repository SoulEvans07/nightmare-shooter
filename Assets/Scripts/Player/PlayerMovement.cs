using System;
using UnityEngine;
using XInputDotNetPure;

public class PlayerMovement : MonoBehaviour {
	private static readonly float TOLERANCE = 0.000001f;
	private static readonly int IsWalking = Animator.StringToHash("IsWalking");

	public float speed = 6f;
	public float crosshairSpeed = 700f;
	public Camera cam;
	public RectTransform crosshair;

	private Animator _animator;
	private Rigidbody _rigidbody;
	private int floorMask;
	private readonly float camRayLength = 100f;
	private Vector3 movement;

	private const PlayerIndex singlePlayer = 0;
	private bool controllerConnected;
	private bool prevControllerState;
	private Vector3 crosshairMovement;
	public RectTransform _canvas;
	private float _maxWidth;
	private float _maxHeight;
	private bool isControllerMode;

	private int furnitureMask;
	private VisibilityController lastHit;

	private void Awake() {
		_rigidbody = GetComponent<Rigidbody>();
		_animator = GetComponent<Animator>();
		this.floorMask = LayerMask.GetMask("Floor");
		this.controllerConnected = GamePad.GetState(singlePlayer).IsConnected;
		this.prevControllerState = this.controllerConnected;
		SetControllerMode(0, 0);
		crosshair.gameObject.SetActive(this.isControllerMode);

		_maxWidth = _canvas.rect.width;
		_maxHeight = _canvas.rect.height;

		this.furnitureMask = LayerMask.GetMask("Furniture");
	}

	private void FixedUpdate() {
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");
		float rx = Input.GetAxisRaw("RStickX");
		float ry = Input.GetAxisRaw("RStickY");
		Vector2 mousePos = Input.mousePosition;

		if ((Math.Abs(h) > TOLERANCE) || (Math.Abs(v) > TOLERANCE)) {
			HideObjectsInWay();	
		}

		SetControllerMode(rx, ry);

		Move(h, v);
		if (isControllerMode) {
			MoveCrosshair(rx, ry);
			Turning(crosshair.position);
		} else {
			crosshair.position = new Vector3(mousePos.x, mousePos.y, 0f);
			Turning(mousePos);
		}
		Animating(h, v);
	}

	private void HideObjectsInWay() {
//		Debug.DrawRay(this.transform.position, (this.transform.position - this.cam.transform.position), Color.magenta);

		if (Physics.Raycast(
			this.cam.transform.position,
			(this.transform.position - this.cam.transform.position),
			out RaycastHit hit,
			camRayLength,
			furnitureMask
		)) {
			if(lastHit == null || lastHit.GetTransformId() != hit.transform.GetInstanceID()){
				if (lastHit != null) {
					lastHit.Show();
					lastHit = null;	
				}
				lastHit = hit.transform.GetComponent<VisibilityController>();
				lastHit.Hide();
			}

		} else if (lastHit != null) {
			lastHit.Show();
			lastHit = null;
		}
	}

	private void Move(float h, float v) {
		movement.Set(h, 0f, v);
		movement = movement.normalized * speed * Time.deltaTime;
		_rigidbody.MovePosition(transform.position + movement);
	}

	private void MoveCrosshair(float rx, float ry) {
		crosshairMovement.Set(rx, ry, 0);
		crosshairMovement = crosshairMovement.normalized * crosshairSpeed * Time.deltaTime;
		crosshair.position = new Vector3(
			Mathf.Clamp(crosshair.position.x + crosshairMovement.x, 0, _maxWidth),
			Mathf.Clamp(crosshair.position.y + crosshairMovement.y, 0, _maxHeight),
			0f);
	}

	private void Turning(Vector3 target) {
		Ray camRay = cam.ScreenPointToRay(target);
		// todo: rotate so you shoot at the cursor
		if (Physics.Raycast(camRay, out RaycastHit floorHit, camRayLength, floorMask)) {
			Vector3 playerToMouse = floorHit.point - transform.position;
			playerToMouse.y = 0f;

			Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
			_rigidbody.MoveRotation(newRotation);
		}
	}

	private void Animating(float h, float v) {
		bool walking = (Math.Abs(h) > TOLERANCE) || (Math.Abs(v) > TOLERANCE);
		_animator.SetBool(IsWalking, walking);
	}

	private void SetControllerMode(float rx, float ry) {
		controllerConnected = GamePad.GetState(singlePlayer).IsConnected;

		if (isControllerMode) {
			if (Input.GetMouseButtonUp(0) || !controllerConnected) {
				isControllerMode = false;
				crosshair.gameObject.SetActive(false);
			}
		} else {
			if (controllerConnected && (Math.Abs(rx) > TOLERANCE || Math.Abs(ry) > TOLERANCE)
			    || controllerConnected != prevControllerState) {
				isControllerMode = true;
				crosshair.gameObject.SetActive(true);
			}
		}

		prevControllerState = controllerConnected;
	}
}