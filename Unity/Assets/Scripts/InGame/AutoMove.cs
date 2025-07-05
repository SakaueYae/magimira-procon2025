using R3.Triggers;
using UnityEngine;
using R3;

namespace InGame
{
    public class AutoMove : MonoBehaviour
    {
        [SerializeField] GameObject _player;
        [SerializeField] float _moveForce = 0f;
        [SerializeField] float _maxSpeed = 2f;

        Rigidbody2D _playerRb;
        Rigidbody2D _cpuRb;
        float _movePosX = 0f;

        void Start()
        {
            _playerRb = _player.GetComponent<Rigidbody2D>();
            _cpuRb = GetComponent<Rigidbody2D>();

            // 初期位置を設定
            gameObject.transform.position = new Vector3(_movePosX, gameObject.transform.position.y, gameObject.transform.position.z);

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    // プレイヤーの位置に応じて自動で移動
                    HandleAutoMove();
                })
                .AddTo(this);
        }

        void HandleAutoMove()
        {
            // 移動方向を決定する変数（-1: 左, 1: 右, 0: 動かない）
            float moveDirection = 0;

            if (_playerRb.linearVelocityX != 0)
            {
                // CPUのX座標とプレイヤーのX座標を比較
                if (transform.position.x < _player.transform.position.x)
                {
                    // プレイヤーが右側にいる場合、CPUは左に逃げる
                    moveDirection = -1;
                }
                else if (transform.position.x > _player.transform.position.x)
                {
                    // プレイヤーが左側にいる場合、CPUは右に逃げる
                    moveDirection = 1;
                }

            }

            if (Mathf.Abs(_cpuRb.linearVelocityX) < _maxSpeed)
            {
                // プレイヤーの速度が最大速度未満の場合、移動力を適用
                _cpuRb.AddForce(new Vector2(_moveForce * moveDirection, 0f), ForceMode2D.Force);
            }
        }
    }
}
