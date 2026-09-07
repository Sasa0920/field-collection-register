import { useState } from "react";
import Box from "@mui/material/Box";
import TextField from "@mui/material/TextField";
import Button from "@mui/material/Button";
import Alert from "@mui/material/Alert";
import { api } from "../api/apiClient";

export default function EntryForm({ fieldId, onEntryCreated }) {
  const [date, setDate] = useState("");
  const [quantity, setQuantity] = useState("");
  const [note, setNote] = useState("");
  const [error, setError] = useState("");
  const [submitting, setSubmitting] = useState(false);

  function validate() {
    // Basic input validation: never trust whatever the user typed.
    // The backend re-checks all of this too - this is just for a fast,
    // friendly error message before a network round trip.
    if (!fieldId) return "Select a Region, Estate, and Field first.";
    if (!date) return "A collection date is required.";
    if (new Date(date) > new Date()) return "Date cannot be in the future.";

    const numericQuantity = Number(quantity);
    if (quantity === "" || Number.isNaN(numericQuantity)) {
      return "Quantity must be a number.";
    }
    if (numericQuantity <= 0) {
      return "Quantity must be greater than zero.";
    }

    return "";
  }

  async function handleSubmit(e) {
    e.preventDefault();
    const validationError = validate();
    if (validationError) {
      setError(validationError);
      return;
    }

    setError("");
    setSubmitting(true);
    try {
      const created = await api.createEntry({
        fieldId,
        collectionDate: date,
        quantity: Number(quantity),
        note: note || null,
      });
      setDate("");
      setQuantity("");
      setNote("");
      onEntryCreated(created);
    } catch (err) {
      setError(err.message || "Something went wrong submitting the entry.");
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <Box component="form" onSubmit={handleSubmit} sx={{ display: "flex", flexDirection: "column", gap: 2, mb: 3 }}>
      {error && <Alert severity="error">{error}</Alert>}
      <TextField
        //label="Collection date"
        type="date"
        size="small"
        slotProps={{ inputLabel: { shrink: true } }}
        value={date}
        onChange={(e) => setDate(e.target.value)}
      />
      <TextField
        label="Quantity collected"
        type="number"
        size="small"
        value={quantity}
        onChange={(e) => setQuantity(e.target.value)}
      />
      <TextField
        label="Note (optional)"
        size="small"
        multiline
        minRows={2}
        value={note}
        onChange={(e) => setNote(e.target.value)}
      />
      <Button type="submit" variant="contained" disabled={submitting}>
        {submitting ? "Submitting..." : "Submit entry"}
      </Button>
    </Box>
  );
}
