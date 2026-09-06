import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Paper from "@mui/material/Paper";
import Typography from "@mui/material/Typography";

export default function EntryTable({ entries }) {
  if (entries.length === 0) {
    return <Typography color="text.secondary">No entries yet for this filter.</Typography>;
  }

  return (
    <TableContainer component={Paper} variant="outlined">
      <Table size="small">
        <TableHead>
          <TableRow>
            <TableCell>Date</TableCell>
            <TableCell>Region</TableCell>
            <TableCell>Estate</TableCell>
            <TableCell>Field</TableCell>
            <TableCell align="right">Quantity</TableCell>
            <TableCell>Note</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {entries.map((entry) => (
            <TableRow key={entry.id}>
              <TableCell>{entry.collectionDate}</TableCell>
              <TableCell>{entry.regionName}</TableCell>
              <TableCell>{entry.estateName}</TableCell>
              <TableCell>{entry.fieldName}</TableCell>
              <TableCell align="right">{entry.quantity}</TableCell>
              <TableCell>{entry.note || "—"}</TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
}
