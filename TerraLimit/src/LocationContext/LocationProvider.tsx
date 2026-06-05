import { useEffect, useState, type ReactNode } from 'react';
import { getAllLocations } from '../components/Map/getAllLocations';
import type { MarkerType } from '../types/MarkerType';
import { LocationContext } from './LocationContext';

const LocationProvider = ({ children }: { children: ReactNode }) => {
  const [locations, setLocations] = useState<MarkerType[]>([]);
  const [activeCategory, setActiveCategory] = useState('weather/observations');

  useEffect(() => {
    const fetchLocations = async () => {
      const endpoint = activeCategory.startsWith('water')
        ? 'water/stations'
        : 'weather/locations';

      const data = await getAllLocations(endpoint);
      setLocations(data);
    };

    fetchLocations();
  }, [activeCategory]);

  return (
    <LocationContext.Provider
      value={{ locations, activeCategory, setActiveCategory }}
    >
      {children}
    </LocationContext.Provider>
  );
};

export default LocationProvider;
