import React from 'react'
import ReactDOM from 'react-dom/client'
import { BrowserRouter } from 'react-router-dom'
import { AutenticacaoProvider } from './contexts/AutenticacaoProvider' // <-- Importe
import App from './App.jsx'
import 'bootstrap/dist/css/bootstrap.min.css'
import 'bootstrap/dist/js/bootstrap.bundle.min.js'

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <BrowserRouter>
      <AutenticacaoProvider> {/* <-- Adicione */}
        <App />
      </AutenticacaoProvider>
    </BrowserRouter>
  </React.StrictMode>,
)