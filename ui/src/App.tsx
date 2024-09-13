import './App.css';
import React from 'react';
import { BrowserRouter, Route, Routes } from "react-router-dom";
import Home from './pages/Home/Home';
import Login from './pages/Login/Login';
import { HttpRequestServiceProvider } from './contexts/HttpRequestServiceContext';
import HttpRequestService from './services/HttpRequestService';

const App = () => {
  return (
    <HttpRequestServiceProvider httpRequestService={new HttpRequestService()}>
      <BrowserRouter>
        <Routes>
          <Route path='/' element={<Home />} />
          <Route path='/login' element={<Login />} />
        </Routes>
      </BrowserRouter>
    </HttpRequestServiceProvider>
  );
}

export default App;
