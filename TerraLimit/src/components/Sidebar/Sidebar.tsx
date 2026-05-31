import { useEffect, useState } from 'react';
import type { MarkerType } from '@/types/MarkerType';
import { useLocations } from '@/LocationContext/useLocations';

interface SidebarProps {
  selectedPoint: MarkerType | null;
}
type DynamicData = Record<string, string | number | boolean | null>;
const Sidebar = ({ selectedPoint }: SidebarProps) => {
  const { activeCategory } = useLocations();
  const [detailsData, setDetailsData] = useState<DynamicData | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  useEffect(() => {
    if (!selectedPoint) return;

    const fetchDetails = async () => {
      setIsLoading(true);
      try {
        const baseURL = 'http://localhost:5088';
        const response = await fetch(
          `${baseURL}/${activeCategory}/${selectedPoint.id}`
        );

        if (response.ok) {
          const data = await response.json();
          setDetailsData(data);
        } else {
          setDetailsData(null);
        }
      } catch (error) {
        console.error('Error fetching details:', error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchDetails();
  }, [selectedPoint, activeCategory]);
  if (!selectedPoint) {
    return (
      <div className="h-full w-1/4 absolute top-0 right-0 z-10 p-6 bg-gray-50 border-l">
        <p className="text-gray-500">Please select a point on the map</p>
      </div>
    );
  }

  return (
    <div className="h-full w-1/4 absolute top-0 right-0 z-10 p-6 bg-white shadow-[-10px_0_15px_rgba(0,0,0,0.1)] overflow-y-auto">
      <h2 className="text-2xl font-bold mb-6 border-b pb-2">
        Location Details
      </h2>
      <div className="mb-6 bg-gray-100 p-4 rounded-lg">
        <p className="text-sm text-gray-500">City / Station</p>
        <p className="text-lg font-medium">
          {selectedPoint.city || selectedPoint.stationIdentifier}
        </p>
      </div>
      <h3 className="text-lg font-bold mb-4 uppercase text-blue-600">
        {activeCategory.replace('/', ' ')} Data
      </h3>

      {isLoading ? (
        <p>Loading data...</p>
      ) : detailsData ? (
        <div className="space-y-3">
          {Object.entries(detailsData).map(([key, value]) => {
            if (key === 'id' || key === 'recordId' || value === null)
              return null;
            return (
              <div
                key={key}
                className="flex justify-between border-b border-gray-100 pb-1"
              >
                <span className="text-gray-600 capitalize">
                  {key.replace(/([A-Z])/g, ' $1').trim()}:
                </span>
                <span className="font-medium">{String(value)}</span>
              </div>
            );
          })}
        </div>
      ) : (
        <p>No {activeCategory} data found for this location.</p>
      )}
    </div>
  );
};

export default Sidebar;
