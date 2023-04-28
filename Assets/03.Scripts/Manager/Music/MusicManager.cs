using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : SingletonBase<MusicManager>
{
    //BGM部分
    AudioSource BgmAudio = null;
    float BgmVolume = 1;

    public float BgmInitValue = 0.2f;

    //Sound音效部分
    GameObject SoundAudioHolder = null;
    List<AudioSource> soundAudioList = null;
    float SoundVolume = 1;

    public MusicManager()
    {
        //实时监测有无播放完的音效，进行销毁
        MonoManager.Instance.AddUpdateAction(CheckSound);
    }

    //BGM部分
    public void StartBgm(string BgmName, ResourceLoadWay resourceLoadWay=ResourceLoadWay.Addressables)
    {
        //创建一个空物体挂载Bgm的AudioSource
        if (BgmAudio == null)
        {
            GameObject bgmHolder = new GameObject("bgmHolder");
            BgmAudio = bgmHolder.AddComponent<AudioSource>();
            GameObject.DontDestroyOnLoad(bgmHolder);
        }
        //异步加载BGM资源,并播放
        ResourceManager.Instance.LoadAsync<AudioClip>(BgmName, (bgmClip) => {
            BgmAudio.clip = bgmClip;
            BgmAudio.loop = true;
            BgmAudio.volume = BgmVolume;
            BgmAudio.Play();
        },resourceLoadWay);
    }
    /// <summary>
    /// 暂停BGM
    /// </summary>
    public void PauseBgm()
    {
        if (BgmAudio != null)
        {
            BgmAudio.Pause();
        }
    }
    /// <summary>
    /// 停止BGM
    /// </summary>
    public void StopBgm()
    {
        if (BgmAudio != null)
        {
            BgmAudio.Stop();
        }
    }
    /// <summary>
    /// 调整BGM音量大小
    /// </summary>
    /// <param name="v"></param>
    public void ChangeBgmVolume(float v)
    {
        if (BgmAudio == null)
            return;
        BgmVolume = v;
        BgmAudio.volume = v;
    }
    /// <summary>
    /// 控制BGM开关
    /// mute后仍然播放但是静音
    /// </summary>
    /// <param name="isOpen"></param>
    public void SetBgmOpen(bool isOpen)
    {
        BgmAudio.mute = isOpen;
    }
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="name">音效文件的名字（路径）</param>
    /// <returns></returns>
    public void StartSound(string name, bool isLoop=false, Action<AudioSource> callback=null, ResourceLoadWay resourceLoadWay=ResourceLoadWay.Addressables)
    {
        if (SoundAudioHolder == null)
        {
            SoundAudioHolder = new GameObject("SoundAudioHolder");
            soundAudioList = new List<AudioSource>();
        }
        //第一个参数是音效文件名，第二个是AudioSource组件挂载到的游戏物体
        //第三个是初始化加载音效切片资源时调用的委托
        //第四个是后面从池里拿的委托
        ComponentPoolManager.Instance.GetItem<AudioSource>(name,  () =>
        {
            ResourceManager.Instance.LoadAsync<AudioClip>(name, (clip) =>
            {
                AudioSource source = null;
                var sourceArr = SoundAudioHolder.GetComponentsInParent<AudioSource>(true);
                bool isClipExist = false;
                for(int i = 0; i < sourceArr.Length; i++)
                {
                    if (sourceArr[i].clip.name == name)
                    {
                        isClipExist = true;
                        break;
                    }
                }
                if (isClipExist) return;
                source = SoundAudioHolder.AddComponent<AudioSource>();
                source.clip = clip;
                source.loop = isLoop;
                source.volume = SoundVolume;
                source.Play();
                soundAudioList.Add(source);
                callback?.Invoke(source);

            },resourceLoadWay);
        }, (source) =>
        {
            source.Play();
            soundAudioList.Add(source);
            callback?.Invoke(source);
        });
    }
    /// <summary>
    /// 停止音效
    /// </summary>
    /// <param name="source"></param>
    public void StopSound(AudioSource source)
    {
        if (soundAudioList.Contains(source))
        {
            source.Stop();
            soundAudioList.Remove(source);
            ComponentPoolManager.Instance.PushItem(source.clip.name, source);
        }
    }
    /// <summary>
    /// 改变音效音量
    /// </summary>
    /// <param name="v"></param>
    public void ChangeSoundVolum(float v)
    {
        if (soundAudioList != null && soundAudioList.Count >= 0)
        {
            SoundVolume = v;
            foreach (AudioSource source in soundAudioList)
            {
                source.volume = SoundVolume;
            }
        }
    }
    /// <summary>
    /// 检测有无播放完的音效组件并销毁
    /// </summary>
    public void CheckSound()
    {
        if (soundAudioList != null && soundAudioList.Count > 0)
        {
            for (int i = soundAudioList.Count - 1; i >= 0; i--)
            {
                if (!soundAudioList[i].isPlaying)
                {
                    AudioSource source = soundAudioList[i];
                    soundAudioList.RemoveAt(i);
                    ComponentPoolManager.Instance.PushItem(source.clip.name,source); 
                }
            }
        }
    }

}
