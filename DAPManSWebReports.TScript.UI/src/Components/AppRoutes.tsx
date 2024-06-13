import React from 'react';
import { Routes, Route }    from 'react-router-dom';
import Home                 from '../pages/Home';
import LogIn                from '../pages/LogIn';
import About                from '../pages/About';
import NotFound             from '../pages/NotFound';
import Settings             from '../pages/Settings';

import ChildFolderComponent from './ChildFolderComponent';


interface AppRoutesProps {
  updateBreadcrumbs: (breadcrumbs: string[]) => void;
  breadcrumbs: string[];
  activeFolderId: string | null;
  isFolderComponentVisible: boolean;
}

const AppRoutes: React.FC<AppRoutesProps> = ({ 
  updateBreadcrumbs, 
  breadcrumbs,
  activeFolderId,
  isFolderComponentVisible
}) => {
  return (
    <Routes>
      <Route 
        path="/home" 
        element={<Home
                    path=""
                    updateBreadcrumbs={updateBreadcrumbs}
                    breadcrumbs={breadcrumbs}
                 />} 
      />
      <Route 
        path="/login" 
        element={<LogIn
                    path="/login"
                    updateBreadcrumbs={updateBreadcrumbs}
                    breadcrumbs={breadcrumbs}
                 />} 
      />
      <Route 
        path="/about" 
        element={<About
                    path="/about"
                    updateBreadcrumbs={updateBreadcrumbs}
                    breadcrumbs={breadcrumbs}
                 />} 
      />
      <Route 
        path="*" 
        element={<NotFound
                    path="NotFoundPage"
                    updateBreadcrumbs={updateBreadcrumbs}
                    breadcrumbs={breadcrumbs}
                 />} 
      />
      <Route 
        path="/" 
        element={activeFolderId !== null && isFolderComponentVisible && (
          <ChildFolderComponent
            parentid={activeFolderId}
            path={'/activeFolderId'}
            updateBreadcrumbs={updateBreadcrumbs}
            breadcrumbs={breadcrumbs}
          />
        )}
      />
      <Route 
        path="/settings" 
        element={<Settings
                    path=""
                    updateBreadcrumbs={updateBreadcrumbs}
                    breadcrumbs={breadcrumbs}
                 />}
      />
    </Routes>
  );
};

export default AppRoutes;