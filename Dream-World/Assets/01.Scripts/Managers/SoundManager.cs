using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundManager
{
    public SerializableDictionary<int, AudioClip> audios;

    public AudioSource BGMSource;
    public AudioSource SubBGMSource;

    public List<AudioSource> SFXSources = new List<AudioSource>(15);

    [Range(0f, 1f)]
    public float BGMVolume = 1.0f;

    [Range(0f, 1f)]
    public float SFXVolume = 1.0f;

    [SerializeField]
    private GameObject _root;

    public void Init()
    {
        GameObject obj = new GameObject();
        var newSFXSource = obj.AddComponent<AudioSource>();
        obj.name = "SFXSource";

        Managers.Pool.Pop(obj);
    }

    public void PlaySFX(int index, Vector3 playPos = default)
    {
        if (audios.TryGetValue(index, out AudioClip newSFX) == false)
            return;

        for (int i = 0; i < SFXSources.Count; i++)
        {
            if (SFXSources[i].isPlaying == false)
            {
                Play(SFXSources[i], newSFX, playPos);
                Debug.Log($"SFX 재생 => {newSFX}");
                return;
            }
        }

        // 재생시킬 수 없으면 새로운 AudioSource 게임오브젝트 생성 및 저장
        GameObject obj = new GameObject();
        var newSFXSource = obj.AddComponent<AudioSource>();
        obj.name = "SFXSource";

        // SoundSource List에도 넣고 Pool에도 넣기
        SFXSources.Add(newSFXSource);
        Managers.Pool.Pop(obj);

        Play(SFXSources[SFXSources.Count - 1], newSFX, playPos);
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

    private void Play(AudioSource source, AudioClip sound, Vector3 playPos)
    {
        CoroutineUtil.StartCoroutine(PlayCO(source, sound, playPos));
    }

    IEnumerator PlayCO(AudioSource source, AudioClip sound, Vector3 playPos)
    {
        source.transform.position = playPos;
        source.clip = sound;
        source.volume = SFXVolume;
        source.Play();

        // SFX 길이만큼 대기
        yield return new WaitForSeconds(source.clip.length);

        // 풀에 넣어주기
        Managers.Pool.Push(source.gameObject);

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
