/**
 * Vitest / jsdom global test setup.
 *
 * This file is loaded before every test file. It:
 * 1. Extends Vitest's `expect` with jest-dom matchers (toBeInTheDocument, etc.)
 * 2. Provides a safe in-memory localStorage mock so tests that call
 *    SecurityContext or axiosClient do not crash under jsdom.
 */

import "@testing-library/jest-dom/vitest";
import { vi } from "vitest";

// ---------------------------------------------------------------------------
// Safe localStorage mock — jsdom includes localStorage but React's test
// environment initialization (module-level code in SecurityContext.tsx) runs
// before any test setup, so we ensure localStorage is always mockable.
// ---------------------------------------------------------------------------
const localStorageMock = (() => {
  let store: Record<string, string> = {};
  return {
    getItem: (key: string): string | null => store[key] ?? null,
    setItem: (key: string, value: string): void => {
      store[key] = value;
    },
    removeItem: (key: string): void => {
      delete store[key];
    },
    clear: (): void => {
      store = {};
    },
  };
})();

Object.defineProperty(globalThis, "localStorage", {
  value: localStorageMock,
  writable: true,
});

// ---------------------------------------------------------------------------
// crypto.randomUUID polyfill for jsdom environments that don't include it.
// ---------------------------------------------------------------------------
if (typeof globalThis.crypto === "undefined") {
  Object.defineProperty(globalThis, "crypto", {
    value: {
      randomUUID: (): string =>
        `${Date.now()}-${Math.random().toString(36).slice(2)}`,
    },
    writable: true,
  });
} else if (typeof globalThis.crypto.randomUUID !== "function") {
  Object.defineProperty(globalThis.crypto, "randomUUID", {
    value: (): string =>
      `${Date.now()}-${Math.random().toString(36).slice(2)}`,
    writable: true,
  });
}
