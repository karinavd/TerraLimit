import { BrowserRouter, Route, Routes } from 'react-router-dom';
import './App.css';
import MapComponent from './components/Map/Map';
function App() {
  return (
    <div className="h-screen w-screen">
      <BrowserRouter>
      <Routes>
        <Route path="/" element={<MapComponent />} />
      </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
