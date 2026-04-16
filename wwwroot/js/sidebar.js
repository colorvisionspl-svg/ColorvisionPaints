document.addEventListener('DOMContentLoaded', () => {
    // ═══════════════════════════════════════
    // USER MENU DROPDOWN
    // ═══════════════════════════════════════
    const userArea = document.getElementById('topbar-user');
    const dropdown = document.getElementById('user-dropdown');

    if (userArea && dropdown) {
        userArea.addEventListener('click', (e) => {
            e.stopPropagation();
            dropdown.classList.toggle('open');
        });

        document.addEventListener('click', () => {
            dropdown.classList.remove('open');
        });

        dropdown.addEventListener('click', (e) => {
            e.stopPropagation();
        });
    }

    // ═══════════════════════════════════════
    // MOBILE NAVIGATION
    // ═══════════════════════════════════════
    const hamburger = document.getElementById('hamburger-btn');
    const sidebar = document.getElementById('sidebar');
    const overlay = document.getElementById('sidebar-overlay');

    if (hamburger) {
        hamburger.addEventListener('click', () => {
            sidebar?.classList.toggle('open');
            overlay?.classList.toggle('open');
        });
    }

    if (overlay) {
        overlay.addEventListener('click', () => {
            sidebar?.classList.remove('open');
            overlay?.classList.remove('open');
        });
    }

    // Highlight active sidebar item based on URL
    const currentPath = window.location.pathname.toLowerCase();
    document.querySelectorAll('.sidebar-nav-item').forEach(item => {
        const itemHref = item.getAttribute('href')?.toLowerCase();
        if (itemHref && itemHref !== '#' && currentPath.includes(itemHref)) {
            item.classList.add('active');
        } else if (itemHref === currentPath) {
             item.classList.add('active');
        }
    });
});
