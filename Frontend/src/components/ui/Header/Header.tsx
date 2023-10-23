import React from 'react';
import styles from './Header.module.css'; // Import the styles
import { useAuth } from '../../../context/AuthContext'; // Import your AuthContext

const Header: React.FC = () => {
  const { user } = useAuth(); 

  const renderGreeting = () => {
    const { isLoggedIn } = useAuth(); // Get the isLoggedIn state from your context
  
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
