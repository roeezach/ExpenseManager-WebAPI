import React from 'react';
import { NavLink } from 'react-router-dom';
import styles from './navbar.module.css';
import { FaHome, FaUpload, FaEdit, FaInfoCircle, FaCog, FaSignOutAlt } from 'react-icons/fa'; // Import the specific icons
import { useAuth } from '../../../context/AuthContext';

const Navbar: React.FC = () => 
{
  const { isLoggedIn } = useAuth();
  console.log(`nav re-rendered`);
  console.log(`isLoggedIn in navbar is ${isLoggedIn}`);
  
  if( !isLoggedIn)
    return null;
  
  return (
  <div className="bg-gray-800 text-white h-screen w-64 flex flex-col">
    <div className="p-4">
      <h1 className="text-2xl font-bold">Sidebar</h1>
    </div>
    <nav className={styles.navbar_container}>
      <ul className="space-y-2">
        <li className={styles.navbar_item}>
          <NavLink to='/home' className={styles.navLink} style={{ color:'white' , display:'flex', alignItems:'center', gap:'0.5rem', textDecoration:'none', marginBottom:'0.3rem'}}>
            <FaHome />
            <span>Overview</span>
          </NavLink>
        </li>
        <li className={styles.navbar_item}>
          <NavLink to='/files' className={styles.navLink} style={{ color:'white' , display:'flex', alignItems:'center', gap:'0.5rem', textDecoration:'none' ,marginBottom:'0.3rem'}}>
            <FaUpload />
            <span>Upload Files</span>
          </NavLink>
        </li>
        <li className={styles.navbar_item}>
          <NavLink to='/edit' className={styles.navLink} style={{ color:'white' , display:'flex', alignItems:'center', gap:'0.5rem', textDecoration:'none' , marginBottom:'0.5rem'}}>
            <FaEdit />
            <span>Edit Categories</span>
          </NavLink>
        </li>
        <li className={styles.navbar_item}>
          <NavLink to='/about' className={styles.navLink} style={{ color:'white' , display:'flex', alignItems:'center', gap:'0.5rem', textDecoration:'none' , marginBottom:'0.5rem'}}>
            <FaInfoCircle />
            <span>About</span>
          </NavLink>
        </li>
        <li className={styles.navbar_item}>
          <NavLink to='/settings' className={styles.navLink} style={{ color:'white' , display:'flex', alignItems:'center', gap:'0.5rem', textDecoration:'none' ,marginBottom:'0.5rem'}}>
            <FaCog />
            <span>Settings</span>
          </NavLink>
        </li>
        <li className={styles.navbar_item}>
          <NavLink to='/logout' className={styles.navLink} style={{ color:'white' , display:'flex', alignItems:'center', gap:'0.5rem', textDecoration:'none' ,marginBottom:'0.5rem'}}>
            <FaSignOutAlt />
            <span>Logout</span>
          </NavLink>
        </li>
      </ul>
    </nav>
  </div>
  );
};
    

export default Navbar;
