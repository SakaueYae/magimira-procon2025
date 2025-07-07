using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using Stages;
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
        [SerializeField] float _maxSpeed = 2f;
        [SerializeField] float _jumpSpeed = 3f;

        Rigidbody2D _rb2;

        void Start()
        {
            _rb2 = GetComponent<Rigidbody2D>();
            this.UpdateAsObservable()
            .Where(_ => Time.timeScale > 0)
            .Subscribe(_ => Move())
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

            if (_rb2.linearVelocityX * dir < _maxSpeed)
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
