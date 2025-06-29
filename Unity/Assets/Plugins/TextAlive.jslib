mergeInto(LibraryManager.library, {
  GetNextBeat: function () {
    const nextInterval = getNextBeat();
    console.log(nextInterval);
    return nextInterval;
  },
});
