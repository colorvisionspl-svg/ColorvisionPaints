/**
 * procurement.js — ColorVision ERP
 * Shared logic for forms, slide panels, and real-time calculations.
 */

const ProcurementUI = {
  
  init() {
    this.bindGlobalEvents();
  },

  bindGlobalEvents() {
    // Close panel on backdrop click or ESC
    document.addEventListener('keydown', (e) => {
      if (e.key === 'Escape') this.closeAllPanels();
    });

    // Close buttons
    document.querySelectorAll('.close-panel-btn, .btn-cancel').forEach(btn => {
      btn.addEventListener('click', () => this.closeAllPanels());
    });
  },

  openPanel(panelId) {
    const panel = document.getElementById(panelId);
    if (panel) {
      panel.classList.add('open');
      document.body.style.overflow = 'hidden'; // Prevent background scroll
    }
  },

  closeAllPanels() {
    document.querySelectorAll('.slide-panel').forEach(p => p.classList.remove('open'));
    document.body.style.overflow = '';
  },

  /* ── Calculations ── */
  calculateLineTotal(qty, rate, taxPct) {
    qty = parseFloat(qty) || 0;
    rate = parseFloat(rate) || 0;
    taxPct = parseFloat(taxPct) || 0;
    const subtotal = qty * rate;
    const taxAmount = subtotal * (taxPct / 100);
    return subtotal + taxAmount;
  },

  updateTotals(tableId, subtotalId, taxId, grandId) {
    let subtotal = 0;
    let taxTotal = 0;

    const rows = document.querySelectorAll(`#${tableId} tbody tr`);
    rows.forEach(row => {
      const qty = parseFloat(row.querySelector('.qty-input')?.value) || 0;
      const rate = parseFloat(row.querySelector('.rate-input')?.value) || 0;
      const taxPct = parseFloat(row.querySelector('.tax-input')?.value) || 0;
      
      const lineSub = qty * rate;
      const lineTax = lineSub * (taxPct / 100);
      
      subtotal += lineSub;
      taxTotal += lineTax;

      const lineTotalEl = row.querySelector('.line-total');
      if (lineTotalEl) {
        lineTotalEl.textContent = `₹${(lineSub + lineTax).toLocaleString('en-IN')}`;
      }
    });

    if (subtotalId) document.getElementById(subtotalId).textContent = `₹${subtotal.toLocaleString('en-IN')}`;
    if (taxId) document.getElementById(taxId).textContent = `₹${taxTotal.toLocaleString('en-IN')}`;
    if (grandId) document.getElementById(grandId).textContent = `₹${(subtotal + taxTotal).toLocaleString('en-IN')}`;
  },

  /* ── Vendors Rating ── */
  initStarPicker(containerId, inputId) {
    const container = document.getElementById(containerId);
    if (!container) return;

    const stars = container.querySelectorAll('.star');
    const input = document.getElementById(inputId);

    stars.forEach(star => {
      star.addEventListener('click', () => {
        const val = parseInt(star.dataset.value);
        input.value = val;
        this.updateStars(container, val);
      });
    });
  },

  updateStars(container, val) {
    container.querySelectorAll('.star').forEach(star => {
      const sVal = parseInt(star.dataset.value);
      star.classList.toggle('filled', sVal <= val);
    });
  }
};

// Auto-init
document.addEventListener('DOMContentLoaded', () => ProcurementUI.init());
