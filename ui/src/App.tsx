import './App.css';
import React from 'react';
import { BrowserRouter, Route, Routes } from "react-router-dom";
import Home from './pages/home/Home';
import Login from './pages/login/Login';
import { HttpRequestServiceProvider } from './contexts/HttpRequestServiceContext';
import HttpRequestService from './services/HttpRequestService';
import Register from './pages/register/Register';

const App = () => {
  return (
    <HttpRequestServiceProvider httpRequestService={new HttpRequestService()}>
      <BrowserRouter>
        <Routes>
          <Route path='/' element={<Home />} />
          <Route path='/login' element={<Login />} />
          <Route path='/register' element={<Register />} />
        </Routes>
      </BrowserRouter>
    </HttpRequestServiceProvider>
  );
}

export default App;
