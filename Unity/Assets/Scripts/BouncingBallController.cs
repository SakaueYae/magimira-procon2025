using UnityEngine;
using System.Runtime.InteropServices;
using System.Globalization;

/// <summary>
/// ボールを指定した時間間隔でリズミカルにバウンスさせるコントローラー
/// </summary>
[RequireComponent(typeof(Rigidbody2D))] // Rigidbody2Dが必須であることを示す
public class BouncingBallController : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern float GetNextBeat();

    [Header("バウンス周期（秒）")]
    [Tooltip("一度着地してから、次に着地するまでの時間")]
    float bounceInterval = 0f;

    private Rigidbody2D rb;

    void Start()
    {
        // Rigidbody 2Dコンポーネントを事前に取得しておく
        rb = GetComponent<Rigidbody2D>();
    }

    // 他のコライダーに触れた瞬間に呼ばれる
    private void OnCollisionEnter2D(Collision2D collision)
    {
        SwitchBounce(true);
    }

    public void SwitchBounce(bool isBounce)
    {
        if (isBounce)
        {
            float nextInterval = GetNextBeat();
            SetBounceInterval(nextInterval / 1000f); // msから秒に変換
            Debug.Log("Next beat received: " + bounceInterval);
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
        float requiredVelocity = -0.5f * Physics2D.gravity.y * bounceInterval;

        // 計算した速度をボールに適用する。
        // 水平方向の速度は維持し、垂直方向の速度だけを上書きする。
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, requiredVelocity);
    }

    public void SetBounceInterval(float interval)
    {
        // バウンス間隔を設定する
        bounceInterval = interval;
        Debug.Log("Bounce interval set to: " + bounceInterval + " seconds");
    }
}

