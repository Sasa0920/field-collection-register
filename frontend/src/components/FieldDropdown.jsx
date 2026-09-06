import { useEffect, useState } from "react";
import FormControl from "@mui/material/FormControl";
import InputLabel from "@mui/material/InputLabel";
import Select from "@mui/material/Select";
import MenuItem from "@mui/material/MenuItem";
import { api } from "../api/apiClient";

export default function FieldDropdown({ estateId, value, onChange, allowAll = false }) {
  const [fields, setFields] = useState([]);

  useEffect(() => {
    if (!estateId) { setFields([]); return; }
    let isCurrent = true;
    api.getFields(estateId).then((data) => { if (isCurrent) setFields(data); }).catch(console.error);
    return () => { isCurrent = false; };
  }, [estateId]);

  return (
    <FormControl fullWidth size="small" sx={{ mb: 2 }} disabled={allowAll ? false : !estateId}>
      <InputLabel id="field-label">Field</InputLabel>
      <Select
        labelId="field-label"
        label="Field"
        value={value ?? (allowAll ? "all" : "")}
        onChange={(e) => onChange(e.target.value === "all" ? null : Number(e.target.value))}
      >
        {allowAll && <MenuItem value="all">All Fields</MenuItem>}
        {fields.map((f) => (
          <MenuItem key={f.id} value={f.id}>{f.name}</MenuItem>
        ))}
      </Select>
    </FormControl>
  );
}