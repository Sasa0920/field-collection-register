import { useEffect, useState } from "react";
import Container from "@mui/material/Container";
import Typography from "@mui/material/Typography";
import Divider from "@mui/material/Divider";
import Box from "@mui/material/Box";
import GlobalLoadingIndicator from "./components/GlobalLoadingIndicator";
import RegionDropdown from "./components/RegionDropdown";
import EstateDropdown from "./components/EstateDropdown";
import FieldDropdown from "./components/FieldDropdown";
import EntryForm from "./components/EntryForm";
import EntryTable from "./components/EntryTable";
import { api } from "./api/apiClient";

export default function App() {
  // State for the entry form (submitting a new entry) - unchanged.
  const [formRegionId, setFormRegionId] = useState(null);
  const [formEstateId, setFormEstateId] = useState(null);
  const [formFieldId, setFormFieldId] = useState(null);

  // Separate state for filtering the entries table below.
  // null = "All" for that level.
  const [filterRegionId, setFilterRegionId] = useState(null);
  const [filterEstateId, setFilterEstateId] = useState(null);
  const [filterFieldId, setFilterFieldId] = useState(null);

  const [entries, setEntries] = useState([]);

  function handleFormRegionChange(id) { setFormRegionId(id); setFormEstateId(null); setFormFieldId(null); }
  function handleFormEstateChange(id) { setFormEstateId(id); setFormFieldId(null); }

  useEffect(() => {
    api
      .getEntries({ regionId: filterRegionId, estateId: filterEstateId, fieldId: filterFieldId })
      .then(setEntries)
      .catch(console.error);
  }, [filterRegionId, filterEstateId, filterFieldId]);

  function handleEntryCreated(newEntry) {
    setEntries((prev) => [newEntry, ...prev]);
  }

  return (
    <Container maxWidth="md" sx={{ py: 4 }}>
      <GlobalLoadingIndicator />
      <Typography variant="h4" gutterBottom>Field Collection Register</Typography>
      <Typography variant="body2" color="text.secondary" sx={{ mb: 3 }}>
        Sandbox practice project. All Regions, Estates, and Fields below are made up.
      </Typography>

      <Typography variant="h6" gutterBottom>New collection entry</Typography>
      <RegionDropdown value={formRegionId} onChange={handleFormRegionChange} />
      <EstateDropdown regionId={formRegionId} value={formEstateId} onChange={handleFormEstateChange} />
      <FieldDropdown estateId={formEstateId} value={formFieldId} onChange={setFormFieldId} />
      <EntryForm fieldId={formFieldId} onEntryCreated={handleEntryCreated} />

      <Divider sx={{ my: 3 }} />

      <Typography variant="h6" gutterBottom>Filter entries</Typography>
      <RegionDropdown value={filterRegionId} onChange={setFilterRegionId} allowAll />
      <EstateDropdown regionId={filterRegionId} value={filterEstateId} onChange={setFilterEstateId} allowAll />
      <FieldDropdown estateId={filterEstateId} value={filterFieldId} onChange={setFilterFieldId} allowAll />

      <Typography variant="subtitle2" color="text.secondary" sx={{ mb: 1 }}>
        {filterRegionId || filterEstateId || filterFieldId ? "Showing filtered results" : "Showing all entries"}
      </Typography>
      <Box><EntryTable entries={entries} /></Box>
    </Container>
  );
}