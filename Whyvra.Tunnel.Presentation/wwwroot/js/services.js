document.addEventListener('highlight', () => {
  document.querySelectorAll('pre code').forEach((block) => {
      hljs.highlightBlock(block);
  });
});

window.HighlightService = {
  trigger: () => {
      document.dispatchEvent(new CustomEvent('highlight'))
  }
};

window.ClipboardService = {
  copyText: (text) => {
      navigator.clipboard.writeText(text)
          .catch((error) => console.log);
  }
};