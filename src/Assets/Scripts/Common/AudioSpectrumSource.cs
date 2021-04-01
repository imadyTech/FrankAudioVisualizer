using System;
using UnityEngine;

namespace Assets.Common
{

    public class AudioSpectrumSource
    {
        private AudioSourceType p_AudioSourceType;

        private int p_SpectrumLength { get; set; }

        private string p_AudioClipName { get; set; }

        private string p_RecordingDeviceName { get; set; }

        private AudioSource p_AudioSourceObject { get; set; }

        private AudioSource p_MicrophoneSourceObject { get; set; }


        private float[] p_SpectrumCache;

        public AudioSpectrumSource(AudioSource audiosource)
        {
            p_AudioSourceObject = audiosource;
            if (p_SpectrumLength <= 0) p_SpectrumLength = 64;

            p_SpectrumCache = new float[p_SpectrumLength];
            UpdateAudioSource();
        }

        public AudioSpectrumSource(int length, AudioSource audiosourceObject) : this(audiosourceObject)
        {
            p_SpectrumLength = length;
        }

        public AudioSpectrumSource(int length, AudioSource audiosourceObject, string audioClipName) : this(length, audiosourceObject)
        {
            p_AudioClipName = audioClipName;
        }

        public AudioSpectrumSource(int length, AudioSource audiosourceObject, string audioClipName, string deviceName) : this(length, audiosourceObject, audioClipName)
        {
            p_RecordingDeviceName = deviceName;
        }

        public AudioSpectrumSource(int length, AudioSource audiosourceObject, AudioSource microphoneSource, string audioClipName, string deviceName) : this(length, audiosourceObject, audioClipName)
        {
            p_MicrophoneSourceObject = microphoneSource;
            p_RecordingDeviceName = deviceName;
        }

        public float[] GetSpectrumData()
        {
            AudioListener.GetSpectrumData(p_SpectrumCache, 0, FFTWindow.Rectangular);
            return p_SpectrumCache;
        }

        private AudioClip GetCurrentAudioFrame()
        {
            throw new NotImplementedException();
        }

        private AudioSpectrumSource Interpolate(int length)
        {
            throw new NotImplementedException();
        }

        public void ChangeSource(AudioSourceType audioType)
        {
            if (p_AudioSourceType == audioType)
                return;
            p_AudioSourceType = audioType;
            UpdateAudioSource();
        }

        public void UpdateAudioSource()
        {
            switch (p_AudioSourceType)
            {
                case AudioSourceType.audioSource:
                    {
                        AudioClip clip = Resources.Load<AudioClip>(p_AudioClipName);
                        p_AudioSourceObject.clip = clip;
                        p_AudioSourceObject.Play();
                        break;
                    }
                case AudioSourceType.microphone:
                    {
                        //44100音频采样率   固定格式
                        AudioClip micRecordClip = Microphone.Start(p_RecordingDeviceName, true, 999, 44100);
                        p_MicrophoneSourceObject.clip = micRecordClip;
                        p_MicrophoneSourceObject.Play();
                        break;
                    }
                case AudioSourceType.mixed:
                    {
                        AudioClip clip = Resources.Load<AudioClip>(p_AudioClipName);
                        p_AudioSourceObject.clip = clip;
                        p_AudioSourceObject.Play();

                        AudioClip micRecordClip = Microphone.Start(p_RecordingDeviceName, true, 999, 44100);
                        p_MicrophoneSourceObject.clip = micRecordClip;
                        p_MicrophoneSourceObject.Play();
                        break;
                    }
            }
        }
    }



    public enum AudioSourceType
    {
        microphone = 1,
        audioSource = 2,
        mixed = 3
    }


}