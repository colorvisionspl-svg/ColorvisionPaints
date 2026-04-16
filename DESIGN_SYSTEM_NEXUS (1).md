# DESIGN_SYSTEM.md — Constro ERP (Nexus Style)
**Version:** 2.0  
**Theme:** Light Frosted Glass — Airy, Minimal, Professional  
**Last Updated:** April 2026  

> ⚠️ RULE FOR ALL AI TOOLS: Read this entire file before writing a single line of UI code.  
> Do NOT invent colors, fonts, spacing, or z-index values not listed here.  
> When in doubt — check this file first.

---

## 1. FONT — GILROY

```css
/* Import in index.html <head> or global CSS */
@import url('https://fonts.cdnfonts.com/css/gilroy-free');

/* Fallback stack */
font-family: 'Gilroy', 'Plus Jakarta Sans', 'DM Sans', sans-serif;
```

### CDN Alternative (if cdnfonts blocked):
```html
<link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@300;400;500;600;700&display=swap" rel="stylesheet">
/* Then use Plus Jakarta Sans as drop-in replacement */
```

### Font Weight Map
```
300 → Light     → Muted labels, hints, placeholders
400 → Regular   → Body text, nav items, table rows
500 → Medium    → Card sub-labels, secondary headings
600 → SemiBold  → Card headers, stat labels, nav active
700 → Bold      → Page titles, KPI numbers, logo text
800 → ExtraBold → Hero numbers (optional, big KPI display)
```

### Font Scale
```
--text-2xs:  10px  / lh 1.4  → Micro badges, tooltip labels
--text-xs:   11px  / lh 1.4  → Chart axis, table footnotes
--text-sm:   12px  / lh 1.5  → Secondary labels, meta text, stat sub-labels
--text-base: 13px  / lh 1.6  → Body text, nav items, table cell values
--text-md:   14px  / lh 1.5  → Card content, form labels, dropdown items
--text-lg:   16px  / lh 1.4  → Card section headers (e.g. "Revenue Trend")
--text-xl:   18px  / lh 1.3  → Sidebar section title (e.g. "Dashboard")
--text-2xl:  22px  / lh 1.2  → Page title (e.g. "Dashboard Overview")
--text-3xl:  28px  / lh 1.1  → KPI values (e.g. "$124,500.00")
--text-4xl:  36px  / lh 1.0  → Hero KPI, large metric display
```

---

## 2. COLOR PALETTE

### Base
```css
--color-bg-app:         #F0F2F8   /* Full app background — very light blue-gray */
--color-bg-topbar:      #FFFFFF   /* Top header bar — pure white */
--color-bg-subnav:      #FFFFFF   /* Module tab bar — pure white */
--color-bg-sidebar:     #FFFFFF   /* Left sidebar panel */
--color-bg-card:        #FFFFFF   /* Standard card surface */
--color-bg-input:       #FFFFFF   /* Input field background */
```

### Frosted / Glassmorphism Cards (for chart widgets)
```css
--color-bg-glass-blue:  rgba(220, 235, 255, 0.55)   /* Revenue Trend card tint */
--color-bg-glass-pink:  rgba(255, 220, 235, 0.45)   /* Sales by Region card tint */
--color-bg-glass-purple:rgba(230, 220, 255, 0.45)   /* Purple accent glass */
--color-glass-blur:     blur(12px)                  /* backdrop-filter value */
```

### Text
```css
--color-text-primary:   #1A1D2E   /* Main headings, values, logo */
--color-text-secondary: #4A5270   /* Body text, descriptions */
--color-text-muted:     #8B92A9   /* Labels, placeholders, hints */
--color-text-disabled:  #BCC0CE   /* Disabled state text */
```

### Accent — Blue (Primary)
```css
--color-accent:         #4F6EF7   /* Primary blue — active nav, links, icons */
--color-accent-light:   #EEF1FE   /* Blue tint background for active pill */
--color-accent-hover:   #3D5AE5   /* Hover state */
--color-accent-pressed: #2D48D0   /* Active/pressed state */
```

### Accent — Secondary Colors (for icons on KPI cards)
```css
--color-icon-blue:      #4F6EF7   /* Dollar/revenue icon */
--color-icon-indigo:    #7C6FF7   /* Cart/sales icon */
--color-icon-teal:      #2DC7A4   /* Inventory icon */
--color-icon-pink:      #F76F9A   /* Operations/alert icon */
--color-icon-orange:    #F7A24F   /* Warning states */
```

### Status
```css
--color-success:        #22C997   /* Positive trend, healthy status */
--color-success-bg:     rgba(34, 201, 151, 0.1)
--color-warning:        #F7A24F   /* Attention needed */
--color-warning-bg:     rgba(247, 162, 79, 0.1)
--color-danger:         #F76F6F   /* Errors, negative values */
--color-danger-bg:      rgba(247, 111, 111, 0.1)
--color-info:           #4F6EF7
--color-info-bg:        rgba(79, 110, 247, 0.08)
```

### Borders
```css
--color-border:         rgba(0, 0, 0, 0.06)   /* Default card/section border */
--color-border-medium:  rgba(0, 0, 0, 0.10)   /* Input, sidebar divider */
--color-border-strong:  rgba(0, 0, 0, 0.16)   /* Focus ring base */
--color-border-accent:  rgba(79, 110, 247, 0.4) /* Accent border on focus */
```

---

## 3. SPACING SYSTEM

Based on a **4px base grid**. Never use values outside this scale.

```css
--space-0:   0px
--space-1:   4px
--space-2:   8px
--space-3:   12px
--space-4:   16px
--space-5:   20px
--space-6:   24px
--space-7:   28px
--space-8:   32px
--space-10:  40px
--space-12:  48px
--space-16:  64px
--space-20:  80px
```

---

## 4. BORDER RADIUS

```css
--radius-xs:   4px    /* Tags, tiny badges */
--radius-sm:   6px    /* Buttons, small inputs */
--radius-md:   10px   /* Module nav pills, inputs */
--radius-lg:   14px   /* Cards, sidebar panels */
--radius-xl:   18px   /* Large chart cards, modals */
--radius-2xl:  24px   /* App container outer rounding */
--radius-pill: 999px  /* Active nav pill, toggles, avatar */
```

---

## 5. SHADOWS

```css
--shadow-none:    none
--shadow-xs:      0 1px 2px rgba(0,0,0,0.04)
--shadow-sm:      0 1px 4px rgba(0,0,0,0.06), 0 2px 8px rgba(0,0,0,0.04)
--shadow-md:      0 2px 8px rgba(0,0,0,0.06), 0 4px 20px rgba(0,0,0,0.06)
--shadow-lg:      0 4px 24px rgba(0,0,0,0.08), 0 8px 40px rgba(0,0,0,0.06)
--shadow-topbar:  0 1px 0 rgba(0,0,0,0.06)    /* Bottom border-like shadow on header */
--shadow-card:    var(--shadow-sm)
--shadow-modal:   var(--shadow-lg)
--shadow-dropdown:0 4px 16px rgba(0,0,0,0.12)
```

No colored shadows. No glow effects. All shadows are neutral black, very low opacity.

---

## 6. LAYOUT STRUCTURE

### Full App Layout
```
┌────────────────────────────────────────────────────┐
│  TOPBAR (fixed, 60px height)                       │
├────────────────────────────────────────────────────┤
│  MODULE SUBNAV (fixed, 52px height, below topbar)  │
├──────────────┬─────────────────────────────────────┤
│              │                                     │
│   SIDEBAR    │        MAIN CONTENT AREA            │
│   (280px)    │        (flex: 1, scrollable)        │
│   fixed      │                                     │
│              │                                     │
└──────────────┴─────────────────────────────────────┘
```

### Topbar (Row 1)
```css
height:       60px
background:   var(--color-bg-topbar)
border-bottom:1px solid var(--color-border)
position:     fixed
top:          0
left:         0
right:        0
z-index:      var(--z-topbar)
padding:      0 24px
display:      flex
align-items:  center

/* Layout zones */
Left zone:    Logo (36px icon + "Constro ERP" text), flex-shrink: 0
Center zone:  Search bar (max-width 400px, flex: 1, mx: auto)
Right zone:   Company selector + Bell + Settings + Avatar pill
```

### Module Sub-Navigation (Row 2)
```css
height:       52px
background:   var(--color-bg-subnav)
border-bottom:1px solid var(--color-border)
position:     fixed
top:          60px
left:         0
right:        0
z-index:      var(--z-subnav)
padding:      0 24px
display:      flex
align-items:  center
gap:          4px
overflow-x:   auto   /* horizontal scroll on small screens */
scrollbar:    hidden
```

Active module tab:
```css
background:   var(--color-accent-light)
color:        var(--color-accent)
font-weight:  600
border-radius:var(--radius-md)
padding:      6px 14px
icon-color:   var(--color-accent)
```

Inactive module tab:
```css
background:   transparent
color:        var(--color-text-secondary)
font-weight:  400
padding:      6px 14px
hover-bg:     rgba(0,0,0,0.04)
```

### Sidebar
```css
width:        280px
background:   var(--color-bg-sidebar)
border-right: 1px solid var(--color-border)
position:     fixed
top:          112px   /* 60 topbar + 52 subnav */
left:         0
bottom:       0
z-index:      var(--z-sidebar)
padding:      20px 16px
overflow-y:   auto
```

Sidebar section title:
```css
font-size:    var(--text-xl)   /* 18px */
font-weight:  700
color:        var(--color-text-primary)
margin-bottom:16px
```

Sidebar nav item:
```css
display:      flex
align-items:  center
gap:          10px
padding:      10px 12px
border-radius:var(--radius-md)
font-size:    var(--text-base)   /* 13px */
font-weight:  400
color:        var(--color-text-secondary)
cursor:       pointer
transition:   background 0.15s ease, color 0.15s ease
```

Sidebar nav item — active:
```css
background:   var(--color-accent-light)
color:        var(--color-accent)
font-weight:  600
icon:         var(--color-accent)
/* Right: chevron > icon */
```

Sidebar nav item — hover:
```css
background:   rgba(0,0,0,0.04)
color:        var(--color-text-primary)
```

### Main Content Area
```css
margin-left:  280px
margin-top:   112px   /* 60 + 52 */
padding:      24px 28px
min-height:   calc(100vh - 112px)
background:   var(--color-bg-app)
```

---

## 7. COMPONENT SPECIFICATIONS

### Search Bar (Topbar)
```css
background:       rgba(0,0,0,0.04)
border:           1px solid transparent
border-radius:    var(--radius-md)
height:           38px
padding:          0 14px 0 36px   /* left pad for icon */
font-size:        var(--text-base)
color:            var(--color-text-primary)
placeholder-color:var(--color-text-muted)
max-width:        400px

focus:
  background:     #FFFFFF
  border-color:   var(--color-border-accent)
  box-shadow:     0 0 0 3px rgba(79,110,247,0.12)
```

### KPI Stat Card
```css
background:       #FFFFFF
border:           1px solid var(--color-border)
border-radius:    var(--radius-lg)   /* 14px */
padding:          20px 22px
shadow:           var(--shadow-sm)

/* Inner layout */
Top row:          label (left) + icon (right, 24px, colored per metric)
Label:            font-size 12px, font-weight 500, color: var(--color-text-muted)
Value:            font-size 28px, font-weight 700, color: var(--color-text-primary), margin-top 6px
Sub-label:        font-size 12px, font-weight 400, color: var(--color-success), margin-top 4px
```

Icon per KPI card:
```
Revenue card:     $ icon, color: --color-icon-blue    (#4F6EF7)
Sales Volume:     cart icon, color: --color-icon-indigo (#7C6FF7)
Inventory Level:  box icon, color: --color-icon-teal   (#2DC7A4)
Active Operations:activity icon, color: --color-icon-pink (#F76F9A)
```

KPI card grid:
```css
display:              grid
grid-template-columns:repeat(4, 1fr)
gap:                  16px
margin-bottom:        20px
```

### Chart Card (Frosted Glass style)
```css
border-radius:    var(--radius-xl)   /* 18px */
padding:          22px 24px
border:           1px solid rgba(255,255,255,0.6)

/* Revenue Trend card */
background:       linear-gradient(135deg, rgba(220,235,255,0.7) 0%, rgba(240,242,255,0.5) 100%)
backdrop-filter:  blur(12px)

/* Sales by Region card */
background:       linear-gradient(135deg, rgba(255,220,235,0.6) 0%, rgba(255,235,245,0.5) 100%)
backdrop-filter:  blur(12px)

/* Chart card header */
Title:            font-size 16px, font-weight 600, color: var(--color-text-primary)
Date filter pill: top-right, ghost button style
```

Chart card grid:
```css
display:              grid
grid-template-columns:1.6fr 1fr
gap:                  20px
```

Chart bars (inside glass cards):
```
Bar color:        rgba(255,255,255,0.7) with white/glass fill
Bar radius:       6px top corners
No axis lines     — bars float in gradient background
Height:           ~220px chart area
```

### Company Selector (Topbar right)
```css
background:       rgba(0,0,0,0.04)
border:           1px solid var(--color-border)
border-radius:    var(--radius-md)
padding:          6px 12px
font-size:        var(--text-base)
font-weight:      500
color:            var(--color-text-primary)
gap:              6px   /* text + chevron */
hover:            background rgba(0,0,0,0.07)
```

### Avatar + User Info (Topbar)
```css
/* Initials circle */
width:            34px
height:           34px
border-radius:    var(--radius-pill)
background:       var(--color-accent-light)
color:            var(--color-accent)
font-size:        12px
font-weight:      700

/* Welcome text */
"Welcome back,"  → font-size 11px, color: var(--color-text-muted), font-weight 400
Name + chevron   → font-size 13px, color: var(--color-text-primary), font-weight 600
```

### Primary Button
```css
background:       var(--color-accent)
color:            #FFFFFF
font-size:        var(--text-base)
font-weight:      600
height:           38px
padding:          0 18px
border-radius:    var(--radius-sm)
border:           none
letter-spacing:   0.01em
transition:       background 0.15s ease, transform 0.1s ease

hover:    background var(--color-accent-hover), transform translateY(-1px)
active:   background var(--color-accent-pressed), transform scale(0.98)
```

### Ghost / Outline Button
```css
background:       transparent
color:            var(--color-text-secondary)
border:           1px solid var(--color-border-medium)
height:           34px
padding:          0 14px
border-radius:    var(--radius-sm)
font-size:        var(--text-sm)
font-weight:      500

hover:    background rgba(0,0,0,0.04), color: var(--color-text-primary)
```

### Date Range Pill (e.g. "Last 30 Days")
```css
background:       #FFFFFF
border:           1px solid var(--color-border-medium)
border-radius:    var(--radius-sm)
padding:          7px 14px
font-size:        var(--text-sm)
font-weight:      500
color:            var(--color-text-secondary)
```

### Icon Button (Bell, Settings)
```css
width:            36px
height:           36px
border-radius:    var(--radius-md)
background:       transparent
icon-size:        18px
icon-color:       var(--color-text-muted)
border:           none

hover:  background rgba(0,0,0,0.05), icon-color: var(--color-text-primary)
```

### Filter / Sort Button (Sidebar header right)
```css
width:            30px
height:           30px
border-radius:    var(--radius-sm)
background:       transparent
icon-size:        16px
icon-color:       var(--color-text-muted)
```

### Input Field (Forms)
```css
background:       var(--color-bg-input)
border:           1px solid var(--color-border-medium)
border-radius:    var(--radius-sm)
height:           40px
padding:          0 12px
font-family:      Gilroy, sans-serif
font-size:        var(--text-base)
color:            var(--color-text-primary)
placeholder:      var(--color-text-muted)

focus:
  border-color:   var(--color-border-accent)
  box-shadow:     0 0 0 3px rgba(79,110,247,0.10)
  outline:        none
```

### Dropdown Menu
```css
background:       #FFFFFF
border:           1px solid var(--color-border)
border-radius:    var(--radius-lg)
shadow:           var(--shadow-dropdown)
padding:          6px
min-width:        180px

item:
  padding:        8px 12px
  border-radius:  var(--radius-sm)
  font-size:      var(--text-base)
  color:          var(--color-text-secondary)
  hover-bg:       rgba(0,0,0,0.04)
  active-bg:      var(--color-accent-light)
  active-color:   var(--color-accent)
```

### Modal / Slide Panel
```css
overlay-bg:       rgba(15, 20, 50, 0.3)
overlay-z:        var(--z-overlay)
modal-z:          var(--z-modal)

/* Centered modal */
background:       #FFFFFF
border-radius:    var(--radius-xl)
shadow:           var(--shadow-modal)
max-width:        560px
padding:          0

/* Right slide panel */
width:            480px
height:           100vh
position:         fixed
right:            0
top:              0
border-radius:    var(--radius-xl) 0 0 var(--radius-xl)
```

---

## 8. ICON SYSTEM

### Library
```
Primary:  Lucide Icons
Install:  npm install lucide-react
CDN:      https://unpkg.com/lucide@latest/dist/umd/lucide.min.js

Iconify API (no-install, for Antigravity):
  https://api.iconify.design/lucide/{name}.svg?color=%234F6EF7&width=18
```

### Icon Sizes
```
--icon-xs:   12px   → inline text icons, tiny badges
--icon-sm:   14px   → table row icons
--icon-base: 16px   → sidebar nav, card toolbar
--icon-md:   18px   → topbar icons, button icons
--icon-lg:   20px   → section icons, module tab icons
--icon-xl:   24px   → KPI card metric icons
--icon-2xl:  32px   → empty states, hero icons
```

### Icon Colors
```
On white bg, default:     var(--color-text-muted)     → #8B92A9
On white bg, active:      var(--color-accent)          → #4F6EF7
On white bg, hover:       var(--color-text-primary)    → #1A1D2E
On glass card, default:   rgba(255,255,255,0.8)
On glass card, active:    #FFFFFF
KPI icon colors:          see KPI Stat Card section above
```

### Module Tab Icons
```
Dashboard:    grid or layout-dashboard icon
Receivables:  credit-card or inbox icon
Returns:      refresh-ccw icon
Customers:    users icon
Payables:     file-text icon
Sales:        trending-up icon
Inventory:    package icon
Factory:      factory icon
Imports:      download-cloud icon
Tickets:      ticket icon
```

---

## 9. Z-INDEX STACK

```css
--z-base:      1
--z-card:      10
--z-dropdown:  100
--z-subnav:    150
--z-topbar:    200
--z-sidebar:   300
--z-overlay:   800
--z-modal:     900
--z-toast:     1000
```

RULE: Never use arbitrary values. Never use 9999.

---

## 10. ANIMATION & TRANSITIONS

```css
--ease-fast:    0.12s ease
--ease-base:    0.2s ease
--ease-slow:    0.3s ease
--ease-spring:  0.35s cubic-bezier(0.34, 1.56, 0.64, 1)
```

Usage:
```
Button hover/active:    var(--ease-fast)
Nav item highlight:     var(--ease-fast)
Modal open:             var(--ease-slow), translateY(16px) → translateY(0), opacity 0→1
Sidebar slide:          var(--ease-slow), translateX(-280px) → translateX(0)
Card hover lift:        var(--ease-base), translateY(-2px)
Dropdown appear:        var(--ease-base), scale(0.97) → scale(1), opacity 0→1
```

Always wrap in:
```css
@media (prefers-reduced-motion: no-preference) {
  /* animations here */
}
```

---

## 11. CHART STYLING

Library: **Recharts** (React) or **Chart.js** (vanilla)

```
Bar chart bars:
  Revenue bars:     rgba(255,255,255,0.75) — frosted white on blue glass
  Sales bars:       rgba(255,255,255,0.65) — frosted white on pink glass
  Border-radius:    6px (top corners only)
  Gap between bars: 4px

No grid lines visible
No axis labels (minimal style) — or very faint: rgba(0,0,0,0.2)
Tooltip:
  background:       #FFFFFF
  border:           1px solid var(--color-border)
  border-radius:    var(--radius-md)
  shadow:           var(--shadow-dropdown)
  font:             Gilroy, 12px
```

---

## 12. RESPONSIVE BREAKPOINTS

```css
--bp-sm:  480px   /* Mobile portrait */
--bp-md:  768px   /* Tablet */
--bp-lg:  1024px  /* Laptop — minimum for ERP dashboard */
--bp-xl:  1280px  /* Desktop */
--bp-2xl: 1440px  /* Wide screen */
```

Below 1024px:
- Sidebar collapses to off-canvas (hamburger toggle)
- Module subnav scrolls horizontally
- KPI grid → 2 columns
- Chart grid → single column

Below 768px:
- KPI grid → 1 column
- Topbar → compact (logo + hamburger only)

---

## 13. CSS VARIABLES — MASTER BLOCK

Paste this into your global `variables.css` or `:root` block:

```css
:root {
  /* Fonts */
  --font-primary: 'Gilroy', 'Plus Jakarta Sans', 'DM Sans', sans-serif;

  /* Colors */
  --color-bg-app:           #F0F2F8;
  --color-bg-topbar:        #FFFFFF;
  --color-bg-subnav:        #FFFFFF;
  --color-bg-sidebar:       #FFFFFF;
  --color-bg-card:          #FFFFFF;
  --color-bg-glass-blue:    rgba(220, 235, 255, 0.55);
  --color-bg-glass-pink:    rgba(255, 220, 235, 0.45);
  --color-text-primary:     #1A1D2E;
  --color-text-secondary:   #4A5270;
  --color-text-muted:       #8B92A9;
  --color-text-disabled:    #BCC0CE;
  --color-accent:           #4F6EF7;
  --color-accent-light:     #EEF1FE;
  --color-accent-hover:     #3D5AE5;
  --color-accent-pressed:   #2D48D0;
  --color-icon-blue:        #4F6EF7;
  --color-icon-indigo:      #7C6FF7;
  --color-icon-teal:        #2DC7A4;
  --color-icon-pink:        #F76F9A;
  --color-success:          #22C997;
  --color-success-bg:       rgba(34, 201, 151, 0.10);
  --color-warning:          #F7A24F;
  --color-danger:           #F76F6F;
  --color-border:           rgba(0, 0, 0, 0.06);
  --color-border-medium:    rgba(0, 0, 0, 0.10);
  --color-border-accent:    rgba(79, 110, 247, 0.40);

  /* Spacing */
  --space-1: 4px;   --space-2: 8px;   --space-3: 12px;
  --space-4: 16px;  --space-5: 20px;  --space-6: 24px;
  --space-8: 32px;  --space-10: 40px; --space-12: 48px;

  /* Radius */
  --radius-xs:   4px;   --radius-sm:  6px;   --radius-md:  10px;
  --radius-lg:   14px;  --radius-xl:  18px;  --radius-2xl: 24px;
  --radius-pill: 999px;

  /* Shadows */
  --shadow-xs:       0 1px 2px rgba(0,0,0,0.04);
  --shadow-sm:       0 1px 4px rgba(0,0,0,0.06), 0 2px 8px rgba(0,0,0,0.04);
  --shadow-md:       0 2px 8px rgba(0,0,0,0.06), 0 4px 20px rgba(0,0,0,0.06);
  --shadow-lg:       0 4px 24px rgba(0,0,0,0.08), 0 8px 40px rgba(0,0,0,0.06);
  --shadow-dropdown: 0 4px 16px rgba(0,0,0,0.12);

  /* Z-Index */
  --z-base:     1;   --z-card:    10;  --z-dropdown: 100;
  --z-subnav:   150; --z-topbar:  200; --z-sidebar:  300;
  --z-overlay:  800; --z-modal:   900; --z-toast:    1000;

  /* Transitions */
  --ease-fast:   0.12s ease;
  --ease-base:   0.2s ease;
  --ease-slow:   0.3s ease;
  --ease-spring: 0.35s cubic-bezier(0.34, 1.56, 0.64, 1);

  /* Layout */
  --topbar-height:   60px;
  --subnav-height:   52px;
  --sidebar-width:   280px;
  --content-offset:  112px; /* topbar + subnav */
}
```

---

## 14. AI PROMPT TEMPLATE

Copy-paste this at the START of every Cursor / Antigravity / Claude Code prompt:

```
You are building Constro ERP — a construction management system.
Strictly follow DESIGN_SYSTEM.md (Nexus style v2.0).

Key rules:
- Font: Gilroy (fallback: Plus Jakarta Sans)
- Body background: #F0F2F8 — NOT white, NOT gray
- Cards: white (#FFFFFF) with 1px rgba(0,0,0,0.06) border, 14px radius, soft shadow
- Chart widgets: frosted glass cards with gradient background + backdrop-filter blur
- Primary accent: #4F6EF7 (blue) — for active states, links, primary buttons
- Active nav: background #EEF1FE, color #4F6EF7, font-weight 600
- All overlays/modals: position fixed, z-index from defined stack only
- Icons: Lucide icons, 16–20px size
- No inline hardcoded colors — use CSS variables only
- No z-index values not from the stack
- No shadows with color tints

[Then describe the specific component or page]
```

---

## 15. DO NOT LIST

- ❌ `font-family: Arial / Roboto / Inter` — use Gilroy only
- ❌ White `#FFFFFF` as page body background — use `#F0F2F8`
- ❌ `position: absolute` for modals, dropdowns, panels — always `position: fixed`
- ❌ Arbitrary z-index (9999, 100000, etc.)
- ❌ Hardcoded hex colors in component code — CSS variables only
- ❌ Gradients on sidebar, topbar, or cards (only on glass chart widgets)
- ❌ Colored shadows or glow effects
- ❌ Font sizes below 10px
- ❌ Font weight 800+ in body text — only for hero KPI numbers
- ❌ More than 3 font weights visible on any single card
- ❌ Blue used as body background or large fill — blue is accent only
- ❌ Borders thicker than 1px on cards

---

## 16. FORM LAYOUT DISCIPLINE

All forms, row-entry tables, and data-entry sections must follow a strict aligned grid. No field is allowed to float loosely or size itself inconsistently in a way that breaks straight visual alignment.

### Form Structure Rules

- Every form must use a fixed column grid or fixed-width table layout.
- All labels, inputs, selects, date fields, toggles, upload fields, and action buttons must align in one clean straight line.
- Each column must keep a consistent width across all rows.
- Row height must remain visually consistent across the full form.
- Inputs must stretch to fill their assigned column width, not shrink based on content.
- Headers and input rows must align exactly column-to-column.
- Save / Edit / Action buttons must align with the grid and not appear floating or uneven.

### Row Entry Rules

- For horizontal row-based forms, use a strict table-like structure.
- Recommended implementation:
  - `table-layout: fixed;`
  - or CSS Grid with predefined column widths
- Do not use uneven auto-width fields that cause broken alignment.
- Do not let long text expand one column and disturb the full row structure.
- Remarks fields must stay within their assigned width.
- Status toggles, date fields, supplier fields, and unit fields must remain vertically centered and visually level with adjacent columns.

### Behavior Rules

- Adding a new row must preserve the exact same column widths and alignment as the first row.
- Validation messages must not push fields out of alignment; show them below the row or in a controlled helper area.
- Action buttons must remain anchored consistently below or to the right as defined by the layout.
- Forms must look structured, corporate, and engineering-grade — not loose, floating, or uneven.

### Strict Prohibitions

- ❌ No random field widths
- ❌ No uneven spacing between columns
- ❌ No auto-expanding rows that break alignment
- ❌ No mismatched input heights
- ❌ No floating buttons detached from the form grid
- ❌ No loose flex-wrap form rows for structured ERP data entry

### Final Rule

If a form is row-based, it must always render in a clean, fixed, line-by-line, column-aligned structure with consistent widths and spacing across every row.

---

## 17. GLOBAL SCREEN FLOW & FORM OPENING SYSTEM

This is a global application rule and applies to every module, sub-module, screen, and future feature in the ERP unless explicitly overridden by a written instruction.

### Global Screen Flow Rule

Every screen must follow this structure:

1. Main interface loads first
2. Main interface shows only structured content such as:
   - page title
   - summary cards
   - filters
   - tabs
   - list/table/grid
   - action buttons
3. Any create, edit, update, detail-entry, dispatch, or transaction form must open only as a separate controlled layer

### Allowed Form Opening Patterns

Forms are allowed to open only in one of these patterns:

- Modal popup
- Right-side slide panel
- Centered dialog
- Bottom sheet (only if suitable to the approved design system)

### Default Rule

Unless explicitly instructed otherwise:

- forms must NOT open inline inside the main interface
- forms must NOT appear mixed between tables, widgets, or dashboard sections
- forms must NOT permanently occupy the main listing area by default

### Separation Principle

The main interface and the form interface must always remain visually and structurally separate.

That means:

- main screen stays clean
- action begins from button click
- form opens in popup/panel/dialog
- save/update happens inside that controlled form layer
- after success, the form closes and the main interface refreshes cleanly

### Edit and View Behavior

This rule applies to all of the following:

- Add New
- Edit
- Update
- View Details
- Create Entry
- New Transaction
- Dispatch
- Packaging
- Production Entry
- Any future module form

By default, all such actions must open in a popup/modal/side panel and not as mixed inline content.

### UI Compliance Rule

Every popup, dialog, modal, and side panel must strictly follow the existing design system:

- same typography
- same spacing discipline
- same border radius rules
- same z-index rules
- same button styles
- same overlay behavior
- same visual language as the main ERP

No popup or form layer may introduce a separate design style.

### Layout Protection Rule

Forms must never break or disturb the main page structure.

Opening a form must not:

- push dashboard cards out of place
- disturb table alignment
- insert long blocks into the main page flow
- change the height/position of unrelated interface sections unexpectedly

### Interaction Sequence Rule

The correct ERP interaction sequence is always:

Main Interface → Action Button → Controlled Form Layer → Save/Update → Close Form → Refresh Main Interface

### Strict Prohibitions

- ❌ No inline full-page forms by default
- ❌ No mixing list view and full entry form in one cluttered screen
- ❌ No random expansion panels that break page structure
- ❌ No form opening style that ignores modal/panel/dialog rules
- ❌ No redesign while opening forms
- ❌ No deviation from the approved design system

### Final Global Rule

For all current and future ERP modules, the system must always open the main interface first and open all create/edit/detail forms only through a controlled popup, modal, dialog, or side panel, while fully following the design system and preserving a clean main screen layout.

---

*End of DESIGN_SYSTEM.md — Constro ERP Nexus Style v2.0*
*Next step: Build skeleton shell (Topbar + Subnav + Sidebar + Content area) using this system.*
