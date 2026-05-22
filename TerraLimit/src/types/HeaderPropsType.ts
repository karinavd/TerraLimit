import type { MarkerType } from './MarkerType';

export type MapComponentProps = {
  onLocationSelect?: (location: MarkerType) => void;
};
