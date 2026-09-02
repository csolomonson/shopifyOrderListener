import { build } from "esbuild";
import { mkdir, rm } from "node:fs/promises";
import { fileURLToPath } from "node:url";
import { dirname, resolve } from "node:path";

const root = resolve(dirname(fileURLToPath(import.meta.url)), "..");
const output = resolve(root, "static", "dist");

await rm(output, { recursive: true, force: true });
await mkdir(output, { recursive: true });

await build({
  entryPoints: [resolve(root, "frontend", "src", "main.jsx")],
  absWorkingDir: root,
  bundle: true,
  minify: true,
  sourcemap: false,
  outfile: resolve(output, "app.js"),
  define: { "process.env.NODE_ENV": '"production"' },
  loader: { ".jsx": "jsx" },
});
