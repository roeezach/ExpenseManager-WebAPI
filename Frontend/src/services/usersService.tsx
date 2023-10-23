import axios from 'axios';
import Errors from '../utils/Errors';

interface RegisteredUser {
    username: string;
    password: string;
    firstName:string;
    lastName:string;
    email:string;
  }

  interface loginUser {
    username: string;
    password: string;
  }

const SignUp = async (user : RegisteredUser) => {
    const usersUrl = `${process.env.REACT_APP_BACKEND_BASE_URL}/Users/SignUp/SignUp`

    try {
      const response = await axios.post(usersUrl, user);
      if(response){
            return response.data;        
      } else if (response.status === 409) {
            throw new Error('Username already exists');
      } else {
            throw new Error('Registration failed');
      }

    } catch (error) {
      console.error('registartion error:', error);
      throw new Error('registartion failed');
    }
  };


  const SignIn = async (user: loginUser) => {
    const usersUrl = `${process.env.REACT_APP_BACKEND_BASE_URL}/Users/SignIn/SignIn`;
    try {
      const response = await axios.post(usersUrl, user);
      if (response) {
        return response.data;
      } else if (response.status === 401) {
        throw new Errors.InvalidPasswordOrUserNotFound();
      } else {
        throw new Error('Login failed');
      }
    } catch (error) {
      console.log(`the err on the service ${error}`);      
      if (error.response && (error.response.status === 401 || error.response.status === 404 )) {
        throw new Errors.InvalidPasswordOrUserNotFound();
      }
      else if (error.response && error.response.status === 403) {
        throw new Errors.JWTExpired();
      }
      else{
        console.error('Login error:', error);
        throw new Error('Login failed');
      }
    }
  };

  const usersService = {
    SignUp,
    SignIn
  }

  export default usersService;