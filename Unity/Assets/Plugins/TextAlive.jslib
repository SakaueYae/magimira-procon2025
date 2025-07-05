mergeInto(LibraryManager.library, {
  GetNextBeat: function () {
    const nextInterval = getNextBeat();
    return nextInterval;
  },

  GetIsTimingCorrect: function () {
    console.log("JSLib", isTimingCorrect());
    return isTimingCorrect();
  },
});
