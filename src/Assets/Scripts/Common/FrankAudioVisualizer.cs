using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Common
{
    public class FrankAudioVisualizer
    {
        private List<Transform> VisualBars;
        private int CountOfVisualizer { get { return VisualBars.Count; } }

        /// <summary>
        /// 传入一个visual bar的组合后构造实例
        /// </summary>
        /// <param name="visualBarsGroup">要求bar组合下必须包含一组子物体</param>
        public FrankAudioVisualizer(Transform visualBarsGroup)
        {
            if (!visualBarsGroup)
                throw new ArgumentNullException("Visual bars must be a group of gameobjects!");

            VisualBars = new List<Transform>();
            GetAllChildren(visualBarsGroup);
        }


        public void Render(float[] spectrum)
        {
            float interpolation;
            for (int i=0; i<CountOfVisualizer; i++)
            {
                interpolation = GetInterpolation (i, spectrum);
                VisualBars[i].localScale = new Vector3(1.0f, interpolation*100, 1.0f);
            }
        }

        //对频谱进行重新插值计算，得到第i个条所对应频率下的大概响度（Lerp计算值不一定是真实频响的强度）
        private float GetInterpolation(int i, float[] spectrum)
        {
            var max = spectrum.Length - 1;
            float scale = (float)CountOfVisualizer / max;
            float position = i / scale;

            var interpolate = Mathf.Lerp(spectrum[(int)position], spectrum[(int)position + 1], position - (int)position);
            return interpolate;
        }

        //获取到VisualBarGroup下所有的物体
        private void GetAllChildren(Transform transform)
        {
            foreach (Transform child in transform)
            {
                VisualBars.Add(child);
            }
        }

    }
}