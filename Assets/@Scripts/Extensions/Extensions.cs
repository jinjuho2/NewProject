using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Extensions
{
    public static class Extension
    {
        public static void BindAsync(this Button button, Func<UniTask> asyncAction)
        {
            button.onClick.AddListener(() =>
            {
                asyncAction().Forget();
            });
        }

        public static string FormatPlayTime(float seconds)
        {
            int totalSeconds = Mathf.FloorToInt(seconds);

            int hours = totalSeconds / 3600;
            int minutes = (totalSeconds % 3600) / 60;
            int secs = totalSeconds % 60;

            return $"{hours:00}:{minutes:00}:{secs:00}";
        }
    }
}