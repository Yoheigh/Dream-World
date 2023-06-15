using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundManager
{
    public SerializableDictionary<int, AudioClip> audios;

    public AudioSource BGMSource;
    public List<AudioSource> SFXSources = new List<AudioSource>(15);

    [Range(0f, 1f)]
    public float BGMVolume = 1.0f;

    [Range(0f, 1f)]
    public float SFXVolume = 1.0f;

    [SerializeField]
    private Transform sourceRoot;

    public void Setup()
    {
        Debug.Log($"{this} 셋업 완료");
    }

    public void PlaySFX(int index, Vector3 playPos = default)
    {
        if (audios.TryGetValue(index, out AudioClip newSFX) == false)
            return;

        for (int i = 0; i < SFXSources.Count; i++)
        {
            if (SFXSources[i].isPlaying == false)
            {
                // 3D 사운드 구현을 위한 작업인데 필요없을 수도
                SFXSources[i].transform.position = playPos;

                SFXSources[i].clip = newSFX;
                SFXSources[i].volume = SFXVolume;
                SFXSources[i].Play();
                Debug.Log($"SFX 재생 => {newSFX}");
                return;
            }
        }

        // 재생시킬 수 없으면 새로운 AudioSource 게임오브젝트 생성 및 저장
        GameObject obj = new GameObject();
        obj.transform.SetParent(sourceRoot);
        obj.name = "SFXSource";
        var newSFXSource = obj.AddComponent<AudioSource>();
        SFXSources.Add(newSFXSource);

        // 실제 재생
        Debug.Log(SFXSources[SFXSources.Count - 1] == newSFXSource);
        SFXSources[SFXSources.Count - 1].transform.position = playPos;
        SFXSources[SFXSources.Count - 1].clip = newSFX; 
        SFXSources[SFXSources.Count - 1].volume = SFXVolume;
        SFXSources[SFXSources.Count - 1].Play();
        Debug.Log($"신규 SFX 재생 => {newSFX}");
    }

    public void PlayBGM(int index)
    {
        if (audios.TryGetValue(index, out AudioClip newBGM) == false)
            return;

        Debug.Log($"BGM 재생 => {newBGM}");
        BGMSource.clip = newBGM;
        BGMSource.volume = BGMVolume;
        BGMSource.Play();
    }

    private IEnumerator FadeOut(AudioSource audioSource, float lerpTime = 2f)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume < 0.0f)
        {
            audioSource.volume -= startVolume * Time.deltaTime / lerpTime;
            yield return null;
        }

        audioSource.Stop();
    }
}
