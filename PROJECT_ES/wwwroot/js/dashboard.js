﻿const menuBtn = document.querySelector("#menu-btn");
const sideMenu = document.querySelector("aside");
const closeBtn = document.querySelector("#close-btn");
const themeToggler = document.querySelector(".theme-toggler");
const logoImg = document.querySelector(".logo img");

// OPEN SIDEBAR
menuBtn.addEventListener("click", () => {
    sideMenu.classList.add("active");
});

// CLOSE SIDEBAR
closeBtn.addEventListener("click", () => {
    sideMenu.classList.remove("active");
});

// CHANGE THEME
themeToggler.addEventListener("click", () => {
    document.body.classList.toggle("dark-theme-variables");

    if (localStorage.getItem("darkMode")) {
        localStorage.removeItem("darkMode");
    } else {
        localStorage.setItem("darkMode", document.body.className);
    }

    themeToggler.querySelector("span:first-child").classList.toggle("active");
    themeToggler.querySelector("span:last-child").classList.toggle("active");
});

