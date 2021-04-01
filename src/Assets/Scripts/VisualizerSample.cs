using System.Collections;
using UnityEngine;
using Assets.Common;
using System;

namespace Assets.Scripts
{
    public class VisualizerSample : MonoBehaviour
    {
        private AudioSpectrumSource p_spectrumSource = null;

        [Tooltip("此处设置监听对象，如果是audiolistener请保证有效的audiosource。")]
        //默认只监听麦克风
        public AudioSourceType m_AudioSourceType;


        //默认最小为64
        public int VisualizerCount = 64;
        //从Editor界面赋值
        public AudioSource MusicSource;
        public AudioSource MicrophoneSource;
        public string DefaultMusicName = "song";

        public float[] SpectrumData;

        public GameObject GraphicVisualizer;
        private FrankAudioVisualizer m_Visualizer;

        void OnEnable()
        {
            string currentDevice = string.Empty;
            //获取设备麦克风；如果没有激活的麦克风，则自动转换成音频
            try
            {
                string[] devices = Microphone.devices;
                if (devices.Length <= 0)
                    throw new Exception("None of microphone devices was found!");
                currentDevice = devices[0];
            }
            catch (Exception e)
            {
                m_AudioSourceType = AudioSourceType.audioSource;
            }
            //获取music音频源
            p_spectrumSource = new AudioSpectrumSource(VisualizerCount, MusicSource, MicrophoneSource, DefaultMusicName, currentDevice);

            p_spectrumSource.ChangeSource(m_AudioSourceType);

            m_Visualizer = new FrankAudioVisualizer(GraphicVisualizer.transform);
        }

        // Update is called once per frame
        void Update()
        {
            //退出
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            SpectrumData = p_spectrumSource.GetSpectrumData();
            RenderVisualizer(SpectrumData);
        }

        private void RenderVisualizer(float[] spectrum)
        {
            m_Visualizer.Render(spectrum);
        }

    }
}