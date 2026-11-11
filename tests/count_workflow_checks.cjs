const fs = require("fs"),
  vm = require("vm"),
  assert = require("assert");
const nodes = new Map(),
  storage = new Map(),
  events = {};
function node(id) {
  if (!nodes.has(id))
    nodes.set(id, {
      value: "",
      dataset: {},
      style: {},
      setAttribute(k, v) {
        this[k] = v;
      },
      removeAttribute(k) {
        delete this[k];
      },
      focus() {
        this.focused = true;
      },
      select() {},
    });
  return nodes.get(id);
}
const ctx = vm.createContext({
  console,
  document: {
    querySelector: node,
    addEventListener: (e, fn) => (events[e] = fn),
  },
  window: { addEventListener() {} },
  localStorage: { setItem: (k, v) => storage.set(k, v) },
  setTimeout,
  clearTimeout,
  URLSearchParams,
});
const path = require("path");
const web = path.join(__dirname, "../CafeStockAI.Mobile/wwwroot");
let source =
  fs.readFileSync(path.join(web, "count-workflow.js"), "utf8") +
  "\n" +
  fs
    .readFileSync(path.join(web, "app.js"), "utf8")
    .replace(/boot\(\);\s*$/, "");
vm.runInContext(source, ctx);
vm.runInContext(
  `session={demo:true,person:{id:'test'}};draft={area:'Bar',products:[{id:'a',category:'Sos',name:'A',unit:'Şişe'},{id:'c',category:'Süt',name:'C',unit:'Koli'},{id:'b',category:'Sos',name:'B',unit:'Şişe'}],values:{}};`,
  ctx,
);
assert.equal(vm.runInContext("countOrder().map(p=>p.id).join()", ctx), "a,b,c");
vm.runInContext("updateCountStep(true)", ctx);
const input = node("#stepQuantity");
assert.equal(input.dataset.quantity, "a");
assert.equal(input.inputMode, "numeric");
input.value = "";
vm.runInContext("advanceCount()", ctx);
assert.equal(input.dataset.quantity, "a");
assert.equal(input["aria-invalid"], "true");
input.value = "0";
vm.runInContext("advanceCount()", ctx);
assert.equal(input.dataset.quantity, "b");
assert.equal(vm.runInContext("draft.values.a.bar", ctx), "0");
assert.equal(input.focused, true);
input.value = "10";
events.keydown({
  target: { id: "stepQuantity" },
  key: "Enter",
  preventDefault() {},
});
assert.equal(input.dataset.quantity, "c");
assert.equal(node("#stepCategory").textContent, "Süt");
assert.equal(vm.runInContext("draft.values.b.bar", ctx), "10");
assert.equal(vm.runInContext("draft.values.b.depot", ctx), undefined);
vm.runInContext("draft.cursor='a';updateCountStep(true)", ctx);
assert.equal(input.value, "0");
assert.equal(node("#stepQuantity"), input);
assert.equal(JSON.parse([...storage.values()][0]).values.b.bar, "10");
console.log(
  "PASS: category grouping, blank rejection, zero, Enter, category transition, same input/focus, back value, saved draft, untouched Depo",
);
