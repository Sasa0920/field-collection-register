import { useEffect, useState } from "react";
import FormControl from "@mui/material/FormControl";
import InputLabel from "@mui/material/InputLabel";
import Select from "@mui/material/Select";
import MenuItem from "@mui/material/MenuItem";
import { api } from "../api/apiClient";

export default function RegionDropdown({ value, onChange, allowAll = false }) {
  const [regions, setRegions] = useState([]);

  useEffect(() => {
    api.getRegions().then(setRegions).catch(console.error);
  }, []);

  return (
    <FormControl fullWidth size="small" sx={{ mb: 2 }}>
      <InputLabel id="region-label">Region</InputLabel>
      <Select
        labelId="region-label"
        label="Region"
        value={value ?? (allowAll ? "all" : "")}
        onChange={(e) => onChange(e.target.value === "all" ? null : Number(e.target.value))}
      >
        {allowAll && <MenuItem value="all">All Regions</MenuItem>}
        {regions.map((r) => (
          <MenuItem key={r.id} value={r.id}>{r.name}</MenuItem>
        ))}
      </Select>
    </FormControl>
  );
}