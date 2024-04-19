document.addEventListener('DOMContentLoaded', () => {
    // Attach event listeners once the DOM is fully loaded
    attachEventListeners();

    const filterButtons = document.querySelectorAll('.filter-button');
    const searchInput = document.querySelector('.search-input');
    const videos = [
        { title: 'Video 1', category: 'Goals', url: 'path/to/video1.mp4', thumbnailUrl: 'path/to/thumbnail1.jpg' },
        { title: 'Video 2', category: 'Assists', url: 'path/to/video2.mp4', thumbnailUrl: 'path/to/thumbnail2.jpg' },
        // Additional videos...
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

function attachEventListeners() {
    const clickableElements = document.querySelectorAll('.video-thumbnail, .video-link');

    clickableElements.forEach(element => {
        element.addEventListener('click', function(event) {
            event.preventDefault();
            playVideo(this.dataset.videoUrl, 'mainVideoPlayer', this.dataset.title);
        });
    });
}

function playVideo(videoUrl, videoId, videoTitle) {
    const videoPlayer = document.getElementById(videoId);
    const thumbnailImage = videoPlayer.previousElementSibling; // Ensure this targets the <img> element

    if (thumbnailImage) {
        thumbnailImage.style.display = 'none'; // Hide the thumbnail image
    }

    if (videoPlayer) {
        const videoSource = videoPlayer.querySelector('source');
        const mainVideoTitleElement = document.getElementById('mainVideoTitle');

        videoSource.src = videoUrl;
        videoPlayer.load(); // Load the new video

        // Listen for when the video can play through, and then play the video
        videoPlayer.oncanplaythrough = () => {
            videoPlayer.play();
        };

        videoPlayer.style.display = 'block'; // Make sure the video player is visible

        // Update the title if the element is found
        if (mainVideoTitleElement) {
            mainVideoTitleElement.textContent = videoTitle;
        }
    } else {
        console.error(`No video player found with ID: ${videoId}`);
    }
}





function filterVideos(videos, filter, search) {
    const filteredVideos = videos.filter(video => video.title.toLowerCase().includes(search.toLowerCase()) &&
        (filter === 'all' || video.category === filter));
    updateVideoSuggestions(filteredVideos);
}

function updateVideoSuggestions(videos) {
    const videoSuggestions = document.querySelector('.video-suggestions');
    videoSuggestions.innerHTML = '';

    videos.forEach(video => {
        const videoElement = document.createElement('div');
        videoElement.classList.add('small-video');
        videoElement.innerHTML = `<h3>${video.title}</h3>
            <img src="${video.thumbnailUrl}" alt="Thumbnail for ${video.title}" class="video-thumbnail" data-video-url="${video.url}" data-title="${video.title}">
            <a href="#" class="video-link" data-video-url="${video.url}" data-title="${video.title}">Watch Video</a>`;
        videoSuggestions.appendChild(videoElement);
    });
}
