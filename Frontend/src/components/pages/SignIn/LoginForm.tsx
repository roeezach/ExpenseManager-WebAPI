import React, {useState } from 'react';
import { Button, Form, Container, Row, Col , Alert } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { useNavigate } from 'react-router';
import Cookies from 'universal-cookie';
import { useAuth } from '../../../context/AuthContext';
import usersService from '../../../services/usersService';


interface LoginUser {
  username: string;
  password: string;
}

const LoginForm: React.FC = () => {
  
    const { setIsLoggedInData, isLoggedIn, setUserData } = useAuth();
    const [formData, setFormData] = useState<LoginUser>({
        username: '',
        password: '',
  });
  const [error, setError] = useState<string>(''); // State to store error messages
  const cookie = new Cookies({ path: '/' });
  const navigate = useNavigate();
  
  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleLogin = async () => {
    try {
      const response = await usersService.SignIn(formData);
      console.log('Login successful:', response);      
      cookie.set('token', response.token);
      cookie.set('username', response.username);
      setUserData(response.username ? { username: response.username , userID: response.userID } : null);
      setIsLoggedInData(true);
      console.log(`after the change: ${isLoggedIn}`);      
      navigate('../home');        
    } catch (error) {
        if (error.message === `jwt expired`) {
            setError('JWTExpired');
          } else if (error.message === 'Invalid password or username') {
              setError('Invalid username or password');              
          }  
          else {
            setError('Login error');
        }
    }
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    handleLogin();
  };

  return (
    <Container className="mt-5">
      <Row className="justify-content-center">
        <Col sm={6}>
          <Form onSubmit={handleSubmit}>
            <h2 className="text-center">Login</h2>
            {error && <Alert variant="danger">{error}</Alert>}
            <Form.Group controlId="username">
              <Form.Label>Username</Form.Label>
              <Form.Control
                type="text"
                name="username"
                value={formData.username}
                onChange={handleInputChange}
                style={{ fontSize: '14px' }}
              />
            </Form.Group>
            <Form.Group controlId="password">
              <Form.Label>Password</Form.Label>
              <Form.Control
                type="password"
                name="password"
                value={formData.password}
                onChange={handleInputChange}
                style={{ fontSize: '14px', marginBottom: '2rem' }}
              />
            </Form.Group>
            <Button variant="dark" type="submit" className="w-100">
              Login
            </Button>
            <p className="text-center mt-2">
              Not a user?{' '}
              <Link to="/register">Sign up now</Link>
            </p>
          </Form>
        </Col>
      </Row>
    </Container>
  );
};

export default LoginForm;
