import React from 'react';
import styles from './Header.module.css'; 
import { useAuth } from '../../../context/AuthContext';

const Header: React.FC = () => {
  const { isLoggedIn, user } = useAuth(); 
  
  const renderGreeting = () => { 
    if (isLoggedIn && user) {
      return (
        <span className={styles.greeting}>Hello, {user.username}</span>
      );
    }
    return (
      <span className={styles.greeting}>Hello, Guest</span>
    );
  };

  return (
    <div className={styles.header_container}>
      <div className={styles.greetingContainer}>{renderGreeting()}</div>
    </div>
  );
};

export default Header;
