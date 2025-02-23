import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import App from "./App.tsx";
import { BrowserRouter } from "react-router-dom";
import { AuthProvider } from "react-oidc-context";
import { userManager } from "./oidcConfig.ts";
import { ProtectedApp } from "./components/ProtectedApp.tsx";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <BrowserRouter basename="/">
      <AuthProvider
        userManager={userManager} 
        // onSigninCallback={onSigninCallback}
      >
        <ProtectedApp>
          <App />
        </ProtectedApp>

      </AuthProvider>
    </BrowserRouter>
  </StrictMode>
);
