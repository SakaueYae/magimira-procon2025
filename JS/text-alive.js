const { Player } = TextAliveApp;

// TextAlive Player を作る
const player = new Player({
  app: { token: "JF9mqDMSLPVFLM1V" },
  mediaElement: document.querySelector("#media"),
  mediaBannerPosition: "none",

  // オプション一覧
  // https://developer.textalive.jp/packages/textalive-app-api/interfaces/playeroptions.html
});

const animateWord = function (now, unit) {
  if (unit.contains(now)) {
    gameInstance.SendMessage("JSMessageReceiver", "OnWord", this.text);
  }
};

const animatePhrase = function (now, unit) {
  if (unit.contains(now)) {
    gameInstance.SendMessage("JSMessageReceiver", "OnPhrase", this.text);
  }
};

let segments = [];
let prevSegment = null;

player.addListener({
  onAppReady: (app) => {
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
    let p = player.video.firstPhrase;

    while (w) {
      w.animate = animateWord;
      w = w.next;
    }

    while (p) {
      p.animate = animatePhrase;
      p = p.next;
    }

    segments = player.data.songMap.segments.sort((a, b) => {
      return a.segments[0].startTime - b.segments[0].startTime;
    });
    console.log("セグメント情報", segments);

    gameInstance.SendMessage(
      "JSMessageReceiver",
      "GetWordsCount",
      player.video.wordCount
    );
  },

  onTimeUpdate: (pos) => {
    // if (player.findChorus(pos)) {
    //   console.log("コーラス", player.findChorus(pos));
    // } else {
    const currentSegment = segments.find(({ segments }) => {
      return segments.some((s) => {
        return s.startTime <= pos && pos <= s.endTime;
      });
    });

    // 初回のセグメント
    if (!prevSegment) {
      currentSegment && (prevSegment = currentSegment);
    }

    if (currentSegment && currentSegment !== prevSegment) {
      console.log("セグメントが変わりました", prevSegment, currentSegment);
      prevSegment = currentSegment;
      gameInstance.SendMessage("JSMessageReceiver", "OnSegmentChange");
    }
    // }

    if (pos === player.video.duration) {
      gameInstance.SendMessage("JSMessageReceiver", "OnMusicEnd");
    }
  },
});

function getNextBeat() {
  /** 前回のビート情報 */
  let lastBeat = null;
  /** 今の経過時間 */
  const pos = player.timer.position;
  /** 現在のビート情報 */
  const beat = player.findBeat(pos);
  const RAG_MS = 100;

  // posが0の場合beatが取得できないので、playerから最初のビートを取得する
  if (pos === 0) {
    const firstBeat = player.getBeats()[0];
    if (!firstBeat) return "0";
    lastBeat = firstBeat;

    console.log(player.getChoruses());

    return firstBeat.duration;
  }

  if (!beat) return "0";

  // 前回のビートと現在のビートが同じ場合
  if (lastBeat && lastBeat.index === beat.index) {
    const end =
      lastBeat.endTime - pos < RAG_MS
        ? lastBeat.next.endTime
        : lastBeat.endTime;
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

function isTimingCorrect() {
  const pos = player.timer.position;
  const beat = player.findBeat(pos);

  if (!beat) return false;

  const RAG_MS = 100;

  return (
    Math.abs(beat.endTime - pos) < RAG_MS ||
    Math.abs(beat.startTime - pos) < RAG_MS
  );
}
