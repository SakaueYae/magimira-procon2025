using UnityEngine;
using R3;
using WebGLBridge;

namespace InGame
{
    public class DataTransmitter : MonoBehaviour
    {
        [SerializeField] JSMessageReceiver _jsMessageReceiver;

        [SerializeField] SongWordGenerator _songWordGenerator;
        [SerializeField] BouncingController _ballController;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _jsMessageReceiver.WordStream()
                .Subscribe(word =>
                {
                    _songWordGenerator.GenerateSongWord(word);
                })
                .AddTo(this);

            _jsMessageReceiver.BeatStream()
                .Subscribe(interval =>
                {
                    _ballController.SetBounceInterval(interval / 1000f);
                    Debug.Log("Bounce interval set to: " + interval);
                })
                .AddTo(this);

            _jsMessageReceiver.IsPlaying
                .Subscribe((isPlaying) =>
                {
                    _ballController.SwitchBounce(isPlaying);
                })
                .AddTo(this);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
