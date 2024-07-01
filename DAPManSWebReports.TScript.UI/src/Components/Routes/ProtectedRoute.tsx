import React from 'react';
import { Navigate } from 'react-router-dom';
import { useUser } from '../UserContext/UserContext';

const ProtectedRoute: React.FC<{ role?: string; children: React.ReactNode }> = ({ role, children }) => {
  const { user } = useUser();

  if (!user || (role && user.role !== role)) {
    return <Navigate to="/login" />;
  }

  return <>{children}</>;
};

export default ProtectedRoute;