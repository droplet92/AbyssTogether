mergeInto(LibraryManager.library, {
  RegisterFirstInput: function (callback) {
    var canvas = Module['canvas'];

    function onFirstInput() {
      canvas.removeEventListener('pointerup', onFirstInput);
      window.removeEventListener('keydown', onFirstInput);
      {{{ makeDynCall('v', 'callback') }}}();
    }

    canvas.addEventListener('pointerup', onFirstInput);
    window.addEventListener('keydown', onFirstInput);
  }
});
