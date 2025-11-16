/* eslint-disable no-debugger */
import { useAuth0 } from '@auth0/auth0-react';
import LoginButton from './component/LoginButton';
import LogoutButton from './component/LogoutButton';
import DashboardPage from './pages/DashboardPage';
import icon from './assets/icon.png';

function App() {
  const { isAuthenticated, isLoading, error } = useAuth0();

  if (isLoading) {
    return (
      <div className="app-container">
        <div className="loading-state">
          <div className="loading-text">Loading...</div>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="app-container">
        <div className="error-state">
          <div className="error-title">Oops!</div>
          <div className="error-message">Something went wrong</div>
          <div className="error-sub-message">{error.message}</div>
        </div>
      </div>
    );
  }

  return (
    <div className="app-container">
      {isAuthenticated ? (
        <div className="logged-in-section">
          <div className="profile-card">
            <div className="game-card-wrapper">
              <DashboardPage />
              <div className="action-card">
                <LogoutButton />
              </div>
            </div>
          </div>
        </div>
      ) : (
        <div className="main-card-wrapper">
          <img
            src={icon}
            alt="Auth0 Logo"
            className="auth0-logo"
            onError={(e) => {
              e.currentTarget.style.display = 'none';
            }}
          />
          <h1 className="main-title">Welcome to Tic Tac Toe</h1>
          <div className="action-card">

            <p className="action-text">Get started by signing in to your account</p>
            <LoginButton />
          </div>
        </div>
      )}
    </div>

  );
}

export default App;