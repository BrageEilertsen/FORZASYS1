document.addEventListener('DOMContentLoaded', () => {
    // Attach event listeners once the DOM is fully loaded
    attachEventListeners();

    const filterButton = document.getElementById('filterButton');
    const filterOptions = document.getElementById('filterOptions');

    filterButton.addEventListener('click', () => {
        if (filterOptions.style.display === 'none') {
            filterOptions.style.display = 'block';
        } else {
            filterOptions.style.display = 'none';
        }
    });

    const searchInput = document.getElementById('search-input');
    let debounceTimer;

    searchInput.addEventListener('input', () => {
        clearTimeout(debounceTimer);
        debounceTimer = setTimeout(async () => {
            const searchTerm = searchInput.value;
            await fetchVideos(searchTerm);
        }, 500);  // Adjust the delay as needed
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

async function fetchVideos(searchTerm) {
    try {
        const response = await fetch(`/api/videos?searchTerm=${encodeURIComponent(searchTerm)}`);
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        const videos = await response.json();
        updateVideoSuggestions(videos);
    } catch (error) {
        console.error('Error fetching videos:', error);
    }
}

function updateVideoSuggestions(videos) {
    const videoSuggestions = document.querySelector('.video-suggestions');
    videoSuggestions.innerHTML = '';

    if (videos.length === 0) {
        videoSuggestions.innerHTML = '<p>No videos found. Try a different search!</p>';
        return;
    }

    const mainVideo = videos[0];
    const sideVideos = videos.slice(1);

    // Main video
    const mainVideoElement = document.createElement('div');
    mainVideoElement.classList.add('main-video');
    mainVideoElement.innerHTML = `
        <div class="video-container">
            <img src="${mainVideo.thumbnailUrl}" alt="Thumbnail" class="video-thumbnail" data-video-url="${mainVideo.url}" data-title="${mainVideo.title}" onclick="playVideo('${mainVideo.url}', 'mainVideoPlayer', '${mainVideo.title}')">
            <video id="mainVideoPlayer" controls style="display: none;">
                <source src="" type="video/mp4">
                Your browser does not support the video tag.
            </video>
        </div>
        <h3 id="mainVideoTitle">${mainVideo.title}</h3>
    `;
    videoSuggestions.appendChild(mainVideoElement);

    // Side videos
    const sideVideosContainer = document.createElement('div');
    sideVideosContainer.classList.add('side-videos');
    sideVideos.forEach(video => {
        const videoElement = document.createElement('div');
        videoElement.classList.add('small-video');
        videoElement.innerHTML = `
            <img src="${video.thumbnailUrl}" alt="Thumbnail for ${video.title}" class="video-thumbnail" data-video-url="${video.url}" data-title="${video.title}" onclick="playVideo('${video.url}', 'mainVideoPlayer', '${video.title}')">
            <h3>${video.title}</h3>
            <a href="javascript:void(0);" class="video-link" data-video-url="${video.url}" data-title="${video.title}" onclick="playVideo('${video.url}', 'mainVideoPlayer', '${video.title}')">Watch Video</a>
        `;
        sideVideosContainer.appendChild(videoElement);
    });
    videoSuggestions.appendChild(sideVideosContainer);

    // Re-attach event listeners for dynamically added elements
    attachEventListeners();
}
