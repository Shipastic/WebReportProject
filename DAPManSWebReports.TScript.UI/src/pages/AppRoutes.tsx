import React from 'react';
import { RouteObject } from 'react-router-dom';
import Home         from '../pages/Home';
import About        from '../pages/About';
import ShowDataView from '../pages/ShowDataView';
import LogIn        from '../pages/LogIn';

const AppRoutes: RouteObject[] =
    [
        { path: '/',         element: <Home /> },
        { path: '/about',    element: <About /> },
        { path: '/dataview', element: <ShowDataView /> },
        { path: '/login',    element: <LogIn /> }
    ];


export default AppRoutes;
