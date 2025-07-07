using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using UnityEngine;

namespace InGame.Player
{
    public interface IJump
    {
        void Jump();
    }

    public class PlayerMovement : MonoBehaviour, IJump
    {
        [SerializeField] float _moveForce = 0.1f;
        [SerializeField] float _maxSpeedX = 2f;
        [SerializeField] float _maxSpeedY = 10f;
        [SerializeField] float _jumpSpeed = 3f;
        [SerializeField] float _pressHoldTime = 0.5f;

        Rigidbody2D _rb2;
        Collider2D _collider2D;
        float _pressedTime = 0f; // クラスフィールドに移動

        void Start()
        {
            _rb2 = GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<Collider2D>();

            this.UpdateAsObservable()
            .Where(_ => Time.timeScale > 0)
            .Subscribe(_ => Move())
            .AddTo(this);

            // クリック開始時
            this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            .Subscribe(_ =>
            {
                _pressedTime = Time.time;
            })
            .AddTo(this);

            // クリック継続中
            this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButton(0) || Input.touchCount > 0)
            .Subscribe(_ =>
            {
                float holdDuration = Time.time - _pressedTime;
                if (holdDuration >= _pressHoldTime && !_collider2D.isTrigger)
                {
                    _collider2D.isTrigger = true;
                }
            })
            .AddTo(this);

            // クリック終了時
            this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonUp(0) && Input.touchCount == 0)
            .Subscribe(_ =>
            {
                if (_collider2D.isTrigger)
                {
                    _collider2D.isTrigger = false;
                }
            })
            .AddTo(this);

            // Y軸方向の速度制限
            this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                if (_rb2.linearVelocityY > _maxSpeedY)
                {
                    _rb2.linearVelocity = new Vector2(_rb2.linearVelocityX, _maxSpeedY);
                }
                else if (_rb2.linearVelocityY < -_maxSpeedY)
                {
                    _rb2.linearVelocity = new Vector2(_rb2.linearVelocityX, -_maxSpeedY);
                }
            })
            .AddTo(this);
        }

        void Move()
        {
            float mousePosX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            float halfWidth = rectTransform.rect.width * rectTransform.localScale.x / 2f;

            if (Mathf.Abs(mousePosX - gameObject.transform.position.x) < halfWidth)
            {
                return; // Do not move if the mouse is within half the width of the game object
            }

            int dir = mousePosX > gameObject.transform.position.x ? 1 : -1;

            if (_rb2.linearVelocityX * dir < _maxSpeedX)
            {
                _rb2.AddForce(new Vector2(_moveForce * dir, 0f), ForceMode2D.Force);
            }
        }

        public void Jump()
        {
            _rb2.linearVelocity = new Vector2(_rb2.linearVelocity.x, _jumpSpeed);
        }
    }
}
