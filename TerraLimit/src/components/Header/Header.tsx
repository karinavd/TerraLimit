import { useNavigate } from 'react-router-dom';
import logo from '../../assets/Logo.png';
import menu from '../../assets/menu.png';
import SearchComponent from './SearchComponent';
import type { LocationPickerMapProps } from '@/types/HeaderPropsType';
const Header = ({ onLocationSelect }: LocationPickerMapProps) => {
  const navigate = useNavigate();
  const handleMenuClick = () => {
    navigate('/menu');
  };
  return (
    <div className="h-20 w-full flex items-center justify-between px-4">
      <img src={logo} alt="Logo" className="h-full" />

      <div className="flex items-center justify-center gap-5">
        <SearchComponent onLocationSelect={onLocationSelect} />
        <img
          src={menu}
          alt="Menu"
          className="h-7 cursor-pointer"
          onClick={() => handleMenuClick()}
        />
      </div>
    </div>
  );
};

export default Header;
