import { Link, useNavigate } from 'react-router-dom';
import { navbarItems } from './navbarItems';
import { useLocations } from '@/LocationContext/useLocations';

interface NavbarProps {
  isSidebarOpen: boolean;
  toggleSidebar: () => void;
  setIsSidebarOpen: (val: boolean) => void;
}

const Navbar = ({
  isSidebarOpen,
  toggleSidebar,
  setIsSidebarOpen,
}: NavbarProps) => {
  const navigate = useNavigate();
  const { setActiveCategory } = useLocations();

  return (
    <>
      {isSidebarOpen && (
        <div
          className="fixed inset-0 bg-black/50 z-40 transition-opacity"
          onClick={toggleSidebar}
        />
      )}

      <div
        className={`fixed top-20 left-0 w-72  bg-white z-50 transform transition-transform duration-300 ease-in-out border-r border-gray-200 ${
          isSidebarOpen ? 'translate-x-0' : '-translate-x-full'
        }`}
        style={{ height: 'calc(100vh - 80px)' }}
      >
        <div className="p-8 flex flex-col gap-6 overflow-y-auto">
          {navbarItems.map((item, index) => (
            <div key={index} className="flex flex-col">
              <Link
                to={item.href}
                target={item.isExternal ? "_blank" : "_self"}
  rel={item.isExternal ? "noopener noreferrer" : ""}
                onClick={(e) => {
                  if (item.categoryKey) {
                    e.preventDefault();
                    setActiveCategory(item.categoryKey);
                    navigate('/');
                  }
                  setIsSidebarOpen(false);
                }}
                className="text-gray-800 font-['Abril_Fatface'] hover:opacity-70 transition-opacity text-[28px]"
              >
                {item.title}
              </Link>

              {item.subItems && (
                <div className="flex flex-col gap-3 pl-4 mt-3 border-l-2 border-gray-300">
                  {item.subItems.map((subItem, subIndex) => (
                    <button
                      key={subIndex}
                      onClick={() => {
                        if (subItem.categoryKey) {
                          setActiveCategory(subItem.categoryKey);
                          setIsSidebarOpen(false);
                          navigate('/');
                        }
                      }}
                      className="text-left text-gray-600 text-[20px] font-['Abril_Fatface'] hover:text-black transition-colors"
                    >
                      {subItem.title}
                    </button>
                  ))}
                </div>
              )}
            </div>
          ))}
        </div>
      </div>
    </>
  );
};

export default Navbar;
