import React, { useState, useEffect, createContext, useContext }  from 'react';
import tokenService from '../../Services/tokenService';
import {UserResponse} from '..//../Models/UserResponse';

const UserContext = createContext(null);

export const UserProvider = ({ children }) => {
  const [user, setUser] = useState<UserResponse | null>(null);

  useEffect(() => {
      setUser(tokenService.getUserResponse());
  }, []);

  const login = (userResponse: UserResponse) => {
    //localStorage.setItem('userResponse', JSON.stringify(userResponse));
    tokenService.saveUserResponse(userResponse);
    setUser(userResponse);
  };

  const logout = () => {
    //localStorage.removeItem('userResponse');
    tokenService.removeUserResponse();
    setUser(null);
  };

  return (
    <UserContext.Provider value={{ user, setUser, login, logout }}>
      {children}
    </UserContext.Provider>
  );
};

export const useUser = () => useContext(UserContext);