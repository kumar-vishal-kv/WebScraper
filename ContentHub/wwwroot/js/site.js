//async function fetchData() {
//    const url = document.getElementById("urlInput").value;
//    const loadingScreen = document.getElementById("loadingScreen");
//    const errorMessage = document.getElementById("errorMessage");
//    const toggleChartBtn = document.getElementById("toggleChartBtn");

//    // Show loading screen
//    loadingScreen.style.display = "flex";
//    errorMessage.textContent = '';

//    try {
//        const response = await fetch(`/api/scrape?url=${encodeURIComponent(url)}`);
//        if (!response.ok) {
//            throw new Error('Invalid URL or server issue');
//        }
//        const data = await response.json();

//        // Hide loading screen
//        loadingScreen.style.display = "none";

//        // Display images
//        const imageContainer = document.getElementById("carousel");
//        imageContainer.innerHTML = '';
//        displayImages(data.images);

//        // Display word count table
//        displayWordTable(data.topWords);

//        // Enable chart button if there are 5 or more different words
//        if (data.topWords.length >= 5) {
//            toggleChartBtn.disabled = false;
//        } else {
//            toggleChartBtn.disabled = true;
//        }

//    } catch (error) {
//        loadingScreen.style.display = "none";
//        errorMessage.textContent = "Please check the URL.";
//    }
//}

//function displayImages(images) {
//    const imageContainer = document.getElementById("carousel");
//    if (images.length > 3) {
//        // Use Slick Carousel for more than 3 images
//        imageContainer.classList.add('slick-container');
//        images.forEach(imgSrc => {
//            const imgDiv = document.createElement("div");
//            const img = document.createElement("img");
//            img.src = imgSrc;
//            img.alt = "Image";
//            imgDiv.appendChild(img);
//            imageContainer.appendChild(imgDiv);
//        });

//        // Initialize Slick if there are more than 3 images
//        $(imageContainer).slick({
//            infinite: true,
//            slidesToShow: 3,
//            slidesToScroll: 1,
//            dots: true
//        });
//    } else {
//        // Display images in grid (1, 2, or 3 columns)
//        imageContainer.classList.remove('slick-container');
//        const columns = images.length;
//        const colClass = `col-${columns}`;
//        imageContainer.classList.add(colClass);

//        images.forEach(imgSrc => {
//            const img = document.createElement("img");
//            img.src = imgSrc;
//            img.alt = "Image";
//            imageContainer.appendChild(img);
//        });
//    }
//}

//function displayWordTable(topWords) {
//    const table = document.getElementById("wordCountTable");
//    table.innerHTML = `
//    <tr><th>Word</th><th>Count</th></tr>
//    ${topWords.map(word => `<tr><td>${word.word}</td><td>${word.count}</td></tr>`).join('')}
//  `;
//}

//function toggleChart() {
//    const chart = document.getElementById("wordChart");
//    const toggleChartBtn = document.getElementById("toggleChartBtn");

//    // Toggle chart visibility
//    chart.style.display = chart.style.display === 'none' ? 'block' : 'none';

//    if (chart.style.display === 'block') {
//        // Create Chart.js if not already created
//        const chartData = {
//            labels: chartData.topWords.map(word => word.word),
//            datasets: [{
//                data: chartData.topWords.map(word => word.count),
//                backgroundColor: 'rgba(75, 192, 192, 0.2)',
//                borderColor: 'rgba(75, 192, 192, 1)',
//                borderWidth: 1
//            }]
//        };

//        new Chart(chart, {
//            type: 'bar',
//            data: chartData,
//        });
//    }
//}

//function toggleFullWordCount() {
//    var fullWordCountDiv = document.getElementById('fullWordCount');
//var toggleBtn = document.getElementById('toggleFullWordCountBtn');

//if (fullWordCountDiv.style.display === 'none') {
//    fullWordCountDiv.style.display = 'block';
//toggleBtn.textContent = 'Hide Full Word Count';
//    } else {
//    fullWordCountDiv.style.display = 'none';
//toggleBtn.textContent = 'Full Word Count';
//    }
//}


