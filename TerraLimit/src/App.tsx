import { BrowserRouter, Route, Routes } from 'react-router-dom';
import './App.css';
import MapComponent from './components/Map/Map';
import type { MarkerType } from './types/MarkerType';
import { useState } from 'react';
import LayoutWithHeader from './LayoutWithHeader';
import AboutUs from './components/AboutUs/AboutUs';
import Sidebar from './components/Sidebar/Sidebar';
import LocationProvider from './LocationContext/LocationProvider';

function App() {
  const [searchedLocation, setSearchedLocation] = useState<MarkerType | null>(
    null
  );

  return (
    <LocationProvider>
      <div className="h-screen w-screen overflow-hidden">
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
                element={
                  <div className="relative w-full h-full">
                    <MapComponent
                      searchedLocation={searchedLocation}
                      onPointSelect={(location) =>
                        setSearchedLocation(location)
                      }
                    />
                    <Sidebar selectedPoint={searchedLocation} />
                  </div>
                }
              />

              <Route path="/about" element={<AboutUs />} />
            </Route>
          </Routes>
        </BrowserRouter>
      </div>
    </LocationProvider>
  );
}

export default App;
