/*!
 * rewards-admin.js — QR Rewards ERP admin helpers
 */
window.RewardsUI = (function () {
  'use strict';

  /* ── panel helpers (delegates to MfgUI if loaded) ── */
  function openPanel(id) {
    document.querySelectorAll('.slide-panel').forEach(p => p.classList.remove('open'));
    document.querySelectorAll('.panel-overlay').forEach(o => o.classList.add('visible'));
    const p = document.getElementById(id);
    if (p) { p.classList.add('open'); document.body.style.overflow = 'hidden'; }
  }

  function closeAll() {
    document.querySelectorAll('.slide-panel').forEach(p => p.classList.remove('open'));
    document.querySelectorAll('.panel-overlay').forEach(o => o.classList.remove('visible'));
    document.body.style.overflow = '';
  }

  function initClosers() {
    document.querySelectorAll('.close-panel-btn, .btn-cancel').forEach(b => b.addEventListener('click', closeAll));
    document.querySelector('.panel-overlay')?.addEventListener('click', closeAll);
  }

  /* ── POINT VALUE AUTO-FILL by category ── */
  const CATEGORY_POINTS = {
    'economy':    5,
    'interior':   15,
    'exterior':   25,
    'waterproof': 40,
    'primer':     10,
  };

  function onCategoryChange(sel) {
    const val = CATEGORY_POINTS[sel.value] ?? 15;
    const input = document.getElementById('qr-point-value');
    if (input) input.value = val;
  }

  /* ── TABLE FILTERS ── */
  function filterTable(tableId, val) {
    const q = val.toLowerCase();
    document.querySelectorAll(`#${tableId} tbody tr`).forEach(tr => {
      tr.style.display = tr.textContent.toLowerCase().includes(q) ? '' : 'none';
    });
  }

  /* ── BULK SELECT ── */
  function toggleSelectAll(masterId, checkClass) {
    const master = document.getElementById(masterId);
    document.querySelectorAll('.' + checkClass).forEach(cb => { cb.checked = master?.checked; });
    updateBulkBar();
  }

  function updateBulkBar() {
    const selected = document.querySelectorAll('.row-check:checked').length;
    const bar = document.getElementById('bulk-action-bar');
    const cnt = document.getElementById('selected-count');
    if (bar) bar.style.display = selected > 0 ? 'flex' : 'none';
    if (cnt) cnt.textContent = selected;
  }

  /* ── TIER UTIL ── */
  function tierFromPoints(pts) {
    if (pts >= 35000) return 'Platinum';
    if (pts >= 15000) return 'Gold';
    if (pts >= 5000)  return 'Silver';
    return 'Bronze';
  }

  function tierProgress(pts) {
    if (pts >= 35000) return 100;
    if (pts >= 15000) return Math.round((pts - 15000) / 20000 * 100);
    if (pts >= 5000)  return Math.round((pts - 5000)  / 10000 * 100);
    return Math.round(pts / 5000 * 100);
  }

  function tierNext(pts) {
    if (pts >= 35000) return '— Platinum ✓';
    if (pts >= 15000) return `${(35000 - pts).toLocaleString()} pts to Platinum`;
    if (pts >= 5000)  return `${(15000 - pts).toLocaleString()} pts to Gold`;
    return `${(5000 - pts).toLocaleString()} pts to Silver`;
  }

  /* ── QR CODE GENERATION (client-side serial logic) ── */
  function generateQrDataUrl(serial) {
    const url = `${location.origin}/scan/${serial}`;
    // QR rendering done per-page with CDN library
    return url;
  }

  return { openPanel, closeAll, initClosers, onCategoryChange, filterTable, toggleSelectAll, updateBulkBar, tierFromPoints, tierProgress, tierNext, generateQrDataUrl };
})();
