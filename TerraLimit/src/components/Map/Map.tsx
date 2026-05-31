import { useEffect, useMemo, useRef, useState } from 'react';
import {
  Map,
  MapClusterLayer,
  MapPopup,
  MapControls,
  type MapRef,
} from '@/components/ui/map';
import type { MarkerType } from '@/types/MarkerType';
import type { GeoFeatureCollection } from '@/types/GeoFeatureDataType';
import type { CategorizedSearchMapProps } from '@/types/MapComponentPropsType';
import { useLocations } from '@/LocationContext/useLocations';

export default function MapComponent({
  searchedLocation,
  onPointSelect,
}: CategorizedSearchMapProps) {
  const { locations } = useLocations();
  const [selectedPoint, setSelectedPoint] = useState<{
    coordinates: [number, number];
    properties: MarkerType;
  } | null>(null);
  const mapRef = useRef<MapRef>(null);
  const locationsData = useMemo<GeoFeatureCollection | null>(() => {
    if (!locations || locations.length === 0) return null;
    return {
      type: 'FeatureCollection',
      features: locations.map((location: MarkerType) => ({
        type: 'Feature',
        geometry: {
          type: 'Point',
          coordinates: [location.longitude, location.latitude],
        },
        properties: location,
      })),
    };
  }, [locations]);

  useEffect(() => {
    if (searchedLocation && mapRef.current) {
      mapRef.current.flyTo({
        center: [searchedLocation.longitude, searchedLocation.latitude],
        zoom: 10,
        duration: 1000,
      });
      setSelectedPoint({
        coordinates: [searchedLocation.longitude, searchedLocation.latitude],
        properties: searchedLocation,
      });
    }
  }, [searchedLocation]);

  return (
    <div className="h-full w-full">
      <Map ref={mapRef} center={[-103.59, 40.66]} zoom={8} fadeDuration={0}>
        {locationsData && (
          <MapClusterLayer<MarkerType>
            data={locationsData}
            clusterRadius={50}
            clusterMaxZoom={14}
            clusterColors={['#1d8cf8', '#6d5dfc', '#e23670']}
            pointColor="#1d8cf8"
            onPointClick={(feature, coordinates) => {
              setSelectedPoint({
                coordinates,
                properties: feature.properties,
              });
              if (onPointSelect) {
                onPointSelect(feature.properties);
              }
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
              <p className="text-muted-foreground">Location: </p>
              {selectedPoint.properties.city && (
                <p className="text-foreground font-medium">
                  City: {selectedPoint.properties.city}
                </p>
              )}
              {selectedPoint.properties.stationIdentifier && (
                <p className="text-foreground font-medium">
                  Station: {selectedPoint.properties.stationIdentifier}
                </p>
              )}
              {selectedPoint.properties.waterType && (
                <p className="text-foreground font-medium">
                  Type: {selectedPoint.properties.waterType}
                </p>
              )}
              {selectedPoint.properties.country && (
                <p className="text-foreground font-medium">
                  Country: {selectedPoint.properties.country}
                </p>
              )}
            </div>
          </MapPopup>
        )}
        <MapControls />
      </Map>
    </div>
  );
}
