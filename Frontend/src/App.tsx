import React from 'react';
import { Routes, Route, BrowserRouter } from 'react-router-dom';
import Header from './components/ui/Header/Header';
import Navbar from './components/ui/NavBar/navbar';
import Home from './components/pages/homepage/Homepage';
import UploadFiles from './components/pages/UploadFiles/UploadFiles';
import EditCategories from './components/pages/EditCategory/EditCategory';

const App: React.FC = () => {
  return (
    <BrowserRouter>
      <Header/>
      <Navbar />
        <Routes>
          <Route path="/home" element={<Home/>} />
          <Route path="/files" element={<UploadFiles/>} />
          <Route path="/edit" element={<EditCategories/>} />
        </Routes>
    </BrowserRouter>
  );
};

export default App;
