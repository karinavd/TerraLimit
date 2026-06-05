import { useState } from 'react';
import { Outlet, useLocation } from 'react-router-dom';
import Header from './components/Header/Header';
import Navbar from './components/Navbar/Navbar';
import type { MarkerType } from './types/MarkerType';

interface LayoutProps {
  onLocationSelect: (location: MarkerType) => void;
}

const LayoutWithHeader = ({ onLocationSelect }: LayoutProps) => {
  const [isSidebarOpen, setIsSidebarOpen] = useState(false);
  const toggleSidebar = () => setIsSidebarOpen(!isSidebarOpen);

  const location = useLocation();
  const isAboutUsPage = location.pathname === '/about';

  return (
    <div className="flex flex-col h-full w-full">
      <Header
        onLocationSelect={onLocationSelect}
        isAboutUsPage={isAboutUsPage}
        isSidebarOpen={isSidebarOpen}
        toggleSidebar={toggleSidebar}
      />
      <Navbar
        isSidebarOpen={isSidebarOpen}
        toggleSidebar={toggleSidebar}
        setIsSidebarOpen={setIsSidebarOpen}
      />
      <div className="flex-1 overflow-hidden relative">
        <Outlet />
      </div>
    </div>
  );
};

export default LayoutWithHeader;
