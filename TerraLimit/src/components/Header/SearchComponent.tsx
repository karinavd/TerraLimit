import React, { useEffect, useState } from 'react';
import searchIcon from '../../../public/search-interface-symbol.png';
import { getAllLocations } from '../Map/getAllLocations';
import type { MarkerType } from '@/types/MarkerType';
import type { MapComponentProps } from '@/types/HeaderPropsType';
const SearchComponent = ({ onLocationSelect }: MapComponentProps) => {
  const [suggestions, setSuggestions] = useState<MarkerType[]>([]);
  useEffect(() => {
    const fetchSuggestions = async () => {
      const res = await getAllLocations();
      setSuggestions(res);
    };
    fetchSuggestions();
  }, []);

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const value = event.target.value;
    const matchedLocation = suggestions.find((suggestion) => {
      const fullName = `${suggestion.city}, ${suggestion.region}, ${suggestion.country}`;
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
        {suggestions.map((location, index) => (
          <option
            key={index}
            value={`${location.city}, ${location.region}, ${location.country}`}
          />
        ))}
      </datalist>
    </div>
  );
};

export default SearchComponent;
