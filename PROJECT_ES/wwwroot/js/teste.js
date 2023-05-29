'use strict';

/**
 * navbar variables
 */

window.addEventListener('scroll', function() {
  const header = document.querySelector('.header');
  const scrollPosition = window.scrollY;

  if (scrollPosition > 0) {
    header.classList.add('active');
  } else {
    header.classList.remove('active');
  }
});


/**
 * go top
 */

const goTopBtn = document.querySelector("[data-go-top]");

window.addEventListener("scroll", function () {

  window.scrollY >= 500 ? goTopBtn.classList.add("active") : goTopBtn.classList.remove("active");

});


/**
 * scroll the slider
 */
const slider = document.getElementById('moviesSlider');
let isDown = false;
let startX;
let scrollLeft;

slider.addEventListener('mousedown', (e) => {
  if (e.button !== 0) return; // Ignore right-click or middle-click

  isDown = true;
  startX = e.pageX - slider.offsetLeft;
  scrollLeft = slider.scrollLeft;
  e.preventDefault();
});

window.addEventListener('mouseup', () => {
  isDown = false;
});

window.addEventListener('mousemove', (e) => {
  if (!isDown) return;

  e.preventDefault();
  const x = e.pageX - slider.offsetLeft;
  const walk = (x - startX) * 1.5; // Adjust the scroll speed
  slider.scrollTo({
    left: scrollLeft - walk,
    behavior: 'smooth' // Use smooth scrolling behavior
  });
});