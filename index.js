
import React from 'react';
import ReactDOM from 'react-dom/client';
import './src/styles/index.scss';
import App from './src/App';
(() => {
    console.log('webpack worked')
  })()
const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
);