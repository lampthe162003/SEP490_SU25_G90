// Toggle dropdown menu
const userIcon = document.getElementById('userIcon');
const userDropdown = document.getElementById('userDropdown');

userIcon.addEventListener('click', () => {
    const isVisible = userDropdown.style.display === 'block';
    userDropdown.style.display = isVisible ? 'none' : 'block';
});

window.addEventListener('click', function (e) {
    if (!userIcon.contains(e.target) && !userDropdown.contains(e.target)) {
        userDropdown.style.display = 'none';
    }
});

// Toggle sidebar (responsive)
const toggleBtn = document.getElementById('toggleSidebar');
const sidebar = document.getElementById('sidebar');

toggleBtn.addEventListener('click', () => {
    sidebar.classList.toggle('collapsed');
    const spans = sidebar.querySelectorAll('.menu-item span');
    spans.forEach(span => span.style.display = span.style.display === 'none' ? 'inline' : 'none');
});
