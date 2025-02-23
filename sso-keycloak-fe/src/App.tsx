// src/App.tsx
import { Routes, Route } from "react-router-dom";

import { CallbackPage } from "./pages/CallbackPage";
import { TodoPage } from "./pages/TodoPage";
import { HomePage } from "./pages/HomePage";
import SilentRenew from "./pages/SilentRenew";
import LogoutRedirect from "./pages/LogoutRedirect";

function App() {
  return (
    <Routes>
      <Route path="/oidc-login" element={<CallbackPage />} />
      <Route path="/oidc-logout" element={<LogoutRedirect />} />
      <Route path="/silent-renew" element={<SilentRenew />} />

      <Route
        path="/todo"
        element={
            <TodoPage />
        }
      />

      <Route path="/" element={<HomePage />} />
    </Routes>
  );
}

export default App;
