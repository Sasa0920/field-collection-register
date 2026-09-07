import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import EntryForm from "./EntryForm";
import { api } from "../api/apiClient";

jest.mock("../api/apiClient", () => ({
  api: {
    createEntry: jest.fn(),
  },
}));

describe("EntryForm", () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  test("shows validation error when submitted without fieldId", () => {
    const handleEntryCreated = jest.fn();
    render(<EntryForm fieldId={null} onEntryCreated={handleEntryCreated} />);

    fireEvent.click(screen.getByRole("button", { name: /submit entry/i }));

    expect(screen.getByText("Select a Region, Estate, and Field first.")).toBeInTheDocument();
    expect(handleEntryCreated).not.toHaveBeenCalled();
    expect(api.createEntry).not.toHaveBeenCalled();
  });

  test("shows validation error when date is empty", () => {
    render(<EntryForm fieldId={1} onEntryCreated={jest.fn()} />);

    fireEvent.click(screen.getByRole("button", { name: /submit entry/i }));

    expect(screen.getByText("A collection date is required.")).toBeInTheDocument();
  });

  test("shows validation error when quantity is invalid or zero", () => {
    const { container } = render(<EntryForm fieldId={1} onEntryCreated={jest.fn()} />);

    // Fill valid past date
    const dateInput = container.querySelector('input[type="date"]');
    fireEvent.change(dateInput, { target: { value: "2026-09-01" } });

    // Try submit without quantity
    fireEvent.click(screen.getByRole("button", { name: /submit entry/i }));
    expect(screen.getByText("Quantity must be a number.")).toBeInTheDocument();

    // Try quantity <= 0
    const quantityInput = screen.getByLabelText(/Quantity collected/i);
    fireEvent.change(quantityInput, { target: { value: "0" } });
    fireEvent.click(screen.getByRole("button", { name: /submit entry/i }));
    expect(screen.getByText("Quantity must be greater than zero.")).toBeInTheDocument();
  });

  test("successfully submits entry when inputs are valid", async () => {
    const handleEntryCreated = jest.fn();
    const createdMock = {
      id: 99,
      fieldId: 1,
      collectionDate: "2026-09-01",
      quantity: 45.5,
      note: "Morning pickup",
    };
    api.createEntry.mockResolvedValueOnce(createdMock);

    const { container } = render(<EntryForm fieldId={1} onEntryCreated={handleEntryCreated} />);

    // Date
    const dateInput = container.querySelector('input[type="date"]');
    fireEvent.change(dateInput, { target: { value: "2026-09-01" } });

    // Quantity
    const quantityInput = screen.getByLabelText(/Quantity collected/i);
    fireEvent.change(quantityInput, { target: { value: "45.5" } });

    // Note
    const noteInput = screen.getByLabelText(/Note \(optional\)/i);
    fireEvent.change(noteInput, { target: { value: "Morning pickup" } });

    // Submit
    fireEvent.click(screen.getByRole("button", { name: /submit entry/i }));

    await waitFor(() => {
      expect(api.createEntry).toHaveBeenCalledWith({
        fieldId: 1,
        collectionDate: "2026-09-01",
        quantity: 45.5,
        note: "Morning pickup",
      });
      expect(handleEntryCreated).toHaveBeenCalledWith(createdMock);
    });
  });
});
