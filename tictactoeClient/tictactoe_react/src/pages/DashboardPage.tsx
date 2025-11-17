import { useAuth0 } from "@auth0/auth0-react";
import TictactoeComponent from '../component/TictactoeComponent';
import Service from "../service/service";
import React, { useEffect} from 'react';
import ScoreboardComponent from "../component/ScoreboardComponent";


const DashboardPage = () => {
  const { user, isAuthenticated, isLoading } = useAuth0();

  const hasRunRef = React.useRef(false);

  useEffect(() => {
    if (!user?.email) return;
    if (hasRunRef.current) return;

    hasRunRef.current = true;

    const IfNotExistsCreate = async () => {
      const email = user.email ?? "";
      const _user = await Service.getUser(email);
      if (_user) {
        if (_user.length === 0) {
          await Service.CreateUser(email);
          window.location.reload();
        }
      }
    };

    IfNotExistsCreate();
  }, [user]);

  if (isLoading) {
    return <div className="loading-text">Loading profile...</div>;
  }

  return (
    isAuthenticated && user ? (
      <div className="dashboard-container">
        <img 
          src={user.picture || `data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='110' height='110' viewBox='0 0 110 110'%3E%3Ccircle cx='55' cy='55' r='55' fill='%2363b3ed'/%3E%3Cpath d='M55 50c8.28 0 15-6.72 15-15s-6.72-15-15-15-15 6.72-15 15 6.72 15 15 15zm0 7.5c-10 0-30 5.02-30 15v3.75c0 2.07 1.68 3.75 3.75 3.75h52.5c2.07 0 3.75-1.68 3.75-3.75V72.5c0-9.98-20-15-30-15z' fill='%23fff'/%3E%3C/svg%3E`} 
          alt={user.name || 'User'} 
          className="profile-picture"
          style={{ 
            width: '110px', 
            height: '110px', 
            borderRadius: '50%', 
            objectFit: 'cover',
            border: '3px solid #63b3ed'
          }}
          onError={(e) => {
            const target = e.target as HTMLImageElement;
            target.src = `data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='110' height='110' viewBox='0 0 110 110'%3E%3Ccircle cx='55' cy='55' r='55' fill='%2363b3ed'/%3E%3Cpath d='M55 50c8.28 0 15-6.72 15-15s-6.72-15-15-15-15 6.72-15 15 6.72 15 15 15zm0 7.5c-10 0-30 5.02-30 15v3.75c0 2.07 1.68 3.75 3.75 3.75h52.5c2.07 0 3.75-1.68 3.75-3.75V72.5c0-9.98-20-15-30-15z' fill='%23fff'/%3E%3C/svg%3E`;
          }}
        />
        <div style={{ textAlign: 'center' }}>
          <div className="profile-name" style={{ fontSize: '2rem', fontWeight: '600', color: '#f7fafc', marginBottom: '0.5rem' }}>
            {user.name}
          </div>
          <div className="profile-email" style={{ fontSize: '1.15rem', color: '#a0aec0' }}>
            {user.email}
          </div>
          <h1>Tic Tac Toe</h1>
        </div>
        <div className='row col-12'>
          <div className="col-xl-3">
            <ScoreboardComponent />
          </div>
          <div className="col-xl-6">
            <TictactoeComponent user={user} />
          </div>
          <div className="col-xl-3">
          </div>
        </div>

      </div>
    ) : null
  );
};

export default DashboardPage;