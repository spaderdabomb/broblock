using JSAM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public static DebugManager Instance;

    public void PlayMusicTest()
    {
        AudioManager.PlayMusic(AudioLibraryMusic.Upbeat_Intense);
    }

    public void StopMusicTest()
    {
        AudioManager.StopMusic(AudioLibraryMusic.Upbeat_Intense);
    }

    public void PlayMusicTest2()
    {
        AudioManager.PlayMusic(AudioLibraryMusic.FullSpeed);
    }

    public void StopMusicTest2()
    {
        AudioManager.StopMusic(AudioLibraryMusic.FullSpeed);
    }
}
