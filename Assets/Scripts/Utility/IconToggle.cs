using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class IconToggle : MonoBehaviour 
{
	public Sprite iconTrue, iconFalse;
	public bool defaultIconState = true;
	Image image;
	void Start () {
		image = GetComponent<Image>();
		image.sprite = (defaultIconState) ? iconTrue : iconFalse;
	}
	public void ToggleIcon(bool state)
	{
		if (!image || !iconTrue || !iconFalse)
		{
			Debug.LogWarning("Undefined iconTrue or iconFalse!"); 
			return;
		}
		image.sprite = (state) ? iconTrue : iconFalse;
	}
}
