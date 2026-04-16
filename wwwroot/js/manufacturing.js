/*!
 * manufacturing.js — ColorVision Paints ERP
 * Handles: Formulation % tracking · QC auto-pass/fail · Gantt chart · Slide panels (reuses ProcurementUI)
 */

/* ───────────────────────────────────────────────────────────
   NAMESPACE
   ─────────────────────────────────────────────────────────── */
window.MfgUI = (function () {
  'use strict';

  /* ── SLIDE PANEL ── */
  function openPanel(id) {
    document.querySelectorAll('.slide-panel').forEach(p => p.classList.remove('open'));
    const panel = document.getElementById(id);
    if (panel) {
      panel.classList.add('open');
      document.body.style.overflow = 'hidden';
    }
    document.querySelectorAll('.panel-overlay').forEach(o => o.classList.add('visible'));
  }

  function closeAllPanels() {
    document.querySelectorAll('.slide-panel').forEach(p => p.classList.remove('open'));
    document.body.style.overflow = '';
    document.querySelectorAll('.panel-overlay').forEach(o => o.classList.remove('visible'));
  }

  function initPanelClosers() {
    document.querySelectorAll('.close-panel-btn, .btn-cancel').forEach(btn => {
      btn.addEventListener('click', closeAllPanels);
    });
    document.querySelector('.panel-overlay')?.addEventListener('click', closeAllPanels);
  }

  /* ── FORMULATION INGREDIENT MANAGER ── */
  var batchSizeKg = 1000;
  var ingredientRows = [];

  function setBatchSize(kg) {
    batchSizeKg = parseFloat(kg) || 1000;
    refreshAllPct();
  }

  function addIngredientRow(materials) {
    const tbody = document.querySelector('#ingredient-tbody');
    if (!tbody) return;

    const idx = tbody.rows.length;
    const tr = document.createElement('tr');
    tr.dataset.idx = idx;
    tr.innerHTML = `
      <td>
        <select class="ing-material-sel" style="height:32px;font-size:12px;width:100%;border:1px solid rgba(0,0,0,0.09);border-radius:6px;background:rgba(255,255,255,0.6);padding:0 6px;font-family:inherit">
          <option value="">Select...</option>
          ${(materials||[]).map(m => `<option value="${m.Id}" data-unit="${m.Unit}" data-cost="${m.CostPerKg||0}">${m.Name}</option>`).join('')}
        </select>
      </td>
      <td><input type="number" class="ing-seq" value="${idx+1}" style="width:50px"></td>
      <td><input type="number" class="ing-qty" value="0" step="0.01" style="width:80px"></td>
      <td class="pct-cell ing-pct">0.0%</td>
      <td><input type="number" class="ing-tol-min" value="2" style="width:50px"></td>
      <td><input type="number" class="ing-tol-max" value="2" style="width:50px"></td>
      <td><span class="ing-unit" style="font-size:12px;color:var(--color-text-muted)">kg</span></td>
      <td><input type="text" class="ing-notes" placeholder="Optional" style="width:80px"></td>
      <td>
        <button type="button" class="remove-item-btn" onclick="this.closest('tr').remove(); MfgUI.updateIngredientPct()">
          <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><polyline points="3 6 5 6 21 6"/><path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a1 1 0 0 1 1-1h4a1 1 0 0 1 1 1v2"/></svg>
        </button>
      </td>
    `;
    tbody.appendChild(tr);

    tr.querySelector('.ing-qty').addEventListener('input', () => updateIngredientPct());
    tr.querySelector('.ing-material-sel').addEventListener('change', function () {
      const unit = this.selectedOptions[0]?.dataset.unit || 'kg';
      tr.querySelector('.ing-unit').textContent = unit;
    });

    updateIngredientPct();
  }

  function updateIngredientPct() {
    const rows = document.querySelectorAll('#ingredient-tbody tr');
    let total = 0;
    rows.forEach(tr => {
      const qty = parseFloat(tr.querySelector('.ing-qty')?.value) || 0;
      const pct = batchSizeKg > 0 ? (qty / batchSizeKg * 100) : 0;
      const pctCell = tr.querySelector('.ing-pct');
      if (pctCell) pctCell.textContent = pct.toFixed(2) + '%';
      total += pct;
    });

    const totalEl = document.getElementById('ing-pct-total');
    if (totalEl) {
      const rounded = Math.round(total * 100) / 100;
      totalEl.textContent = rounded.toFixed(2) + '%';
      totalEl.className = 'pct-total-row ' + (Math.abs(rounded - 100) < 0.1 ? 'pct-total-ok' : 'pct-total-bad');
      const submitBtn = document.getElementById('submit-formulation-btn');
      if (submitBtn) submitBtn.disabled = Math.abs(rounded - 100) >= 0.1;
    }
  }

  function refreshAllPct() {
    document.querySelectorAll('#ingredient-tbody tr .ing-qty').forEach(() => updateIngredientPct());
  }

  /* ── PROCESS STEP MANAGER ── */
  function addProcessStep() {
    const tbody = document.querySelector('#step-tbody');
    if (!tbody) return;
    const idx = tbody.rows.length + 1;
    const tr = document.createElement('tr');
    tr.innerHTML = `
      <td style="width:40px; text-align:center">
        <span class="drag-handle">⠿</span>
      </td>
      <td style="width:40px; text-align:center; font-weight:700; color:var(--color-accent)">${idx}</td>
      <td><input type="text" placeholder="Step title" class="step-title" style="width:100%"></td>
      <td><textarea placeholder="Instructions..." class="step-instruction" style="width:100%; height:48px; resize:vertical;"></textarea></td>
      <td><input type="number" placeholder="min" class="step-duration" style="width:60px"></td>
      <td><input type="text" placeholder="25–35°C" class="step-temp" style="width:80px"></td>
      <td><input type="text" placeholder="Equipment" class="step-equipment" style="width:90px"></td>
      <td>
        <button type="button" class="remove-item-btn" onclick="this.closest('tr').remove(); MfgUI.renumberSteps()">
          <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><polyline points="3 6 5 6 21 6"/><path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a1 1 0 0 1 1-1h4a1 1 0 0 1 1 1v2"/></svg>
        </button>
      </td>
    `;
    tbody.appendChild(tr);
  }

  function renumberSteps() {
    document.querySelectorAll('#step-tbody tr').forEach((tr, i) => {
      const numEl = tr.querySelectorAll('td')[1];
      if (numEl) numEl.textContent = i + 1;
    });
  }

  /* ── QC AUTO PASS/FAIL ── */
  function initQcTests() {
    document.querySelectorAll('.qc-actual-input').forEach(input => {
      input.addEventListener('input', () => evaluateQcRow(input));
    });
    evaluateAllQc();
  }

  function evaluateQcRow(input) {
    const row = input.closest('.qc-test-row');
    if (!row) return;
    const actual = parseFloat(input.value);
    const min = parseFloat(row.dataset.min);
    const max = parseFloat(row.dataset.max);
    const resultEl = row.querySelector('.qc-result-badge');

    if (!isNaN(actual) && !isNaN(min) && !isNaN(max)) {
      const pass = actual >= min && actual <= max;
      row.className = 'qc-test-row ' + (pass ? 'pass' : 'fail');
      if (resultEl) {
        resultEl.textContent = pass ? '✓ Pass' : '✗ Fail';
        resultEl.className = 'qc-result-badge ' + (pass ? 'pass' : 'fail');
      }
    }
    evaluateOverallQc();
  }

  function evaluateAllQc() {
    document.querySelectorAll('.qc-actual-input').forEach(i => evaluateQcRow(i));
  }

  function evaluateOverallQc() {
    const allRows = document.querySelectorAll('.qc-test-row');
    let allFilled = true;
    let allPass = true;

    allRows.forEach(row => {
      const input = row.querySelector('.qc-actual-input');
      if (!input || input.value === '') { allFilled = false; return; }
      if (!row.classList.contains('pass')) allPass = false;
    });

    const banner = document.getElementById('qc-overall-banner');
    if (banner) {
      if (!allFilled) {
        banner.className = 'qc-overall-banner';
        banner.innerHTML = '<span>Enter all test values to see overall result</span>';
      } else {
        banner.className = 'qc-overall-banner ' + (allPass ? 'pass' : 'fail');
        banner.innerHTML = allPass
          ? '<span>✓ OVERALL: PASSED — Ready to transfer to finished goods</span>'
          : '<span>✗ OVERALL: FAILED — Batch requires rework or disposal</span>';
      }
    }

    const submitQcBtn = document.getElementById('submit-qc-btn');
    if (submitQcBtn) submitQcBtn.disabled = !allFilled;
  }

  /* ── PO BATCH CALCULATOR ── */
  function calcBatches(qtyKg, batchSzKg) {
    const batches = Math.ceil(qtyKg / batchSzKg);
    const el = document.getElementById('po-batch-count');
    if (el) el.textContent = isFinite(batches) ? batches : '—';
    return batches;
  }

  /* ── GANTT CHART (pure CSS/DOM) ── */
  function renderGantt(lines, batches, containerEl, weekStart) {
    if (!containerEl) return;
    const days = 7;
    const dayMs = 86400000;
    const startMs = weekStart.getTime();

    let headerHtml = '<div class="gantt-header"><div style="width:160px;flex-shrink:0;padding:0 16px">Line</div>';
    for (let d = 0; d < days; d++) {
      const day = new Date(startMs + d * dayMs);
      headerHtml += `<div style="flex:1;text-align:center">${day.toLocaleDateString('en-IN',{weekday:'short',day:'numeric'})}</div>`;
    }
    headerHtml += '</div>';

    let rowsHtml = '';
    lines.forEach(line => {
      const lineBatches = batches.filter(b => b.productionLineId === line.Id);
      let barsHtml = '';
      lineBatches.forEach(b => {
        const bStart = new Date(b.startTime);
        const bEnd = new Date(b.endTime || Date.now() + dayMs);
        const offsetPct = Math.max(0, (bStart - startMs) / (days * dayMs)) * 100;
        const widthPct = Math.min(100, (bEnd - bStart) / (days * dayMs)) * 100;
        const cls = b.status === 'In-Progress' ? 'inprogress' : b.status.toLowerCase().replace('-','');
        barsHtml += `<div class="gantt-bar gantt-bar-${cls}" style="left:${offsetPct}%;width:${widthPct}%" title="${b.productName} · ${b.status}">${b.batchNumber}</div>`;
      });
      rowsHtml += `<div class="gantt-row"><div class="gantt-label">${line.Name}</div><div class="gantt-track">${barsHtml}</div></div>`;
    });

    containerEl.innerHTML = headerHtml + rowsHtml;
  }

  /* ── SPINNER BUTTON ── */
  function setLoading(btn, loading, defaultText) {
    if (loading) {
      btn.disabled = true;
      btn.innerHTML = '<span class="spinner"></span> Saving...';
    } else {
      btn.disabled = false;
      btn.textContent = defaultText;
    }
  }

  /* Public API */
  return {
    openPanel,
    closeAllPanels,
    initPanelClosers,
    addIngredientRow,
    updateIngredientPct,
    setBatchSize,
    addProcessStep,
    renumberSteps,
    initQcTests,
    evaluateQcRow,
    calcBatches,
    renderGantt,
    setLoading,
  };
})();
