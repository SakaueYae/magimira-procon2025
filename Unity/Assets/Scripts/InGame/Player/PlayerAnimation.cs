using R3;
using R3.Triggers;
using UnityEngine;

namespace InGame.Player
{
    public interface IJumpAnim
    {
        void JumpUpAnim();
        void LandAnim();
    }
    public class PlayerAnimation : MonoBehaviour, IJumpAnim
    {
        Animator _animator;
        Rigidbody2D _rb2;
        float _previousVelocityY;
        const float VELOCITY_THRESHOLD = 0.1f; // 速度の閾値

        void Start()
        {
            _animator = gameObject.GetComponent<Animator>();
            _rb2 = gameObject.GetComponent<Rigidbody2D>();
            // _animator.SetBool("isGrounded", true);

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    float currentVelocityY = _rb2.linearVelocity.y;

                    _previousVelocityY = currentVelocityY;

                    _animator.SetFloat("velocityY", currentVelocityY);
                })
                .AddTo(this);

            this.OnCollisionEnter2DAsObservable()
                .Subscribe(collision =>
                {
                    // 着地した瞬間の時間を記録
                    _animator.SetBool("isGrounded", true);
                })
                .AddTo(this);
        }

        public void JumpUpAnim()
        {
            // _animator.SetTrigger("jump");
            _animator.SetBool("isGrounded", false);
        }

        public void LandAnim()
        {
            // _animator.SetTrigger("land");
            _animator.SetBool("isGrounded", true);
            Debug.Log("LandAnim called");
        }
    }
}
