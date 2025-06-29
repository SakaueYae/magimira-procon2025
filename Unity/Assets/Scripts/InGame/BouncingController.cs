using UnityEngine;
using WebGLBridge;

/// <summary>
/// ボールを指定した時間間隔でリズミカルにバウンスさせるコントローラー
/// </summary>
[RequireComponent(typeof(Rigidbody2D))] // Rigidbody2Dが必須であることを示す
public class BouncingController : MonoBehaviour
{
    [Header("バウンス周期（秒）")]
    [Tooltip("一度着地してから、次に着地するまでの時間")]
    float _bounceInterval = 0f;

    Rigidbody2D _rb2;
    JSExecutor _jsExecutor;

    void Start()
    {
        // Rigidbody 2Dコンポーネントを事前に取得しておく
        _rb2 = GetComponent<Rigidbody2D>();
        _jsExecutor = new JSExecutor();
    }

    // 他のコライダーに触れた瞬間に呼ばれる
    // TODO: 触れたコライダーの判定
    private void OnCollisionEnter2D(Collision2D collision)
    {
        SwitchBounce(true);
    }

    public void SwitchBounce(bool isBounce)
    {
        if (isBounce)
        {
            float nextInterval = _jsExecutor.GetNextBeatFromJS();
            SetBounceInterval(nextInterval / 1000f); // msから秒に変換
            Debug.Log("Next beat received: " + _bounceInterval);
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
        float requiredVelocity = -0.5f * Physics2D.gravity.y * _rb2.gravityScale * _bounceInterval;

        // 計算した速度をボールに適用する。
        // 水平方向の速度は維持し、垂直方向の速度だけを上書きする。
        _rb2.linearVelocity = new Vector2(_rb2.linearVelocity.x, requiredVelocity);
    }

    public void SetBounceInterval(float interval)
    {
        // バウンス間隔を設定する
        _bounceInterval = interval;
        Debug.Log("Bounce interval set to: " + _bounceInterval + " seconds");
    }
}

