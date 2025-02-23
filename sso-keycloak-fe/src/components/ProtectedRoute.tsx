// src/components/ProtectedRoute.tsx
import { useAuth } from "react-oidc-context";

interface ProtectedRouteProps {
  children: React.ReactNode;
}

export function ProtectedRoute({ children }: ProtectedRouteProps) {
  const auth = useAuth();

  if (auth.isLoading) {
    return <div>Đang kiểm tra phiên đăng nhập...</div>;
  }

  if (!auth.isAuthenticated) {
    // Chưa đăng nhập -> chuyển hướng sang đăng nhập
    auth.signinRedirect();
    return null; // hoặc <Navigate to="/login" /> nếu có route login riêng
  }

  return <>{children}</>;
}
