// A single shared "how many requests are in flight right now" counter,
// so every screen shares ONE global loading indicator instead of each
// component keeping its own `loading` useState flag.
//
// Usage:
//   import { trackPromise, subscribeToLoading } from "./loadingTracker";
//   const data = await trackPromise(fetch("/api/regions"));
//
// Any component (usually just one - the global indicator) can call
// subscribeToLoading(callback) to be told whenever the pending count
// changes between zero and non-zero.

let pendingCount = 0;
const listeners = new Set();

function notify() {
  const isLoading = pendingCount > 0;
  listeners.forEach((listener) => listener(isLoading));
}

/**
 * Wrap any promise (typically a fetch call) so the global indicator
 * shows itself while it's pending and hides itself once every
 * currently-tracked promise has settled.
 */
export function trackPromise(promise) {
  pendingCount += 1;
  notify();

  return promise.finally(() => {
    pendingCount = Math.max(0, pendingCount - 1);
    notify();
  });
}

/**
 * Subscribe to loading state changes. Returns an unsubscribe function.
 * Call the callback immediately with the current state so a component
 * mounting mid-request starts in the right state.
 */
export function subscribeToLoading(callback) {
  listeners.add(callback);
  callback(pendingCount > 0);
  return () => listeners.delete(callback);
}
