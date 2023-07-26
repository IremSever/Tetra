using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public bool musicEnabled = true, fxEnabled = true;
	[Range(0,1)] public float musicVolume = 1.0f, fxVolume = 1.0f;
	public AudioClip clearRowSound, moveSound, dropSound, gameOverSound, errorSound, levelUpVocalClip;
	public AudioSource musicSource;
	public AudioClip[] musicClips, vocalClips;
	AudioClip randomMusicClip;
	public IconToggle musicIconToggle, fxIconToggle;
	const string MusicEnabledKey = "MusicEnabled", FXEnabledKey = "FXEnabled", MusicVolumeKey = "MusicVolume", FXVolumeKey = "FXVolume";
	void Start () 
	{
		LoadSoundSettings();
		randomMusicClip = GetRandomClip(musicClips);
		PlayBackgroundMusic (randomMusicClip);
	}
	public AudioClip GetRandomClip(AudioClip[] clips)
	{
		AudioClip randomClip = clips[Random.Range(0, clips.Length)];
		return randomClip;
	}
	public void PlayBackgroundMusic(AudioClip musicClip)
	{
		if (!musicEnabled || !musicClip || !musicSource)
			return;
		musicSource.Stop();
		musicSource.clip = musicClip;
		musicSource.volume = musicVolume;
		musicSource.loop = true;
		musicSource.Play();        
	} 
	void UpdateMusic ()
	{
		if (musicSource.isPlaying != musicEnabled) 
		{
			if (musicEnabled) 
			{
				randomMusicClip = GetRandomClip(musicClips);
				PlayBackgroundMusic(randomMusicClip);
			}
			else
				musicSource.Stop();
		}
	}
	public void ToggleMusic()
	{
		musicEnabled = !musicEnabled;
		UpdateMusic();
		if (musicIconToggle)
			musicIconToggle.ToggleIcon(musicEnabled);
		SaveSoundSettings();
	}
	public void ToggleFX()
	{
		fxEnabled = !fxEnabled;
		if (fxIconToggle)
			fxIconToggle.ToggleIcon(fxEnabled);
		SaveSoundSettings();
	}
	private void SaveSoundSettings()
	{
		PlayerPrefs.SetInt(MusicEnabledKey, musicEnabled ? 1 : 0);
		PlayerPrefs.SetInt(FXEnabledKey, fxEnabled ? 1 : 0);
		PlayerPrefs.SetFloat(MusicVolumeKey, musicVolume);
		PlayerPrefs.SetFloat(FXVolumeKey, fxVolume);
		PlayerPrefs.Save();
	}
	private void LoadSoundSettings()
	{
		musicEnabled = PlayerPrefs.GetInt(MusicEnabledKey, 1) == 1;
		fxEnabled = PlayerPrefs.GetInt(FXEnabledKey, 1) == 1;
		musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1.0f);
		fxVolume = PlayerPrefs.GetFloat(FXVolumeKey, 1.0f);
	}
}
