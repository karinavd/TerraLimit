import type { MarkerType } from './MarkerType';

export type CategorizedSearchMapProps = {
  searchedLocation: MarkerType | null;
  onPointSelect?: (location: MarkerType | null) => void;
};
