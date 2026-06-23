import { defineConfig } from "vite";

// https://vitejs.dev/config/
export default defineConfig({
  // Note: @vitejs/plugin-react is not added here because Vite 8 does not yet
  // have a compatible plugin-react version. Vite handles JSX/TSX transformation
  // via esbuild natively for the dev server and build. For the test environment,
  // Vitest uses the same esbuild pipeline with the jsdom environment configured below.
  plugins: [],
  test: {
    // Use jsdom to simulate a browser environment for React component tests
    environment: "jsdom",
    // Load global test setup before each test file
    setupFiles: ["./src/test/setup.ts"],
    globals: true,
  },
});
