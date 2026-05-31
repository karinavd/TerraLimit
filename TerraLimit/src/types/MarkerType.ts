export type MarkerType = {
  id: number | string;
  city?: string;
  region?: string;
  country?: string;
  longitude: number;
  latitude: number;
  timezone?: string;
  stationIdentifier?: string;
  countryName?: string;
  waterType?: string;
};
