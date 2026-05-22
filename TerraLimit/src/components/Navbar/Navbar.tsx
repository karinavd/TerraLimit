import { navbarItems } from './navbarItems';

const Navbar = () => {
  return (
    <div className="h-screen w-full flex items-start justify-start bg-[#afafaf] p-3">
      <nav>
        {navbarItems.map((item, index) => (
          <div key={index} className="">
            <a
              href={item.href}
              className="text-white font-['Abril_Fatface'] hover:opacity-80  transition-opacity hover:text-black text-[50px]"
            >
              {item.title}
            </a>
            {item.subItems && (
              <div className="flex flex-col gap-2 mt-2 ml-4">
                {item.subItems.map((subItem, subIndex) => (
                  <a
                    key={subIndex}
                    href={subItem.href}
                    className="text-white text-[30px] font-['Abril_Fatface'] hover:opacity-80 hover:text-black transition-opacity"
                  >
                    {subItem.title}
                  </a>
                ))}
              </div>
            )}
          </div>
        ))}
      </nav>
    </div>
  );
};

export default Navbar;
