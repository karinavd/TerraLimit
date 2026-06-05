import SearchComponent from './SearchComponent';
import type { LocationPickerMapProps } from '@/types/HeaderPropsType';
import menu from '../../assets/menu.png';

type HeaderProps = LocationPickerMapProps & {
  isSidebarOpen: boolean;
  toggleSidebar: () => void;
};

const Header = ({
  onLocationSelect,
  isAboutUsPage = false,
  toggleSidebar,
}: HeaderProps) => {
  return (
    <header className="h-20 w-full flex items-center justify-between px-6 bg-white relative z-50">
      <div className="flex-none">
        <img
          src={menu}
          alt="Menu"
          className="h-8 cursor-pointer hover:opacity-80 transition-opacity"
          onClick={toggleSidebar}
        />
      </div>
      {isAboutUsPage && (
        <div className="absolute left-1/2 top-1/2 -translate-x-1/2 -translate-y-1/2 pointer-events-none">
          <h1 className="text-2xl md:text-5xl font-['Abril_Fatface'] uppercase tracking-wide text-gray-900 whitespace-nowrap">
            About Our Team
          </h1>
        </div>
      )}
      <div className="flex-none ml-auto">
        {!isAboutUsPage && (
          <SearchComponent onLocationSelect={onLocationSelect} />
        )}
      </div>
    </header>
  );
};

export default Header;
