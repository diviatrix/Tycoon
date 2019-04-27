using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource musicPlayer,soundPlayer;
    public AudioClip bgMusic;
    public AudioClip selectSound;
    public AudioClip buildSound;
    public AudioClip sellSound;

    // Start is called before the first frame update
    
    void OnEnable()
	{
		EventManager.OnSelection += playSelectSound;
		EventManager.OnBuild += playBuildSound;
	}

	void OnDisable()
	{
		EventManager.OnSelection -= playSelectSound;
		EventManager.OnBuild -= playBuildSound;
	}

    void Start()
    {
        musicPlayer = gameObject.AddComponent<AudioSource>();
        musicPlayer.loop = true;
        musicPlayer.volume = 0.15f;
        musicPlayer.clip = bgMusic;
        musicPlayer.Play();

        soundPlayer = gameObject.AddComponent<AudioSource>();
        soundPlayer.loop = false;
    }

    void playSelectSound(SceneObjectBehavior sob)
    {
        soundPlayer.clip = selectSound;
        soundPlayer.Play();
    }

    void playBuildSound(SceneObjectBehavior sob)
    {
        soundPlayer.clip = buildSound;
        soundPlayer.Play();
    }

    public void PlaySellSound()
    {
        soundPlayer.clip = sellSound;
        soundPlayer.Play();
    }

    public void SwitchMusicVolume()
    {
        if (musicPlayer.volume == 0) musicPlayer.volume = 0.15f;
        else musicPlayer.volume = 0;
    }
}
