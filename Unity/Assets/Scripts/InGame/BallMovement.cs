using System;
using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using UnityEngine;

namespace InGame
{
    public class BallMovement : MonoBehaviour
    {
        [SerializeField] float _moveForce = 0.1f;
        [SerializeField] float _maxSpeed = 2f;
        [SerializeField] float _jumpForce = 1f;

        Rigidbody2D _rb2;

        void Start()
        {
            _rb2 = GetComponent<Rigidbody2D>();
            this.UpdateAsObservable()
            .Subscribe(_ => Move())
            .AddTo(this);

            this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonDown(0))
            .Subscribe(_ => Jump())
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

        void Jump()
        {
            Debug.Log("Jump triggered");
            _rb2.AddForce(new Vector2(0f, _jumpForce), ForceMode2D.Impulse);
        }
    }
}
