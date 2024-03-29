import Cookies from 'universal-cookie';
import React, { createContext, useContext, useState, ReactNode, useEffect } from 'react';


interface AuthContextType {
  isLoggedIn: boolean;
  setIsLoggedInData: (value: boolean) => void;
  user: User | null;
  setUserData: (userData: User | null) => void;
}

interface User {
  username: string;
  userID : number
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

interface AuthProviderProps {
  children: ReactNode;
}

export function AuthProvider({ children }: AuthProviderProps) {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [user, setUser] = useState<User | null>(null);

  const setUserData = (userData: User | null) => {
    setUser(userData);
    if (userData) {
      localStorage.setItem('currentUser', JSON.stringify(userData));
    } else {
      localStorage.removeItem('currentUser');
    }
  };

  const setIsLoggedInData = (value: boolean) => {
    setIsLoggedIn(value);
    console.log(`isLoggedIn updated to: ${value}`);
  };

  useEffect(() => {
    const cookie = new Cookies();
    const token = cookie.get('token');
    if (token) {
      setIsLoggedInData(true);
      const user = localStorage.getItem('currentUser'); 
      setUserData(user ? JSON.parse(user) : null); 
      console.log(`username is ${user}`);
    } else {
      setIsLoggedIn(false);
    }
  }, [isLoggedIn]);
  

  return (
    <AuthContext.Provider value={{ isLoggedIn, setIsLoggedInData, user, setUserData }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
}
