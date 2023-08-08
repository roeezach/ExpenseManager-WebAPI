import React from 'react';
import styles from './Homepage.module.css'; // Import your CSS module

const Homepage: React.FC = () => {
  return (
    <div className={`flex flex-col ${styles.centerContent} ${styles.adjustRight}`}>
      <h1 className="text-4xl font-bold text-black">Hello, Welcome!</h1>
    </div>
  );
};

export default Homepage;
