import { trackPromise } from "../utils/loadingTracker";

// Base URL for the .NET backend. In a real project this would come from
// an environment variable rather than being hardcoded.
const BASE_URL = "http://localhost:5178/api";

async function request(path, options = {}) {
  const response = await trackPromise(
    fetch(`${BASE_URL}${path}`, {
      headers: { "Content-Type": "application/json" },
      ...options,
    })
  );

  if (!response.ok) {
    let message = `Request failed with status ${response.status}`;
    try {
      const body = await response.json();
      if (body?.error) message = body.error;
    } catch {
      // response wasn't JSON - keep the generic message
    }
    throw new Error(message);
  }

  if (response.status === 204) return null;
  return response.json();
}

export const api = {
  getRegions: () => request("/regions"),
  getEstates: (regionId) => request(`/estates?regionId=${regionId}`),
  getFields: (estateId) => request(`/fields?estateId=${estateId}`),
  getEntries: (filters = {}) => {
    const params = new URLSearchParams(
      Object.entries(filters).filter(([, v]) => v !== null && v !== undefined)
    );
    const query = params.toString();
    return request(`/entries${query ? `?${query}` : ""}`);
  },
  createEntry: (entry) =>
    request("/entries", {
      method: "POST",
      body: JSON.stringify(entry),
    }),
};
