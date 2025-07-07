using R3;
using UnityEngine;
using System.Linq;

namespace Stages
{
    public class StagePresenter : MonoBehaviour
    {
        StageModel _stageModel;

        void Start()
        {
            _stageModel = new StageModel();

            _stageModel.CurrentStageState
                .Subscribe(state =>
                {
                    // ここでステージの状態に応じた処理を行う
                    Debug.Log($"Current Stage State: {state}");
                })
                .AddTo(this);
        }

        /// <summary>
        /// 外部からStageModelの状態変更を呼び出すためのメソッド
        /// </summary>
        public void SwitchToRandomState()
        {
            _stageModel.SwitchToRandomStateEfficient();
        }
    }

    public enum StageState
    {
        Right,
        Left,
        Up,
    }

    /// <summary>
    /// ステージの状態を管理するモデル
    /// </summary>
    public class StageModel
    {
        ReactiveProperty<StageState> _currentStageState = new ReactiveProperty<StageState>(StageState.Right);
        public ReadOnlyReactiveProperty<StageState> CurrentStageState => _currentStageState;

        public void SwitchStageState(StageState state)
        {
            _currentStageState.Value = state;
        }

        /// <summary>
        /// 今の状態を除くランダムな状態に切り替える
        /// </summary>
        public void SwitchToRandomStateEfficient()
        {
            var availableStates = System.Enum.GetValues(typeof(StageState))
                .Cast<StageState>()
                .Where(state => state != _currentStageState.Value)
                .ToArray();

            if (availableStates.Length > 0)
            {
                int randomIndex = Random.Range(0, availableStates.Length);
                _currentStageState.Value = availableStates[randomIndex];
            }
        }
    }
}
