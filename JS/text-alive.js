const { Player } = TextAliveApp;

// TextAlive Player を作る
const player = new Player({
  app: { token: "JF9mqDMSLPVFLM1V" },
  mediaElement: document.querySelector("#media"),
  mediaBannerPosition: "bottom right",

  // オプション一覧
  // https://developer.textalive.jp/packages/textalive-app-api/interfaces/playeroptions.html
});

// 単語が発声されていたら #text に表示する
const animateWord = function (now, unit) {
  if (unit.contains(now)) {
    gameInstance.SendMessage("JSTransmitter", "OnWord", this.text);
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
  } else {
    player.requestPlay();
  }
});
