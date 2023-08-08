import React from 'react';
import { NavLink } from 'react-router-dom';
import  styles from './navbar.module.css';

const Navbar: React.FC = () => (
  <div className="bg-gray-800 text-white h-screen w-64 flex flex-col">
    <div className="p-4">
      <h1 className="text-2xl font-bold">Sidebar</h1>
    </div>
    <nav className={styles.navbar_container}>
      <ul className="space-y-2">
        <li className="p-4 hover:bg-gray-700 cursor-pointer">
          <NavLink to='/home'>Homepage</NavLink>
        </li>
        <li className="p-4 hover:bg-gray-700 cursor-pointer">
          <NavLink to='/files'>Upload Files</NavLink>
        </li>
        <li className="p-4 hover:bg-black-700 cursor-pointer">
          <NavLink to='/edit' className="mt-2">Edit Categories</NavLink>
        </li>
      </ul>
    </nav>
  </div>
);

export default Navbar;
