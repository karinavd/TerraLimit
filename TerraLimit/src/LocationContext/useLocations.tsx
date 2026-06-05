import { useContext } from 'react';
import { LocationContext } from './LocationContext';

export const useLocations = () => {
  const context = useContext(LocationContext);
  if (context === undefined)
    throw new Error('useLocations must be used within a LocationProvider');
  return context;
};
