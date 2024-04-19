document.addEventListener('DOMContentLoaded', () => {
    // Attach event listeners once the DOM is fully loaded
    attachEventListeners();

    function attachEventListeners() {
        const videoThumbnails = document.querySelectorAll('.video-thumbnail');
        videoThumbnails.forEach(thumbnail => {
            thumbnail.addEventListener('click', function(event) {
                event.preventDefault();
                playVideo(this.dataset.videoUrl, 'mainVideoPlayer', this.dataset.title);
            });
        });
    }

    function playVideo(videoUrl, videoId, videoTitle) {
        const videoPlayer = document.getElementById(videoId);
        const videoSource = videoPlayer.querySelector('source');
        const mainVideoTitle = document.getElementById('mainVideoTitle'); // Ensure you have an element with this ID

        if (videoPlayer && videoSource && mainVideoTitle) {
            videoSource.src = videoUrl;
            videoPlayer.load();
            videoPlayer.play();
            videoPlayer.style.display = 'block'; // Make sure the video player is visible
            mainVideoTitle.textContent = videoTitle; // Update the title dynamically
        } else {
            console.error(`No video player or source found with ID: ${videoId}`);
        }
    }






// Setup filter functionality
    const filterButtons = document.querySelectorAll('.filter-button');
    const searchInput = document.querySelector('.search-input');
    const videos = [
        { title: 'Video 1', category: 'Goals', url: 'path/to/video1.mp4', thumbnailUrl: 'path/to/thumbnail1.jpg' },
        { title: 'Video 2', category: 'Assists', url: 'path/to/video2.mp4', thumbnailUrl: 'path/to/thumbnail2.jpg' },
        { title: 'Video 3', category: 'Red Cards', url: 'path/to/video3.mp4', thumbnailUrl: 'path/to/thumbnail3.jpg' },
        { title: 'Video 4', category: 'Goals', url: 'path/to/video4.mp4', thumbnailUrl: 'path/to/thumbnail4.jpg' },
        { title: 'Video 5', category: 'Assists', url: 'path/to/video5.mp4', thumbnailUrl: 'path/to/thumbnail5.jpg' },
        { title: 'Video 6', category: 'Red Cards', url: 'path/to/video6.mp4', thumbnailUrl: 'path/to/thumbnail6.jpg' }
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

function playVideo(videoUrl, videoId) {
    const videoPlayer = document.getElementById(videoId);
    const thumbnailImage = videoPlayer.previousElementSibling; // Assuming <img> is right before <video>

    thumbnailImage.style.display = 'none';
    videoPlayer.style.display = 'block';
    videoPlayer.querySelector('source').src = videoUrl;
    videoPlayer.load();
    videoPlayer.play();
}

function filterVideos(videos, filter, search) {
    const filteredVideos = videos.filter(video => {
        return video.title.toLowerCase().includes(search.toLowerCase()) && (filter === 'all' || video.category === filter);
    });
    updateVideoSuggestions(filteredVideos);
}

function updateVideoSuggestions(videos) {
    const videoSuggestions = document.querySelector('.video-suggestions');
    videoSuggestions.innerHTML = '';  // Clear the current videos

    videos.forEach(video => {
        const videoElement = document.createElement('div');
        videoElement.classList.add('small-video');
        videoElement.innerHTML = `
            <h3>${video.title}</h3>
            <img src="${video.thumbnailUrl}" alt="Thumbnail for ${video.title}" class="video-thumbnail" data-video-url="${video.url}">
            <a href="#" class="video-link" data-video-url="${video.url}">Watch Video</a>
        `;
        videoSuggestions.appendChild(videoElement);
    });
}
