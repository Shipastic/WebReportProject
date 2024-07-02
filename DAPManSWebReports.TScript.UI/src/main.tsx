import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App.tsx';
import { UserProvider } from './Components/UserContext/UserContext';
//import { ReactKeycloakProvider } from '@react-keycloak/web';
//import keycloak from './keycloak';

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <UserProvider>
      <App />
    </UserProvider>
  </React.StrictMode>
   /* 
      <ReactKeycloakProvider authClient={keycloak}>
        <App />
      </ReactKeycloakProvider>
   */
)

