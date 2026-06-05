import type { MarkerType } from './MarkerType';

export type LocationPickerMapProps = {
  onLocationSelect?: (location: MarkerType) => void;
  isAboutUsPage?: boolean;
};
