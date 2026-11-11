"use strict";
if (new URLSearchParams(location.search).get("telefon") === "1") {
  const nodes = [...document.body.childNodes];
  document.body.classList.add("phone-preview");
  const stage = document.createElement("div");
  stage.className = "device-stage";
  const device = document.createElement("div");
  device.className = "phone-device";
  device.setAttribute("aria-label", "Telefon önizlemesi");
  const screen = document.createElement("div");
  screen.className = "phone-screen";
  const status = document.createElement("div");
  status.className = "phone-status";
  status.setAttribute("aria-hidden", "true");
  status.innerHTML =
    '<span>9:41</span><span class="phone-indicators"><span class="phone-signal">▂▄▆█</span><span>⌁</span><span class="phone-battery"></span></span>';
  const island = document.createElement("div");
  island.className = "phone-island";
  island.setAttribute("aria-hidden", "true");
  const home = document.createElement("div");
  home.className = "phone-home";
  home.setAttribute("aria-hidden", "true");
  const app = document.createElement("div");
  app.className = "phone-app";
  for (const node of nodes) app.appendChild(node);
  screen.append(status, island, app, home);
  device.append(screen);
  stage.append(device);
  document.body.append(stage);
  const fit = () =>
    stage.style.setProperty(
      "--phone-scale",
      Math.max(
        0.25,
        Math.min(1, (innerHeight - 30) / 892, (innerWidth - 32) / 414),
      ),
    );
  fit();
  addEventListener("resize", fit);
}
