mergeInto(LibraryManager.library, {
  GetNextBeat: function () {
    const nextInterval = getNextBeat();
    return nextInterval;
  },

  GetIsTimingCorrect: function () {
    return isTimingCorrect();
  },

  PauseMusic: function () {
    player.requestPause();
  },

  ResumeMusic: function () {
    player.requestPlay();
  },
});
