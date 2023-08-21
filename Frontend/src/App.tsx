import React from 'react';
import { Routes, Route, BrowserRouter } from 'react-router-dom';
import Header from './components/ui/Header/Header';
import Navbar from './components/ui/NavBar/navbar';
import Home from './components/pages/Overview/Overview';
import UploadFiles from './components/pages/UploadFiles/UploadFiles';
import EditCategories from './components/pages/EditCategory/EditCategory';
import 'bootstrap/dist/css/bootstrap.min.css';

const App: React.FC = () => {
  return (
    <BrowserRouter>
      <Header />
      <div className="d-flex">
        <Navbar />
        <div className="flex-grow-1">
          <Routes>
            <Route path="/home" element={<Home />} />
            <Route path="/files" element={<UploadFiles />} />
            <Route path="/edit" element={<EditCategories />} />
          </Routes>
        </div>
      </div>
    </BrowserRouter>
  );
};

export default App;
