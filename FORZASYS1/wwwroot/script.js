document.addEventListener('DOMContentLoaded', () => {
    const filterButtons = document.querySelectorAll('.filter-button');
    const searchInput = document.querySelector('.search-input');
  
    const videos = [
      { title: 'Video 1', category: 'Goals' },
      { title: 'Video 2', category: 'Assists' },
      { title: 'Video 3', category: 'Red Cards' },
      { title: 'Video 4', category: 'Goals' },
      { title: 'Video 5', category: 'Assists' },
      { title: 'Video 6', category: 'Red Cards' },
    ];
  
    filterButtons.forEach(button => {
      button.addEventListener('click', () => {
        filterVideos(videos, button.dataset.filter, searchInput.value);
      });
    });
  
    searchInput.addEventListener('input', () => {
      filterVideos(videos, 'all', searchInput.value);
    });
  });
  
  function filterVideos(videos, filter, search) {
    const filteredVideos = videos.filter(video => {
      const titleMatches = video.title.toLowerCase().includes(search.toLowerCase());
      const categoryMatches = filter === 'all' || video.category === filter;
      return titleMatches && categoryMatches;
    });
  
    updateVideoSuggestions(filteredVideos);
  }
  
  function updateVideoSuggestions(videos) {
    const videoSuggestions = document.querySelector('.video-suggestions');
    videoSuggestions.innerHTML = '';
  
    videos.forEach(video => {
      const videoElement = document.createElement('div');
      videoElement.classList.add('small-video');
      videoElement.innerHTML = `
        <h3>${video.title}</h3>
        <p>${video.category}</p>
      `;
      videoSuggestions.appendChild(videoElement);
    });
  }