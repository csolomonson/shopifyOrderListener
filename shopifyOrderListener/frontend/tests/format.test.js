import assert from "node:assert/strict";
import test from "node:test";

import { stateTone, statusLabel } from "../src/format.js";

test("review states are presented as readable labels", () => {
  assert.equal(statusLabel("customer_review"), "customer review");
});

test("danger lifecycle events use the red queue tone", () => {
  assert.equal(stateTone({ severity: "danger" }), "red");
});
