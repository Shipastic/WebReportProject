import React from 'react';
import { Routes, Route }    from 'react-router-dom';
import Home                 from '../pages/Home';
import LogIn                from '../pages/LogIn';
import About                from '../pages/About';
import NotFound             from '../pages/NotFound';
import Settings             from '../pages/Settings';

import ChildFolderComponent from './ChildFolderComponent/ChildFolderComponent';


interface AppRoutesProps {
  updateBreadcrumbs: (breadcrumbs: string[]) => void;
  breadcrumbs: string[];
  activeFolderId: number | null;
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
        element={<LogIn/>} 
      />
      <Route 
        path="/about" 
        element={<About />} 
      />
      <Route 
        path="*" 
        element={<NotFound/>} 
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
        element={<Settings/>}
      />
    </Routes>
  );
};

export default AppRoutes;