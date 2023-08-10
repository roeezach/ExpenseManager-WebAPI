import React from 'react';
import styles from './Header.module.css'; // Import the styles
import profile from '../../../assets/PROFILE.png';

// Sample user data
const user = {
  name: 'Roee Zach',
  profilePicture: profile,
};

const Header: React.FC = () => {
  const renderGreeting = () => {
    if (user) {
      return (
        <span className={styles.greeting}>Hello, {user.name}</span>
      );
    }
    return (
      <span className={styles.greeting}>Hello, Guest</span>
    );
  };

  return (
    <div className={styles.header_container}>
      <div className={styles.greetingContainer}>{renderGreeting()}</div>
      {user && (
        <div className={`${styles.profileContainer} flex items-center`}>
          <img
            src={user.profilePicture}
            alt="Profile"
            className={styles.profileImage}
          />
        </div>
      )}
    </div>
  );
};

export default Header;
