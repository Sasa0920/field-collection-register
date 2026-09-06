import { useEffect, useState } from "react";
import FormControl from "@mui/material/FormControl";
import InputLabel from "@mui/material/InputLabel";
import Select from "@mui/material/Select";
import MenuItem from "@mui/material/MenuItem";
import { api } from "../api/apiClient";

export default function EstateDropdown({ regionId, value, onChange, allowAll = false }) {
  const [estates, setEstates] = useState([]);

  useEffect(() => {
    if (!regionId) { setEstates([]); return; }
    let isCurrent = true;
    api.getEstates(regionId).then((data) => { if (isCurrent) setEstates(data); }).catch(console.error);
    return () => { isCurrent = false; };
  }, [regionId]);

  return (
    <FormControl fullWidth size="small" sx={{ mb: 2 }} disabled={allowAll ? false : !regionId}>
      <InputLabel id="estate-label">Estate</InputLabel>
      <Select
        labelId="estate-label"
        label="Estate"
        value={value ?? (allowAll ? "all" : "")}
        onChange={(e) => onChange(e.target.value === "all" ? null : Number(e.target.value))}
      >
        {allowAll && <MenuItem value="all">All Estates</MenuItem>}
        {estates.map((e) => (
          <MenuItem key={e.id} value={e.id}>{e.name}</MenuItem>
        ))}
      </Select>
    </FormControl>
  );
}