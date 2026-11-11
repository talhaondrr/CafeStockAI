"use strict";

function selectedField() {
  return draft?.area === "Bar" ? "bar" : "depot";
}
function completed() {
  return draft
    ? draft.products.filter((p) =>
        validInput(draft.values[p.id]?.[selectedField()]),
      ).length
    : 0;
}
function validInput(v) {
  return v !== undefined && v !== "" && /^\d+(?:[.,]\d{1,6})?$/.test(String(v));
}
function parsed(v) {
  if (!validInput(v)) throw Error("Miktar sıfır veya pozitif bir sayı olmalı.");
  return Number(String(v).replace(",", "."));
}

function countPage() {
  if (!draft)
    return (
      header(
        "Nereyi sayıyoruz?",
        "Önce alanını seç. Bar ve Depo ayrı sayılır, ayrı onaylanır.",
      ) +
      '<div class="area-options">' +
      ["Bar", "Depo"]
        .map(
          (area) =>
            '<button class="area-option" data-start-area="' +
            area +
            '"><span class="badge-icon">' +
            (area === "Bar" ? "☕" : "▦") +
            "</span><span><strong>" +
            area +
            "</strong><small>" +
            (area === "Bar"
              ? "Servis alanındaki ürünler"
              : "Depoda bulunan ürünler") +
            "</small><em>" +
            (drafts[area]
              ? "Kaydedilmiş taslağa devam et"
              : "Yeni " + area.toLocaleLowerCase("tr") + " sayımı") +
            "</em></span><b>→</b></button>",
        )
        .join("") +
      "</div>" +
      '<div class="notice">Bir alandaki sayım diğer alanın miktarlarını değiştirmez. Girdiğin miktarlar bu cihazda alanına göre saklanır.</div><div class="section-title"><h3>Sayım geçmişin</h3></div>' +
      data.counts.map(countCard).join("")
    );
  const rows = countOrder(),
    p = currentCountProduct(),
    cats = [...new Set(rows.map((p) => p.category))];
  if (!p) return header("Sayım", "Sayılacak ürün bulunamadı.");
  return (
    '<section class="count-wizard">' +
    header(
      draft.area + " sayımı",
      "Her ekranda tek ürün.",
      '<button id="switchCountArea" class="secondary">Alanı değiştir</button>',
    ) +
    '<div class="draft-head"><div class="row"><strong>Elazığ • ' +
    draft.area +
    '</strong><span class="small" id="draftProgress">' +
    completed() +
    " / " +
    rows.length +
    ' tamamlandı</span></div><div class="progress"><div id="progressFill"></div></div></div>' +
    '<label class="count-category-label">Kategori<select id="countCategory">' +
    cats
      .map(
        (c) =>
          '<option value="' +
          esc(c) +
          '" ' +
          (c === p.category ? "selected" : "") +
          ">" +
          esc(c) +
          "</option>",
      )
      .join("") +
    "</select></label>" +
    '<div class="count-step"><h2 id="stepCategory"></h2><span id="stepPosition" class="small"></span></div>' +
    '<article class="product-card count-one"><div id="stepHeading"></div><label class="count-quantity-label">' +
    draft.area +
    ' miktarı<input id="stepQuantity" inputmode="decimal" enterkeyhint="next" autocomplete="off" placeholder="Miktar gir" aria-label="Sayım miktarı"></label><p id="stepError" class="error" role="alert"></p><div class="count-step-actions"><button id="previousCount" class="secondary">← Geri</button><button id="nextCount" class="primary">Gir / Sonraki →</button></div><p class="small count-hint">Miktarı yaz, Enter ile devam et. Yoksa 0 gir.</p></article>' +
    '<div class="count-footer"><button id="cancelDraft" class="text-button">Taslağı bırak</button><button id="reviewDraft" class="text-button">Sayımı kontrol et →</button></div></section>'
  );
}
function countOrder() {
  const cats = [...new Set(draft.products.map((p) => p.category))];
  return cats.flatMap((c) => draft.products.filter((p) => p.category === c));
}
function currentCountProduct() {
  const rows = countOrder();
  return (
    rows.find((p) => p.id === draft.cursor) ??
    rows.find((p) => !validInput(draft.values[p.id]?.[selectedField()])) ??
    rows[0]
  );
}
function updateCountStep(focus = false) {
  if (!draft || !$("#stepQuantity")) return;
  const rows = countOrder(),
    p = currentCountProduct(),
    index = rows.indexOf(p),
    group = rows.filter((r) => r.category === p.category),
    input = $("#stepQuantity");
  draft.cursor = p.id;
  saveDraft();
  $("#stepCategory").textContent = p.category;
  $("#countCategory").value = p.category;
  $("#stepPosition").textContent =
    group.indexOf(p) +
    1 +
    " / " +
    group.length +
    " ürün • Genel " +
    (index + 1) +
    " / " +
    rows.length;
  $("#stepHeading").innerHTML = productHeading(p);
  input.dataset.quantity = p.id;
  input.dataset.area = selectedField();
  input.value = draft.values[p.id]?.[selectedField()] ?? "";
  input.inputMode = ["Kg", "Litre"].includes(p.unit) ? "decimal" : "numeric";
  input.setAttribute("aria-label", p.name + " " + draft.area + " miktarı");
  input.removeAttribute("aria-invalid");
  $("#stepError").textContent = "";
  $("#previousCount").disabled = index === 0;
  $("#nextCount").textContent =
    index === rows.length - 1
      ? "Gir / Kontrol et ✓"
      : group.indexOf(p) === group.length - 1
        ? "Gir / Sonraki kategori →"
        : "Gir / Sonraki →";
  if (focus) {
    input.focus({ preventScroll: true });
    input.select();
  }
}
function advanceCount() {
  const input = $("#stepQuantity");
  if (!draft || !input) return;
  if (!validInput(input.value)) {
    input.setAttribute("aria-invalid", "true");
    $("#stepError").textContent = "Miktarı gir. Ürün yoksa 0 yaz.";
    input.focus();
    return;
  }
  draft.values[input.dataset.quantity] ??= {};
  draft.values[input.dataset.quantity][selectedField()] = input.value;
  saveDraft();
  const rows = countOrder(),
    i = rows.findIndex((p) => p.id === input.dataset.quantity);
  if (i === rows.length - 1) {
    previewDraft();
    return;
  }
  draft.cursor = rows[i + 1].id;
  updateCountStep(true);
}

function previewDraft() {
  const done = completed();
  if (done !== draft.products.length) {
    toast(
      draft.products.length -
        done +
        " ürünün " +
        draft.area +
        " miktarı eksik.",
    );
    return;
  }
  modal(
    "Göndermeden önce",
    '<div class="card"><h3>Elazığ • ' +
      esc(draft.area) +
      ' sayımı</h3><p class="subtitle">' +
      done +
      ' ürünün miktarı tamamlandı.</p><p class="small">Sayımını ' +
      esc(session.person.name) +
      ' adına göndereceğiz. Müdür onayına kadar stok değişmeyecek.</p></div><p class="small">Kategoriyi seçip Geri ve Sonraki düğmeleriyle miktarları kontrol edebilirsin. Bağlantı hatasında tekrar gönderme aynı sayımı çoğaltmaz.</p><div class="error form-error"></div><button id="sendCount" class="primary full">Sayımı onaya gönder →</button>',
  );
}
