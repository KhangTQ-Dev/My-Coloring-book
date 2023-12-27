using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClipsButton;

    [SerializeField] private AudioSource audioSourceBG;

    [SerializeField] private AudioSource audioSourceButton;

    [SerializeField] private AudioSource audioSourceDraw;

    public void PlaySoundButton(TypeSoundButton typeSoundButton)
    {
        if (GameManager.Instance.DataManager.GetSound())
        {
            audioSourceButton.clip = audioClipsButton[(int)typeSoundButton];

            audioSourceButton.Play();
        }
    }

    public void SetVolumnBG(bool isTrue)
    {
        audioSourceBG.volume = isTrue? 0.7f : 0;
    }

    public void PlaySoundBG()
    {
        if (GameManager.Instance.DataManager.GetMusic())
        {
            audioSourceBG.Play();
        }
    }

    public void PlaySoundDraw()
    {
        if (GameManager.Instance.DataManager.GetSound())
        {
            audioSourceDraw.Play();
        }
    }
}

public enum TypeSoundButton
{
    Defaul,
    Tab,
    ButtonColor
}
