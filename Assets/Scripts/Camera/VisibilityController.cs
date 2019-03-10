using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityController : MonoBehaviour {
	public Renderer _renderer;
	private Material _material;
	public Material transparentMaterial;
	private bool visible;
	private int _id;

	void Awake() {
		_material = _renderer.material;
		this.visible = true;
		_id = transform.GetInstanceID();
	}

	public int GetTransformId() {
		return this._id;
	}

	public void Hide() {
		if (visible) {
//			Debug.Log("hide " + this.name);
			_renderer.material = transparentMaterial;
			visible = false;
		}
	}

	public void Show() {
		if (!visible) {
//			Debug.Log("show " + this.name);
			_renderer.material = _material;
			visible = true;
		}
	}
}