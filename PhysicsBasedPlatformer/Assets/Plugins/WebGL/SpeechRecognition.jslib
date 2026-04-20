mergeInto(LibraryManager.library, {
  StartSpeechRecognition: function () {
    var recognition = new (window.SpeechRecognition || window.webkitSpeechRecognition)();
    recognition.lang = 'en-US';
    recognition.interimResults = false;
    recognition.continuous = true;

    recognition.onresult = function (event) {
      var transcript = event.results[event.results.length - 1][0].transcript.trim().toLowerCase();
      SendMessage('SpeechManager', 'OnSpeechResult', transcript);
    };

    recognition.onerror = function (e) {
      console.warn('Speech error:', e.error);
    };

    recognition.start();
    window._speechRecognition = recognition;
  },

  StopSpeechRecognition: function () {
    if (window._speechRecognition) {
      window._speechRecognition.stop();
    }
  }
});