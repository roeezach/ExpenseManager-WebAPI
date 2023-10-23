import React from 'react';
import { Routes, Route, BrowserRouter  } from 'react-router-dom';
import Header from './components/ui/Header/Header';
import Navbar from './components/ui/NavBar/navbar';
import Home from './components/pages/Overview/Overview';
import UploadFiles from './components/pages/UploadFiles/UploadFiles';
import EditCategories from './components/pages/EditCategory/EditCategory';
import RegistrationForm from './components/pages/SignUp/RegistrationForm';
import 'bootstrap/dist/css/bootstrap.min.css';
import { AuthProvider } from './context/AuthContext.tsx';
import LoginForm from './components/pages/SignIn/LoginForm.tsx';
import Logout from './components/ui/SignOut/logout.tsx';

const App: React.FC = () => {

  return (
    <AuthProvider>
    <BrowserRouter>
      <Header />
      <div className="d-flex">
            <Navbar />
        <div className="flex-grow-1">
          <Routes>
          <Route path="/login" element={<LoginForm/>} />
            <Route path="/home" element={<Home />} />
            <Route path="/register" element={<RegistrationForm/>} />
            <Route path="/files" element={<UploadFiles />} />
            <Route path="/edit" element={<EditCategories />} />
            <Route path="/logout" element={<Logout />} /> 
          </Routes>          
        </div>
      </div>
    </BrowserRouter>
    </AuthProvider>
  );
};

export default App;
