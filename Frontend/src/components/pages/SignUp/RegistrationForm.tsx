import React, { useState } from 'react';
import { Button, Form, Container, Row, Col } from 'react-bootstrap';
import usersService from '../../../services/usersService';
import Cookies from 'universal-cookie';
import { useNavigate } from 'react-router';
import { Link } from 'react-router-dom';
import { useAuth } from '../../../context/AuthContext';
import Errors from '../../../utils/Errors';

interface RegisteredUser {
  username: string;
  password: string;
  firstName: string;
  lastName: string;
  email: string;
}

const RegistrationForm: React.FC = () => {
  const [user, setUser] = useState<RegisteredUser>({
    username: '',
    password: '',
    firstName: '',
    lastName: '',
    email: '',
  });

  const [errors, setErrors] = useState<Record<string, string>>({}); // Object to hold error messages
  const { setIsLoggedInData, setUserData } = useAuth();
  const navigate = useNavigate();
  const cookie = new Cookies(null, { path: '/' });

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setUser({ ...user, [name]: value });
    // Clear the error message for the current field
    setErrors({ ...errors, [name]: '' });
  };

  const validateEmail = (email: string): boolean => {
    // You can use a regular expression to validate the email format
    const emailRegex = /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i;
    return emailRegex.test(email);
  };

  const validateStrongPassword = (password: string): boolean => {
    if (password.length < 6 || password.length > 24) {
      return false; // Password length is not within the required range.
    }
  
    const hasLetter = /[a-zA-Z]/.test(password);
    const hasSpecialChar = /[!@#$%^&*]/.test(password);
  
    return hasLetter && hasSpecialChar;
  };

  const handleRegistration = async () => {
    const newErrors: Record<string, string> = {};

    // Validate fields
    if (!user.username) {
      newErrors.username = 'Username cannot be empty';
    }
    if (!user.password) {
      newErrors.password = 'Password cannot be empty';
    } else if (!validateStrongPassword(user.password)) {
      newErrors.password = 'Password should be stronger : 6-24 and with letters and special characters';
    }
    if (!user.firstName) {
      newErrors.firstName = 'First name cannot be empty';
    }
    if (!user.lastName) {
      newErrors.lastName = 'Last name cannot be empty';
    }
    if (!user.email) {
      newErrors.email = 'Email cannot be empty';
    } else if (!validateEmail(user.email)) {
      newErrors.email = 'Invalid email format';
    }

    if (Object.keys(newErrors).length > 0) {
      // If there are errors, don't proceed with registration
      setErrors(newErrors);
      return;
    }

    try {
      const response = await usersService.SignUp(user);
      console.log('Registration successful:', response);
      cookie.set('token', response.token);
      cookie.set('token', response.username);
      setUserData(response.username ? { username: response.username , userID: response.userID} : null);
      setIsLoggedInData(true);
      navigate('../home');
    } catch (error) {
      if (error.status === 403) {
        throw new Error('JWTExpired');
      } else if (error.status === 404) {
        throw Errors.InvalidPasswordOrUserNotFound;
      } else {
        console.error('Registration error:', error);
      }
    }
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    handleRegistration();
  };

  return (
    <Container className="mt-5">
      <Row className="justify-content-center">
        <Col sm={6}>
          <Form onSubmit={handleSubmit}>
            <h2 className="text-center">Registration</h2>
            <Form.Group controlId="username">
              <Form.Label>Username</Form.Label>
              <Form.Control
                type="text"
                name="username"
                value={user.username}
                onChange={handleInputChange}
                style={{ fontSize: '14px' }}
              />
              <Form.Text className="text-danger">{errors.username}</Form.Text>
            </Form.Group>
            <Form.Group controlId="password">
              <Form.Label>Password</Form.Label>
              <Form.Control
                type="password"
                name="password"
                value={user.password}
                onChange={handleInputChange}
                style={{ fontSize: '14px' }}
              />
              <Form.Text className="text-danger">{errors.password}</Form.Text>
            </Form.Group>
            <Form.Group controlId="firstName">
              <Form.Label>First Name</Form.Label>
              <Form.Control
                type="text"
                name="firstName"
                value={user.firstName}
                onChange={handleInputChange}
                style={{ fontSize: '14px' }}
              />
              <Form.Text className="text-danger">{errors.firstName}</Form.Text>
            </Form.Group>
            <Form.Group controlId="lastName">
              <Form.Label>Last Name</Form.Label>
              <Form.Control
                type="text"
                name="lastName"
                value={user.lastName}
                onChange={handleInputChange}
                style={{ fontSize: '14px' }}
              />
              <Form.Text className="text-danger">{errors.lastName}</Form.Text>
            </Form.Group>
            <Form.Group controlId="email">
              <Form.Label>Email</Form.Label>
              <Form.Control
                type="email"
                name="email"
                value={user.email}
                onChange={handleInputChange}
                style={{ fontSize: '14px' }}
              />
              <Form.Text className="text-danger">{errors.email}</Form.Text>
            </Form.Group>
            <Button variant="dark" type="submit" className="w-100">
              Register
            </Button>
            <p className="text-center mt-2">
              Already Registered?{' '}
              <Link to="/login">Log in now</Link>
            </p>
          </Form>
        </Col>
      </Row>
    </Container>
  );
};

export default RegistrationForm;
