import { BrowserRouter, Route, Routes } from 'react-router-dom';
import './App.css';
import MapComponent from './components/Map/Map';
import type { MarkerType } from './types/MarkerType';
import { useState } from 'react';
import Navbar from './components/Navbar/Navbar';
import LayoutWithHeader from './LayoutWithHeader';
function App() {
  const [searchedLocation, setSearchedLocation] = useState<MarkerType | null>(
    null
  );
  return (
    <div className="h-screen w-screen">
      <BrowserRouter>
        <Routes>
          <Route
            element={
              <LayoutWithHeader
                onLocationSelect={(location: MarkerType) =>
                  setSearchedLocation(location)
                }
              />
            }
          >
            <Route
              path="/"
              element={<MapComponent searchedLocation={searchedLocation} />}
            />
          </Route>

          <Route path="/menu" element={<Navbar />} />
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
