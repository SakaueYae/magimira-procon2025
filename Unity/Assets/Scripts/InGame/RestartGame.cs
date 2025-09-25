using UnityEngine;
using WebGLBridge;

public class RestartGame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        JSExecutor.PlayMusicFromStartFromJS();
    }
}
