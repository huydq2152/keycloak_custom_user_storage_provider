import { useEffect } from "react";
import { useAuth } from "react-oidc-context";
import { Navigate, useNavigate } from "react-router-dom";

// export const CallbackPage = () => {
//   const auth = useAuth();

//   if (!auth.isLoading && auth.isAuthenticated) {
//     return <Navigate to="/todo" replace />;
//   }
//   return null;
// };

export const CallbackPage = () => {
  const auth = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    // Khi auth đã xử lý xong và user được xác thực
    if (!auth.isLoading && auth.isAuthenticated) {
      // Điều hướng sang /todo thay vì hiển thị "callback"
      
      navigate("/todo", { replace: true });
    }
  }, [auth.isLoading, auth.isAuthenticated, navigate]);

  return <div>Loading...</div>;
};