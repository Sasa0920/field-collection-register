import { useEffect, useState } from "react";
import LinearProgress from "@mui/material/LinearProgress";
import { subscribeToLoading } from "../utils/loadingTracker";

// The ONE place in the whole app that renders a loading bar.
// No other component needs its own loading spinner or flag.
export default function GlobalLoadingIndicator() {
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    const unsubscribe = subscribeToLoading(setIsLoading);
    return unsubscribe;
  }, []);

  // Reserve the space either way so the layout doesn't jump when it appears.
  return (
    <div style={{ height: 4 }}>
      {isLoading && <LinearProgress />}
    </div>
  );
}
