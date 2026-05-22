import { BrowserRouter, Route, Routes } from 'react-router-dom';
import './App.css';
import MapComponent from './components/Map/Map';
import Header from './components/Header/Header';
import type { MarkerType } from './types/MarkerType';
import { useState } from 'react';
function App() {
  const [searchedLocation, setSearchedLocation] = useState<MarkerType | null>(
    null
  );
  return (
    <div className="h-screen w-screen">
      <BrowserRouter>
        <Header onLocationSelect={setSearchedLocation} />
        <main className="h-[calc(100vh-80px)] w-full">
          <Routes>
            <Route
              path="/"
              element={<MapComponent searchedLocation={searchedLocation} />}
            />
          </Routes>
        </main>
      </BrowserRouter>
    </div>
  );
}

export default App;
