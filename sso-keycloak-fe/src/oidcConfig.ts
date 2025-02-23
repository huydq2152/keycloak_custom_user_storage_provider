import { UserManager, WebStorageStateStore } from "oidc-client-ts";

export const userManager = new UserManager({
  authority: "http://localhost:8080/realms/test-keycloak",           // Thay bằng URL IdP của bạn
  client_id: "react-vite-client-test",                      // Thay bằng client_id của bạn
  redirect_uri: `${window.location.origin}/oidc-login`, // Callback URL
  post_logout_redirect_uri: `${window.location.origin}/oidc-logout`, // URL sau khi đăng xuất
  response_type: "code",                          // Sử dụng code flow (chuẩn OIDC hiện đại)
  scope: "openid profile email",                  // Thay bằng scope bạn cần
  // Bạn cũng có thể bổ sung các thiết lập khác như silent_redirect_uri nếu làm silent renew
  userStore: new WebStorageStateStore({ store: window.localStorage }),
  monitorSession: true, // this allows cross tab login/logout detection
  automaticSilentRenew: true,
  silent_redirect_uri: `${window.location.origin}/silent-renew`, // URL silent renew
});

export const onSigninCallback = () => {
  window.history.replaceState({}, document.title, '/todo');
};

