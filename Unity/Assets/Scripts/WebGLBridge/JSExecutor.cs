using System.Runtime.InteropServices;
using UnityEngine;

namespace WebGLBridge
{
    public class JSExecutor
    {
        [DllImport("__Internal")]
        private static extern float GetNextBeat();
        [DllImport("__Internal")]
        private static extern bool GetIsTimingCorrect();

        /// <summary>
        /// Calls the JavaScript function to get the next beat.
        /// </summary>
        /// <returns></returns>
        public float GetNextBeatFromJS()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                return GetNextBeat();
            }
            else
            {
                // If not running in WebGL, return a default value or handle accordinglyß
                return 511.29999999999995f; // Fallback value for non-WebGL platforms
            }
        }

        public bool IsTimingCorrectFromJS()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                return GetIsTimingCorrect();
            }
            else
            {
                // If not running in WebGL, return a default value or handle accordingly
                return true; // Fallback value for non-WebGL platforms
            }
        }
    }
}
