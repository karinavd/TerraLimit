import Header from './components/Header/Header';
import { Outlet } from 'react-router-dom';
import type { MarkerType } from './types/MarkerType';

const LayoutWithHeader = ({
  onLocationSelect,
}: {
  onLocationSelect: (location: MarkerType) => void;
}) => {
  return (
    <div>
      <Header onLocationSelect={onLocationSelect} />
      <main className="h-[calc(100vh-80px)] w-full">
        <Outlet />
      </main>
    </div>
  );
};

export default LayoutWithHeader;
