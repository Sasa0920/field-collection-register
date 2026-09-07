import { render, screen, waitFor } from "@testing-library/react";
import App from "./App";
import { api } from "./api/apiClient";

jest.mock("./api/apiClient", () => ({
  api: {
    getRegions: jest.fn(),
    getEstates: jest.fn(),
    getFields: jest.fn(),
    getEntries: jest.fn(),
  },
}));

describe("App Component", () => {
  beforeEach(() => {
    jest.clearAllMocks();
    api.getRegions.mockResolvedValue([
      { id: 1, name: "Region A" },
      { id: 2, name: "Region B" },
    ]);
    api.getEntries.mockResolvedValue([]);
  });

  test("renders main heading and key sections", async () => {
    render(<App />);

    expect(screen.getByText("Field Collection Register")).toBeInTheDocument();
    expect(screen.getByText("New collection entry")).toBeInTheDocument();
    expect(screen.getByText("Filter entries")).toBeInTheDocument();
    expect(screen.getByText("Showing all entries")).toBeInTheDocument();

    await waitFor(() => {
      expect(api.getEntries).toHaveBeenCalled();
    });
  });
});
