import searchIcon from '../../assets/search-interface-symbol.png';
import type { MarkerType } from '@/types/MarkerType';
import type { LocationPickerMapProps } from '@/types/HeaderPropsType';
import { useLocations } from '@/LocationContext/useLocations';
const SearchComponent = ({ onLocationSelect }: LocationPickerMapProps) => {
  const { locations } = useLocations();
  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const value = event.target.value;
    const matchedLocation = locations.find((location: MarkerType) => {
      const fullName = `${location.city}, ${location.region}, ${location.country}`;
      return fullName.toLowerCase() === value.toLowerCase();
    });
    if (matchedLocation && onLocationSelect) {
      onLocationSelect({ ...matchedLocation });
    }
  };
  return (
    <div className="flex items-center bg-white px-2 py-1 rounded-[25px] min-w-80 border-2 border-black-500">
      <img src={searchIcon} alt="Search" className="h-6 w-6" />
      <input
        type="text"
        list="search-suggestions"
        placeholder="Search..."
        className="ml-2 w-full focus:outline-none text-black placeholder:text-black"
        onChange={handleInputChange}
      />
      <datalist id="search-suggestions">
        {locations?.map((location: MarkerType) => (
          <option
            key={location.id}
            value={`${location.city}, ${location.region}, ${location.country}`}
          />
        ))}
      </datalist>
    </div>
  );
};

export default SearchComponent;
