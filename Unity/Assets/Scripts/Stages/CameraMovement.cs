using R3.Triggers;
using UnityEngine;
using R3;

namespace Stages
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] float _moveSpeed = 1f; // 座標/s

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            this.UpdateAsObservable()
                .Where(_ => Time.timeScale > 0)
                .Subscribe(_ =>
                {
                    transform.position += new Vector3(_moveSpeed * Time.deltaTime, 0, 0);
                })
                .AddTo(this);
        }
    }
}
