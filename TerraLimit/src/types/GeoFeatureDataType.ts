import type { MarkerType } from './MarkerType';

export type GeoFeature = {
  type: 'Feature';
  geometry: {
    type: 'Point';
    coordinates: [number, number];
  };
  properties: MarkerType;
};
export type GeoFeatureCollection = {
  type: 'FeatureCollection';
  features: GeoFeature[];
};
