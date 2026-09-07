import { render, screen } from "@testing-library/react";
import EntryTable from "./EntryTable";

describe("EntryTable", () => {
  test("renders empty message when entries list is empty", () => {
    render(<EntryTable entries={[]} />);
    expect(screen.getByText(/No entries yet for this filter/i)).toBeInTheDocument();
  });

  test("renders table headers and rows when entries are provided", () => {
    const mockEntries = [
      {
        id: 1,
        collectionDate: "2026-09-01",
        regionName: "Central Highlands",
        estateName: "Valley Green",
        fieldName: "Field A",
        quantity: 150.25,
        note: "Good harvest",
      },
      {
        id: 2,
        collectionDate: "2026-09-02",
        regionName: "Central Highlands",
        estateName: "Valley Green",
        fieldName: "Field B",
        quantity: 80.0,
        note: null,
      },
    ];

    render(<EntryTable entries={mockEntries} />);

    // Table headers
    expect(screen.getByText("Date")).toBeInTheDocument();
    expect(screen.getByText("Region")).toBeInTheDocument();
    expect(screen.getByText("Estate")).toBeInTheDocument();
    expect(screen.getByText("Field")).toBeInTheDocument();
    expect(screen.getByText("Quantity")).toBeInTheDocument();
    expect(screen.getByText("Note")).toBeInTheDocument();

    // Data rows
    expect(screen.getByText("2026-09-01")).toBeInTheDocument();
    expect(screen.getByText("Field A")).toBeInTheDocument();
    expect(screen.getByText("150.25")).toBeInTheDocument();
    expect(screen.getByText("Good harvest")).toBeInTheDocument();

    expect(screen.getByText("2026-09-02")).toBeInTheDocument();
    expect(screen.getByText("Field B")).toBeInTheDocument();
    expect(screen.getByText("80")).toBeInTheDocument();
    // Default dash for empty note
    expect(screen.getByText("—")).toBeInTheDocument();
  });
});
