const { Player } = TextAliveApp;

// TextAlive Player を作る
const player = new Player({
  app: { token: "JF9mqDMSLPVFLM1V" },
  mediaElement: document.querySelector("#media"),
  mediaBannerPosition: "bottom right",

  // オプション一覧
  // https://developer.textalive.jp/packages/textalive-app-api/interfaces/playeroptions.html
});

const animateWord = function (now, unit) {
  if (unit.contains(now)) {
    gameInstance.SendMessage("TextAliveManager", "OnWord", this.text);
  }
};

player.addListener({
  onAppReady: (app) => {
    //  if (app.managed) {
    //   document.querySelector("#control").className = "disabled";
    // }
    if (!app.songUrl) {
      document.querySelector("#media").className = "disabled";

      // ストリートライト / 加賀(ネギシャワーP)
      player.createFromSongUrl("https://piapro.jp/t/ULcJ/20250205120202", {
        video: {
          // 音楽地図訂正履歴
          beatId: 4694275,
          chordId: 2830730,
          repetitiveSegmentId: 2946478,

          // 歌詞URL: https://piapro.jp/t/DPXV
          // 歌詞タイミング訂正履歴: https://textalive.jp/lyrics/piapro.jp%2Ft%2FULcJ%2F20250205120202
          lyricId: 67810,
          lyricDiffId: 20654,
        },
      });
    }
  },

  // 動画オブジェクトの準備が整ったとき（楽曲に関する情報を読み込み終わったとき）に呼ばれる
  onVideoReady: (v) => {
    // 定期的に呼ばれる各単語の "animate" 関数をセットする
    let w = player.video.firstWord;

    while (w) {
      w.animate = animateWord;
      w = w.next;
    }
  },
});

document.querySelector("#play-button").addEventListener("click", () => {
  if (player.isPlaying) {
    player.requestPause();
    // gameInstance.SendMessage("TextAliveManager", "OnBeat", "0");
    gameInstance.SendMessage("TextAliveManager", "OnPause");
  } else {
    player.requestPlay();
    // const duration = player.getBeats()[0].duration;

    // console.log(player.getBeats());

    // gameInstance.SendMessage("TextAliveManager", "OnBeat", duration.toString());
    gameInstance.SendMessage("TextAliveManager", "OnPlay");
  }
});

function getNextBeat() {
  /** 前回のビート情報 */
  let lastBeat = null;
  /** 今の経過時間 */
  const pos = player.timer.position;
  /** 現在のビート情報 */
  const beat = player.findBeat(pos);

  if (!beat) return "0";

  console.log(beat);

  // 前回のビートと現在のビートが同じ場合
  if (lastBeat && lastBeat.index === beat.index) {
    const end =
      lastBeat.endTime - pos < 0.02 ? lastBeat.next.endTime : lastBeat.endTime;
    const duration = end - pos;
    return duration;
  }
  // 前回のビートと現在のビートが異なる場合
  else {
    lastBeat = beat;
    const end = beat.endTime;
    const duration = end - pos;
    return duration;
  }
}
