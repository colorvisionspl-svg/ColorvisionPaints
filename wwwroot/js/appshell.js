/*!
 * AppShell.js — ColorVision Paints ERP
 * Handles: Module tab switching · Sidebar rendering + navigation · User dropdown
 * Nexus Design System v2.0
 */

(function () {
  'use strict';

  /* ═══════════════════════════════════════════════════════════
     MODULE DATA — with URL routes for each page
     ═══════════════════════════════════════════════════════════ */

  const MODULES = {
    dashboard: {
      title: 'Dashboard',
      items: [
        { icon: 'layout-dashboard', label: 'Overview',   id: 'overview',   url: '/Dashboard/Admin' },
        { icon: 'file-text',        label: 'Financials', id: 'financials', url: '/Dashboard/Admin' },
        { icon: 'activity',         label: 'Operations', id: 'operations', url: '/Dashboard/Admin' },
        { icon: 'lightbulb',        label: 'Insights',   id: 'insights',   url: '/Dashboard/Admin' },
      ],
    },
    procurement: {
      title: 'Procurement',
      items: [
        { icon: 'file-text',     label: 'Purchase Orders',    id: 'purchase-orders',    url: '/Procurement/PurchaseOrders' },
        { icon: 'receipt',       label: 'Purchase Bills',     id: 'purchase-bills',     url: '/Procurement/PurchaseBills' },
        { icon: 'truck',         label: 'Vendors',            id: 'vendors',            url: '/Procurement/Vendors' },
        { icon: 'package-check', label: 'GRN',                id: 'grn',                url: '/Procurement/GRN' },
        { icon: 'package',       label: 'Raw Material Stock', id: 'raw-material-stock', url: '/Procurement/RawMaterialStock' },
      ],
    },
    inventory: {
      title: 'Inventory',
      items: [
        { icon: 'package',          label: 'Stock Dashboard', id: 'stock-dashboard', url: '/Dashboard/Admin' },
        { icon: 'plus-circle',      label: 'Adjust Stock',    id: 'adjust-stock',    url: '/Dashboard/Admin' },
        { icon: 'arrow-left-right', label: 'Transfers',       id: 'transfers',       url: '/Dashboard/Admin' },
        { icon: 'bar-chart-2',      label: 'Reports',         id: 'inv-reports',     url: '/Dashboard/Admin' },
      ],
    },
    manufacturing: {
      title: 'Manufacturing',
      items: [
        { icon: 'clipboard-list', label: 'Formulations',      id: 'formulations',      url: '/Manufacturing/Formulations' },
        { icon: 'flask-conical',  label: 'Production Orders', id: 'production-orders', url: '/Manufacturing/ProductionOrders' },
        { icon: 'check-circle',   label: 'Quality Control',   id: 'quality-control',   url: '/Manufacturing/QualityControl' },
        { icon: 'calendar',       label: 'Schedule',          id: 'mfg-schedule',      url: '/Manufacturing/Schedule' },
        { icon: 'activity',       label: 'Lines',             id: 'mfg-lines',         url: '/Manufacturing/Lines' },
      ],
    },

    dealers: {
      title: 'Dealers',
      items: [
        { icon: 'users',       label: 'All Dealers',       id: 'all-dealers',  url: '/Dashboard/Admin' },
        { icon: 'user-plus',   label: 'Onboarding',        id: 'onboarding',   url: '/Dashboard/Admin' },
        { icon: 'map',         label: 'Territory Map',     id: 'territory-map',url: '/Dashboard/Admin' },
        { icon: 'credit-card', label: 'Credit Management', id: 'credit-mgmt',  url: '/Dashboard/Admin' },
        { icon: 'award',       label: 'Performance',       id: 'dealer-perf',  url: '/Dashboard/Admin' },
      ],
    },
    sales: {
      title: 'Sales',
      items: [
        { icon: 'shopping-cart', label: 'Orders',   id: 'sales-orders', url: '/Dashboard/Admin' },
        { icon: 'file-text',     label: 'Invoices', id: 'invoices',     url: '/Dashboard/Admin' },
        { icon: 'truck',         label: 'Dispatch', id: 'dispatch',     url: '/Dashboard/Admin' },
        { icon: 'rotate-ccw',    label: 'Returns',  id: 'returns',      url: '/Dashboard/Admin' },
        { icon: 'tag',           label: 'Schemes',  id: 'schemes',      url: '/Dashboard/Admin' },
      ],
    },
    fieldteam: {
      title: 'Field Team',
      items: [
        { icon: 'clock',   label: 'Attendance',    id: 'attendance',    url: '/Dashboard/Admin' },
        { icon: 'route',   label: 'Beat Plans',    id: 'beat-plans',    url: '/Dashboard/Admin' },
        { icon: 'map-pin', label: 'Visit Reports', id: 'visit-reports', url: '/Dashboard/Admin' },
        { icon: 'receipt', label: 'Expenses',      id: 'expenses',      url: '/Dashboard/Admin' },
        { icon: 'radio',   label: 'Live Tracking', id: 'live-tracking', url: '/Dashboard/Admin' },
      ],
    },
    rewards: {
      title: 'QR Rewards',
      items: [
        { icon: 'qr-code',  label: 'QR Generation',   id: 'qr-gen',          url: '/Rewards/QRGeneration' },
        { icon: 'users',    label: 'Painter Profiles', id: 'painter-profiles',url: '/Rewards/PainterProfiles' },
        { icon: 'gift',     label: 'Redemptions',      id: 'redemptions',     url: '/Rewards/Redemptions' },
        { icon: 'megaphone',label: 'Campaigns',        id: 'campaigns',       url: '/Rewards/Campaigns' },
        { icon: 'shield',   label: 'Fraud Monitor',    id: 'fraud-monitor',   url: '/Rewards/FraudMonitor' },
        { icon: 'bar-chart-2',label: 'Analytics',      id: 'analytics',       url: '/Rewards/Analytics' },
      ],
    },
    finance: {
      title: 'Finance',
      items: [
        { icon: 'refresh-cw',   label: 'Tally Sync',  id: 'tally-sync',  url: '/Dashboard/Admin' },
        { icon: 'wallet',       label: 'Payments',    id: 'payments',    url: '/Dashboard/Admin' },
        { icon: 'alert-circle', label: 'Outstanding', id: 'outstanding', url: '/Dashboard/Admin' },
        { icon: 'bar-chart-2',  label: 'Reports',     id: 'fin-reports', url: '/Dashboard/Admin' },
      ],
    },
    settings: {
      title: 'Settings',
      items: [
        { icon: 'users',  label: 'Users',         id: 'settings-users',      url: '/Dashboard/Admin' },
        { icon: 'map',    label: 'Territories',   id: 'settings-territories',url: '/Dashboard/Admin' },
        { icon: 'shield', label: 'Roles',         id: 'settings-roles',      url: '/Dashboard/Admin' },
        { icon: 'bell',   label: 'Notifications', id: 'settings-notif',      url: '/Dashboard/Admin' },
        { icon: 'sliders',label: 'App Config',    id: 'settings-config',     url: '/Dashboard/Admin' },
      ],
    },
  };

  /* ═══════════════════════════════════════════════════════════
     URL → MODULE/PAGE DETECTION
     Determines which module tab and sidebar item to mark active
     based on the current browser URL.
     ═══════════════════════════════════════════════════════════ */
  function detectActiveFromUrl() {
    var path = window.location.pathname.toLowerCase();
    var bestModule = 'dashboard';
    var bestPage   = 'overview';

    for (var moduleId in MODULES) {
      var mod = MODULES[moduleId];
      for (var i = 0; i < mod.items.length; i++) {
        var item = mod.items[i];
        if (item.url && path === item.url.toLowerCase()) {
          bestModule = moduleId;
          bestPage   = item.id;
        }
      }
    }

    // Broader prefix match for Procurement pages
    if (path.startsWith('/procurement')) bestModule = 'procurement';
    if (path.startsWith('/dashboard'))   bestModule = 'dashboard';

    return { moduleId: bestModule, pageId: bestPage };
  }

  /* ═══════════════════════════════════════════════════════════
     LUCIDE SVG PATHS
     ═══════════════════════════════════════════════════════════ */
  const ICONS = {
    'layout-dashboard': '<rect x="3" y="3" width="7" height="7"/><rect x="14" y="3" width="7" height="7"/><rect x="14" y="14" width="7" height="7"/><rect x="3" y="14" width="7" height="7"/>',
    'file-text':        '<path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"/><polyline points="14 2 14 8 20 8"/><line x1="16" y1="13" x2="8" y2="13"/><line x1="16" y1="17" x2="8" y2="17"/><polyline points="10 9 9 9 8 9"/>',
    'activity':         '<polyline points="22 12 18 12 15 21 9 3 6 12 2 12"/>',
    'lightbulb':        '<line x1="9" y1="18" x2="15" y2="18"/><line x1="10" y1="22" x2="14" y2="22"/><path d="M15.09 14c.18-.98.65-1.74 1.41-2.5A4.65 4.65 0 0 0 18 8 6 6 0 0 0 6 8c0 1 .23 2.23 1.5 3.5A4.61 4.61 0 0 1 8.91 14"/>',
    'receipt':          '<path d="M4 2v20l2-1 2 1 2-1 2 1 2-1 2 1 2-1 2 1V2l-2 1-2-1-2 1-2-1-2 1-2-1-2 1-2-1Z"/><path d="M16 8h-6a2 2 0 1 0 0 4h4a2 2 0 1 1 0 4H8"/><path d="M12 17.5v-11"/>',
    'truck':            '<rect x="1" y="3" width="15" height="13"/><polygon points="16 8 20 8 23 11 23 16 16 16 16 8"/><circle cx="5.5" cy="18.5" r="2.5"/><circle cx="18.5" cy="18.5" r="2.5"/>',
    'package-check':    '<path d="m16 16 2 2 4-4"/><path d="M21 10V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l2-1.14"/><path d="m7.5 4.27 9 5.15"/><polyline points="3.29 7 12 12 20.71 7"/><line x1="12" y1="22" x2="12" y2="12"/>',
    'package':          '<line x1="16.5" y1="9.4" x2="7.5" y2="4.21"/><path d="M21 16V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16z"/><polyline points="3.27 6.96 12 12.01 20.73 6.96"/><line x1="12" y1="22.08" x2="12" y2="12"/>',
    'plus-circle':      '<circle cx="12" cy="12" r="10"/><line x1="12" y1="8" x2="12" y2="16"/><line x1="8" y1="12" x2="16" y2="12"/>',
    'arrow-left-right': '<path d="M8 3 4 7l4 4"/><path d="M4 7h16"/><path d="m16 21 4-4-4-4"/><path d="M20 17H4"/>',
    'bar-chart-2':      '<line x1="18" y1="20" x2="18" y2="10"/><line x1="12" y1="20" x2="12" y2="4"/><line x1="6" y1="20" x2="6" y2="14"/>',
    'clipboard-list':   '<rect x="8" y="2" width="8" height="4" rx="1" ry="1"/><path d="M16 4h2a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2H6a2 2 0 0 1-2-2V6a2 2 0 0 1 2-2h2"/><path d="M12 11h4"/><path d="M12 16h4"/><path d="M8 11h.01"/><path d="M8 16h.01"/>',
    'flask-conical':    '<path d="M14 2v6l2.5 7H7.5L10 8V2"/><path d="M6.2 2h11.6"/><path d="M4 20a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2Z"/>',
    'check-circle':     '<path d="M22 11.08V12a10 10 0 1 1-5.93-9.14"/><polyline points="22 4 12 14.01 9 11.01"/>',
    'calendar':         '<rect x="3" y="4" width="18" height="18" rx="2" ry="2"/><line x1="16" y1="2" x2="16" y2="6"/><line x1="8" y1="2" x2="8" y2="6"/><line x1="3" y1="10" x2="21" y2="10"/>',
    'users':            '<path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/>',
    'user-plus':        '<path d="M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><line x1="19" y1="8" x2="19" y2="14"/><line x1="22" y1="11" x2="16" y2="11"/>',
    'map':              '<polygon points="1 6 1 22 8 18 16 22 23 18 23 2 16 6 8 2 1 6"/><line x1="8" y1="2" x2="8" y2="18"/><line x1="16" y1="6" x2="16" y2="22"/>',
    'credit-card':      '<rect x="1" y="4" width="22" height="16" rx="2" ry="2"/><line x1="1" y1="10" x2="23" y2="10"/>',
    'award':            '<circle cx="12" cy="8" r="6"/><path d="M15.477 12.89 17 22l-5-3-5 3 1.523-9.11"/>',
    'shopping-cart':    '<circle cx="9" cy="21" r="1"/><circle cx="20" cy="21" r="1"/><path d="M1 1h4l2.68 13.39a2 2 0 0 0 2 1.61h9.72a2 2 0 0 0 2-1.61L23 6H6"/>',
    'rotate-ccw':       '<polyline points="1 4 1 10 7 10"/><path d="M3.51 15a9 9 0 1 0 .49-4H1"/>',
    'tag':              '<path d="M20.59 13.41l-7.17 7.17a2 2 0 0 1-2.83 0L2 12V2h10l8.59 8.59a2 2 0 0 1 0 2.82z"/><line x1="7" y1="7" x2="7.01" y2="7"/>',
    'clock':            '<circle cx="12" cy="12" r="10"/><polyline points="12 6 12 12 16 14"/>',
    'route':            '<circle cx="6" cy="19" r="3"/><path d="M9 19h8.5a3.5 3.5 0 0 0 0-7h-11a3.5 3.5 0 0 1 0-7H15"/><circle cx="18" cy="5" r="3"/>',
    'map-pin':          '<path d="M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0 1 18 0z"/><circle cx="12" cy="10" r="3"/>',
    'radio':            '<circle cx="12" cy="12" r="2"/><path d="M4.93 19.07a10 10 0 0 1 0-14.14"/><path d="M7.76 16.24a6 6 0 0 1 0-8.48"/><path d="M16.24 7.76a6 6 0 0 1 0 8.48"/><path d="M19.07 4.93a10 10 0 0 1 0 14.14"/>',
    'qr-code':          '<rect x="3" y="3" width="5" height="5"/><rect x="16" y="3" width="5" height="5"/><rect x="3" y="16" width="5" height="5"/><path d="M21 16h-3a2 2 0 0 0-2 2v3"/><path d="M21 21v.01"/><path d="M12 7v3a2 2 0 0 1-2 2H7"/><path d="M3 12h.01"/><path d="M12 3h.01"/><path d="M12 16v.01"/><path d="M16 12h1"/><path d="M21 12v.01"/>',
    'gift':             '<polyline points="20 12 20 22 4 22 4 12"/><rect x="2" y="7" width="20" height="5"/><line x1="12" y1="22" x2="12" y2="7"/><path d="M12 7H7.5a2.5 2.5 0 0 1 0-5C11 2 12 7 12 7z"/><path d="M12 7h4.5a2.5 2.5 0 0 0 0-5C13 2 12 7 12 7z"/>',
    'megaphone':        '<path d="m3 11 18-5v12L3 14v-3z"/><path d="M11.6 16.8a3 3 0 1 1-5.8-1.6"/>',
    'shield':           '<path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z"/>',
    'refresh-cw':       '<polyline points="23 4 23 10 17 10"/><polyline points="1 20 1 14 7 14"/><path d="M3.51 9a9 9 0 0 1 14.85-3.36L23 10M1 14l4.64 4.36A9 9 0 0 0 20.49 15"/>',
    'wallet':           '<path d="M21 12V7H5a2 2 0 0 1 0-4h14v4"/><path d="M3 5v14a2 2 0 0 0 2 2h16v-5"/><path d="M18 12a2 2 0 0 0 0 4h4v-4Z"/>',
    'alert-circle':     '<circle cx="12" cy="12" r="10"/><line x1="12" y1="8" x2="12" y2="12"/><line x1="12" y1="16" x2="12.01" y2="16"/>',
    'sliders':          '<line x1="4" y1="21" x2="4" y2="14"/><line x1="4" y1="10" x2="4" y2="3"/><line x1="12" y1="21" x2="12" y2="12"/><line x1="12" y1="8" x2="12" y2="3"/><line x1="20" y1="21" x2="20" y2="16"/><line x1="20" y1="12" x2="20" y2="3"/><line x1="1" y1="14" x2="7" y2="14"/><line x1="9" y1="8" x2="15" y2="8"/><line x1="17" y1="16" x2="23" y2="16"/>',
    'bell':             '<path d="M18 8A6 6 0 0 0 6 8c0 7-3 9-3 9h18s-3-2-3-9"/><path d="M13.73 21a2 2 0 0 1-3.46 0"/>',
    'chevron-right':    '<polyline points="9 18 15 12 9 6"/>',
  };

  /* ═══════════════════════════════════════════════════════════
     STATE
     ═══════════════════════════════════════════════════════════ */
  var activeModule = 'dashboard';
  var activePage   = 'overview';

  /* ═══════════════════════════════════════════════════════════
     HELPERS
     ═══════════════════════════════════════════════════════════ */
  function svgIcon(name, size) {
    var s = size || 16;
    var paths = ICONS[name] || '<circle cx="12" cy="12" r="4"/>';
    return '<svg xmlns="http://www.w3.org/2000/svg" width="' + s + '" height="' + s + '" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true">' + paths + '</svg>';
  }

  /* ═══════════════════════════════════════════════════════════
     RENDER SIDEBAR
     ═══════════════════════════════════════════════════════════ */
  function renderSidebar(moduleId, activePageId) {
    var mod = MODULES[moduleId];
    if (!mod) return;

    // Update title
    var titleEl = document.getElementById('sidebar-module-title');
    if (titleEl) titleEl.textContent = mod.title;

    // Render nav items
    var navEl = document.getElementById('sidebar-nav');
    if (!navEl) return;

    var currentPath = window.location.pathname.toLowerCase();

    var html = '';
    mod.items.forEach(function (item) {
      // Mark active based on current URL or passed activePageId
      var isActive = (activePageId && item.id === activePageId) ||
                     (!activePageId && item.url && currentPath === item.url.toLowerCase());

      var activeClass  = isActive ? ' active' : '';
      var chevronHtml  = isActive ? '<span class="sidebar-chevron">' + svgIcon('chevron-right', 14) + '</span>' : '';

      html += '<a href="' + (item.url || '#') + '" class="sidebar-nav-item' + activeClass + '" data-page="' + item.id + '" id="sidebarItem-' + item.id + '" aria-selected="' + isActive + '">'
        + svgIcon(item.icon, 16)
        + '<span>' + item.label + '</span>'
        + chevronHtml
        + '</a>';
    });

    navEl.innerHTML = html;
  }

  /* ═══════════════════════════════════════════════════════════
     MODULE TAB SWITCHING
     On tab click — if first item has a URL, navigate there.
     ═══════════════════════════════════════════════════════════ */
  function initModuleTabs() {
    var tabs = document.querySelectorAll('.module-tab');
    tabs.forEach(function (tab) {
      tab.addEventListener('click', function () {
        var moduleId = this.dataset.module;

        // Update tab active state
        tabs.forEach(function (t) {
          t.classList.remove('active');
          t.setAttribute('aria-selected', 'false');
        });
        this.classList.add('active');
        this.setAttribute('aria-selected', 'true');

        // Navigate to first item's URL if available
        var mod = MODULES[moduleId];
        if (mod && mod.items.length > 0 && mod.items[0].url) {
          var firstUrl = mod.items[0].url;
          // Don't navigate if the tab is already for the current module
          if (window.location.pathname.toLowerCase() !== firstUrl.toLowerCase()) {
            window.location.href = firstUrl;
            return;
          }
        }

        // Otherwise just re-render sidebar
        activeModule = moduleId;
        renderSidebar(moduleId);
      });
    });
  }

  /* ═══════════════════════════════════════════════════════════
     USER DROPDOWN
     ═══════════════════════════════════════════════════════════ */
  function initUserDropdown() {
    var userBtn  = document.getElementById('topbar-user');
    var dropdown = document.getElementById('user-dropdown');
    if (!userBtn || !dropdown) return;

    userBtn.addEventListener('click', function (e) {
      e.stopPropagation();
      var isOpen = dropdown.classList.contains('open');
      dropdown.classList.toggle('open', !isOpen);
      userBtn.setAttribute('aria-expanded', String(!isOpen));
    });

    document.addEventListener('click', function (e) {
      if (!dropdown.contains(e.target) && e.target !== userBtn) {
        dropdown.classList.remove('open');
        userBtn.setAttribute('aria-expanded', 'false');
      }
    });

    document.addEventListener('keydown', function (e) {
      if (e.key === 'Escape') {
        dropdown.classList.remove('open');
        userBtn.setAttribute('aria-expanded', 'false');
      }
    });
  }

  /* ═══════════════════════════════════════════════════════════
     INIT
     ═══════════════════════════════════════════════════════════ */
  document.addEventListener('DOMContentLoaded', function () {
    // Detect active module/page from URL
    var detected = detectActiveFromUrl();
    activeModule = detected.moduleId;
    activePage   = detected.pageId;

    // Mark the correct module tab active
    var tabs = document.querySelectorAll('.module-tab');
    tabs.forEach(function (t) {
      t.classList.remove('active');
      t.setAttribute('aria-selected', 'false');
      if (t.dataset.module === activeModule) {
        t.classList.add('active');
        t.setAttribute('aria-selected', 'true');
      }
    });

    renderSidebar(activeModule, activePage);
    initModuleTabs();
    initUserDropdown();
  });

})();
