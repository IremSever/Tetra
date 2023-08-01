using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]

public class IconToggle : MonoBehaviour 
{
    public Sprite iconTrue, iconFalse;
    bool defaultIconState = true;
    Image image;
    const string PlayerPrefKey = "IconToggleState";
    void Awake()
    {
        defaultIconState = PlayerPrefs.GetInt(PlayerPrefKey, 1) == 1; 
    }
    void Start()
    {
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
        PlayerPrefs.SetInt(PlayerPrefKey, state ? 1 : 0); 
        PlayerPrefs.Save();
    }
}

