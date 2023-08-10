import React from 'react';
import { NavLink } from 'react-router-dom';
import styles from './navbar.module.css';
import uploadImage from '../../../assets/UPLOAD_FILES.png'
import editCategories from '../../../assets/EDIT_CATEGORIES.png'
import overview from '../../../assets/OVERVIEW.png'
import settings from '../../../assets/SETTINGS.png'
import about from '../../../assets/ABOUT.png'
import logout from '../../../assets/LOGOUT.png'


const Navbar: React.FC = () => (
  <div className="bg-gray-800 text-white h-screen w-64 flex flex-col">
    <div className="p-4">
      <h1 className="text-2xl font-bold">Sidebar</h1>
    </div>
    <nav className={styles.navbar_container}>
      <ul className="space-y-2">
        <li className={styles.overview}>
          <NavLink to='/home'>
          <img src={overview} alt =''></img>
            </NavLink>
        </li>
        <li className={styles.upload_files}>
          <NavLink to='/files'>
            <img src={uploadImage} alt =''></img>
          </NavLink>
        </li>
        <li className={styles.edit_categories}>
          <NavLink to='/edit'>
          <img src={editCategories} alt =''></img>
          </NavLink>
        </li>
        <li className={styles.about}>
          <NavLink to='/about'>
          <img src={about} alt =''></img>
          </NavLink>
        </li>
        <li className={styles.settings}>
          <NavLink to='/settings'>
          <img src={settings} alt =''></img>
          </NavLink>
        </li>
        <li className={styles.logout}>
          <NavLink to='/logout'>
          <img src={logout} alt =''></img>
          </NavLink>
        </li>
      </ul>
    </nav>
  </div>
);

export default Navbar;
