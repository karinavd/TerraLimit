import type { MarkerType } from './MarkerType';

export type LocationContextType = {
  locations: MarkerType[];
  activeCategory: string;
  setActiveCategory: (category: string) => void;
};
