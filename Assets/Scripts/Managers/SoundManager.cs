using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    private AudioSource[] audioSources = new AudioSource[(int)Define.Sound.MaxCount];

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if(root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));
            for(int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject temp = new GameObject { name = soundNames[i] };
                audioSources[i] = temp.AddComponent<AudioSource>();
                temp.transform.parent = root.transform;
            }

            audioSources[(int)Define.Sound.Bgm].loop = true;
        }
    }

    public void Clear()
    {
        foreach(AudioSource source in audioSources)
        {
            source.clip = null;
            source.Stop();
        }
        audioClips.Clear();
    }

    /// <summary>
    /// ���� ��� �Լ�
    /// </summary>
    /// <param name="_path">���� ���� ���</param>
    /// <param name="_type">���� Ÿ��</param>
    /// <param name="_pitch">���� ������</param>
    public void Play(string _path, Define.Sound _type = Define.Sound.Sfx, float _pitch = 1f)
    {
        AudioClip clip = GetOrAddAudioClip(_path, _type);
        Play(clip, _type, _pitch);
    }

    /// <summary>
    /// ���� ��� �Լ�
    /// </summary>
    /// <param name="_path">���� ���� ���</param>
    /// <param name="_type">���� Ÿ��</param>
    /// <param name="_pitch">���� ������</param>
    public void Play(AudioClip _clip, Define.Sound _type = Define.Sound.Sfx, float _pitch = 1f)
    {
        if (_clip == null) return;

        if (_type == Define.Sound.Bgm)
        {   // TODO: ����� ���
            AudioSource source = audioSources[(int)Define.Sound.Bgm];
            if (source.isPlaying) source.Stop();
            source.pitch = _pitch;
            source.clip = _clip;
            source.Play();
        }
        else
        {   // TODO: ȿ���� ���
            AudioSource source = audioSources[(int)Define.Sound.Sfx];
            source.pitch = _pitch;
            source.PlayOneShot(_clip);
        }
    }

    /// <summary>
    /// ���� ������ �������ų� ���� �߰� �ϴ� �Լ�
    /// </summary>
    /// <param name="_path">���� ���� ���</param>
    /// <param name="_type">���� Ÿ��</param>
    /// <returns></returns>
    private AudioClip GetOrAddAudioClip(string _path, Define.Sound _type = Define.Sound.Sfx)
    {
        if (!_path.Contains("Sounds/")) _path = $"Sounds/{_path}";

        AudioClip clip = null;

        if (_type == Define.Sound.Bgm)
        {   // TODO: ����� ���
            clip = Managers.Resource.Load<AudioClip>(_path);
            if (clip == null)
            {
                Debug.LogWarning($"AudioClip Missing! {_path}");
            }
        }
        else
        {   // TODO: ȿ���� ���
            if (!audioClips.TryGetValue(_path, out clip))
            {
                clip = Managers.Resource.Load<AudioClip>(_path);
                audioClips.Add(_path, clip);
            }

            if (clip == null)
            {
                Debug.LogWarning($"AudioClip Missing! {_path}");
            }
        }
        return clip;
    }
}