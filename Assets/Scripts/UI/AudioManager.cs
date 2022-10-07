using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] public AudioSource[] soundEffects;

    [SerializeField] public AudioSource bgm, levelEndMusic, dieMusic;

    private void Awake()
    {
        instance = this;
    }
#region Sound 
    public void PlaySFX(int soundToPlay)
    {
        soundEffects[soundToPlay].Stop();
        soundEffects[soundToPlay].pitch = Random.Range(.9f, 1.1f);
        soundEffects[soundToPlay].Play();
    }

    public void StopSFX(int soundToPlay)
    {
        soundEffects[soundToPlay].Stop();
    }
#endregion

#region Music

    public void stopMusic()
    {
        bgm.Stop();
    }
     public void playMusic()
    {
        bgm.Play();
    }
    
    public void DieMusic()
    {
        bgm.Stop();
        dieMusic.Play();
    }
    public void PlayLevelVictory()
    {
        bgm.Stop();
        levelEndMusic.Play();
    }

    #endregion
}
