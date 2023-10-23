import { useAuth } from '../../../context/AuthContext';
import React, {  useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Cookies from 'universal-cookie';

const Logout : React.FC = () => {
  const navigate = useNavigate();
  const cookie = new Cookies();
  const { setIsLoggedInData,isLoggedIn} = useAuth();    
    useEffect(() => {
        console.log('you reached the logout');
        cookie.remove('token');
        console.log(`after removing the cookie is ${cookie.get('token')}`);        
        setIsLoggedInData(false);
        navigate('/login');
      }, [navigate ,isLoggedIn, cookie]);

      return null;
};

export default Logout;
