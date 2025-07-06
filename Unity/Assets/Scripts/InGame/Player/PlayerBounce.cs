using R3.Triggers;
using UnityEngine;
using WebGLBridge;
using R3;

namespace InGame.Player
{
    public class PlayerBounce : MonoBehaviour, IBounce
    {
        [Header("バウンス周期（秒）")]
        [Tooltip("一度着地してから、次に着地するまでの時間")]
        float _bounceIntervalSec = 0f;

        Rigidbody2D _rb2;
        JSExecutor _jsExecutor;
        IJump _jumpController;
        IJumpAnim _jumpAnim;
        float _prevBouceTimeSec;
        float _nextBounceTimeSec;

        void Start()
        {
            _rb2 = GetComponent<Rigidbody2D>();
            _jsExecutor = new JSExecutor();
            _jumpController = gameObject.GetComponent<IJump>();
            _jumpAnim = gameObject.GetComponent<IJumpAnim>();

            this.UpdateAsObservable()
                .Where(_ => Input.touchCount > 0 || Input.GetMouseButtonDown(0))
                .Where(_ => _jsExecutor.IsTimingCorrectFromJS())
                .Subscribe(_ =>
                {
                    _jumpController.Jump();
                })
                .AddTo(this);

            this.OnCollisionEnter2DAsObservable()
                .Where(collision => collision.gameObject.GetComponent<IPlatform>() != null)
                .Where(collision => IsLandingOnTop(collision))
                .Subscribe(collision =>
                {
                    _jumpAnim.LandAnim();
                    // 着地した瞬間の時間を記録
                    _prevBouceTimeSec = Time.time;
                    HandleBounce(true);
                })
                .AddTo(this);
        }

        public void HandleBounce(bool isBounce)
        {
            if (isBounce)
            {
                float nextInterval = _jsExecutor.GetNextBeatFromJS();
                SetBounceInterval(nextInterval / 1000f); // msから秒に変換
            }
            else
            {
                SetBounceInterval(0f);
            }
            Bounce();
        }

        /// <summary>
        /// ボールをバウンスさせる処理
        /// </summary>
        private void Bounce()
        {
            // 目的の滞空時間（bounceInterval）を実現するために必要な、垂直方向の初速度を計算する。
            // 物理公式: v = -0.5 * g * t  (v:初速, g:重力加速度, t:時間)
            // x = v0 * t + 0.5 * g * t^2
            // 0 = v0 * t + 0.5 * g * t^2
            // v0 = -0.5 * g * t
            float requiredVelocity = -0.5f * Physics2D.gravity.y * _rb2.gravityScale * _bounceIntervalSec;

            // 計算した速度をボールに適用する。
            // 水平方向の速度は維持し、垂直方向の速度だけを上書きする。
            _rb2.linearVelocity = new Vector2(_rb2.linearVelocity.x, requiredVelocity);

            // 次のバウンス予定時間を設定（_prevBouceTimeSecは既にOnCollisionEnter2Dで設定済み）
            _nextBounceTimeSec = _prevBouceTimeSec + _bounceIntervalSec;

            _jumpAnim.JumpUpAnim();
        }

        public void SetBounceInterval(float interval)
        {
            // バウンス間隔を設定する
            _bounceIntervalSec = interval;
        }

        /// <summary>
        /// プレイヤーが足場の上に着地したかどうかを判定する
        /// </summary>
        /// <param name="collision">衝突情報</param>
        /// <returns>足場の上に着地した場合はtrue</returns>
        private bool IsLandingOnTop(Collision2D collision)
        {
            // 法線ベクトルによる判定
            // 
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // 法線ベクトルが上向き（Y成分が正）の場合、プレイヤーが上から衝突している
                if (contact.normal.y > 0.7f) // 0.7fは閾値（約45度以上上向き）
                {
                    return true;
                }
            }

            return false;
        }

    }

}