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
        Debug.Log($"{this} �¾� �Ϸ�");
    }

    public void PlaySFX(int index, Vector3 playPos = default)
    {
        if (audios.TryGetValue(index, out AudioClip newSFX) == false)
            return;

        for (int i = 0; i < SFXSources.Count; i++)
        {
            if (SFXSources[i].isPlaying == false)
            {
                // 3D ���� ������ ���� �۾��ε� �ʿ���� ����
                SFXSources[i].transform.position = playPos;

                SFXSources[i].clip = newSFX;
                SFXSources[i].volume = SFXVolume;
                SFXSources[i].Play();
                Debug.Log($"SFX ��� => {newSFX}");
                return;
            }
        }

        // �����ų �� ������ ���ο� AudioSource ���ӿ�����Ʈ ���� �� ����
        GameObject obj = new GameObject();
        obj.transform.SetParent(sourceRoot);
        obj.name = "SFXSource";
        var newSFXSource = obj.AddComponent<AudioSource>();
        SFXSources.Add(newSFXSource);

        // ���� ���
        Debug.Log(SFXSources[SFXSources.Count - 1] == newSFXSource);
        SFXSources[SFXSources.Count - 1].transform.position = playPos;
        SFXSources[SFXSources.Count - 1].clip = newSFX; 
        SFXSources[SFXSources.Count - 1].volume = SFXVolume;
        SFXSources[SFXSources.Count - 1].Play();
        Debug.Log($"�ű� SFX ��� => {newSFX}");
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
