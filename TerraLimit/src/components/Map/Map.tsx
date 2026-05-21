import { useEffect, useState } from 'react';
import {
  Map,
  MapClusterLayer,
  MapPopup,
  MapControls,
} from '@/components/ui/map';
import { getAllLocations } from './getAllLocations';
import type { MarkerType } from '@/types/MarkerType';
import type { GeoFeatureCollection } from '@/types/GeoFeatureDataType';

export default function MapComponent() {
  const [locations, setLocationsData] = useState<GeoFeatureCollection | null>(
    null
  );
  const [selectedPoint, setSelectedPoint] = useState<{
    coordinates: [number, number];
    properties: MarkerType;
  } | null>(null);
  useEffect(() => {
    const fetchLocations = async () => {
      const data = await getAllLocations();
      const geoJSON: GeoFeatureCollection = {
        type: 'FeatureCollection',
        features: data.map((location: MarkerType) => ({
          type: 'Feature',
          geometry: {
            type: 'Point',
            coordinates: [location.longitude, location.latitude],
          },
          properties: location,
        })),
      };
      setLocationsData(geoJSON);
    };

    fetchLocations();
  }, []);
  return (
    <div className="h-full w-full">
      <Map center={[-103.59, 40.66]} zoom={3.4} fadeDuration={0}>
        {locations && (
          <MapClusterLayer<MarkerType>
            data={locations}
            clusterRadius={50}
            clusterMaxZoom={14}
            clusterColors={['#1d8cf8', '#6d5dfc', '#e23670']}
            pointColor="#1d8cf8"
            onPointClick={(feature, coordinates) => {
              setSelectedPoint({
                coordinates,
                properties: feature.properties,
              });
            }}
          />
        )}

        {selectedPoint && (
          <MapPopup
            key={`${selectedPoint.coordinates[0]}-${selectedPoint.coordinates[1]}`}
            longitude={selectedPoint.coordinates[0]}
            latitude={selectedPoint.coordinates[1]}
            onClose={() => setSelectedPoint(null)}
            closeOnClick={false}
            focusAfterOpen={false}
            closeButton
            className="w-34"
          >
            <div className="text-[13px]">
              <p className="text-muted-foreground">Location: </p>{' '}
              <p className="text-foreground font-medium">
                City: {selectedPoint.properties.city}
              </p>{' '}
              <p className="text-foreground font-medium">
                Region: {selectedPoint.properties.region}
              </p>
              <p className="text-foreground font-medium">
                Country: {selectedPoint.properties.country}
              </p>
            </div>
          </MapPopup>
        )}

        <MapControls />
      </Map>
    </div>
  );
}
