import { Link, useNavigate } from 'react-router-dom';
import { navbarItems } from './navbarItems';
import { useLocations } from '@/LocationContext/useLocations';

const Navbar = () => {
  const navigate = useNavigate();
  const { setActiveCategory } = useLocations();
  return (
    <div className="h-16 w-full flex items-center justify-start bg-[#afafaf] px-6 z-50 relative">
      <nav className="flex gap-8">
        {navbarItems.map((item, index) => (
          <div key={index} className="relative group">
            <Link
              to={item.href}
              className="text-white font-['Abril_Fatface'] hover:opacity-80 transition-opacity hover:text-black text-[30px]"
            >
              {item.title}
            </Link>
            {item.subItems && (
              <div className="absolute left-0 top-full hidden group-hover:flex flex-col pt-2 z-50">
                <div className="flex flex-col gap-2 bg-white/90 p-4 rounded-lg shadow-xl min-w-[250px]">
                  {item.subItems.map((subItem, subIndex) => (
                    <button
                      key={subIndex}
                      onClick={() => {
                        if (subItem.categoryKey) {
                          setActiveCategory(subItem.categoryKey);
                          navigate('/');
                        }
                      }}
                      className="text-left text-gray-800 text-[20px] font-['Abril_Fatface'] hover:opacity-80 transition-colors cursor-pointer"
                    >
                      {subItem.title}
                    </button>
                  ))}
                </div>
              </div>
            )}
          </div>
        ))}
      </nav>
    </div>
  );
};

export default Navbar;
