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
                Debug.Log($"SFX ��� => {newSFX}");
                return;
            }
        }

        // �����ų �� ������ ���ο� AudioSource ���ӿ�����Ʈ ���� �� ����
        GameObject obj = new GameObject();
        var newSFXSource = obj.AddComponent<AudioSource>();
        obj.name = "SFXSource";

        // SoundSource List���� �ְ� Pool���� �ֱ�
        SFXSources.Add(newSFXSource);
        Managers.Pool.Pop(obj);

        Play(SFXSources[SFXSources.Count - 1], newSFX, playPos);
    }

    public void PlayBGM(int index)
    {
        if (audios.TryGetValue(index, out AudioClip newBGM) == false)
            return;

        Debug.Log($"BGM ��� => {newBGM}");
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

        // SFX ���̸�ŭ ���
        yield return new WaitForSeconds(source.clip.length);

        // Ǯ�� �־��ֱ�
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
