import type { LocationContextType } from '@/types/LocationContextType';
import { createContext } from 'react';

export const LocationContext = createContext<LocationContextType | undefined>(
  undefined
);
