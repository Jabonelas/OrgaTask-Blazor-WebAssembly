window.carousel = {
    init: function () {
        let currentSlide = 0;
        const slides = document.getElementById('slides');
        const dots = document.querySelectorAll('.dot');
        const totalSlides = 3;

        function updateCarousel() {
            slides.style.transform = `translateX(-${currentSlide * 33.33}%)`;
            dots.forEach((dot, index) => {
                dot.classList.toggle('active', index === currentSlide);
            });
        }

        window.nextSlide = function () {
            currentSlide = (currentSlide + 1) % totalSlides;
            updateCarousel();
        };

        window.prevSlide = function () {
            currentSlide = (currentSlide - 1 + totalSlides) % totalSlides;
            updateCarousel();
        };

        window.goToSlide = function (index) {
            currentSlide = index;
            updateCarousel();
        };

        // Auto-rotate
        let autoSlide = setInterval(window.nextSlide, 5000);

        // Pausar auto-rotate quando o mouse entra
        document.querySelector('.hero-carousel').addEventListener('mouseenter', () => {
            clearInterval(autoSlide);
        });

        // Retomar auto-rotate quando o mouse sai
        document.querySelector('.hero-carousel').addEventListener('mouseleave', () => {
            autoSlide = setInterval(window.nextSlide, 5000);
        });

        // Inicializar o carrossel
        updateCarousel();
    }
};