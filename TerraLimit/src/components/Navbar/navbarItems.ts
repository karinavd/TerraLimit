export const navbarItems = [
  {
    title: 'Weather',
    href: '/weather',
    subItems: [
      {
        title: 'Locations',
        categoryKey: 'weather/locations',
        href: '/weather/locations',
      },
      {
        title: 'Observations',
        categoryKey: 'weather/observations',
        href: '/weather/observations',
      },
      {
        title: 'Air Quality',
        categoryKey: 'weather/air_qualities',
        href: '/weather/air-quality',
      },
      {
        title: 'Atmosphere Metrics',
        categoryKey: 'weather/atmosphere_metrics',
        href: '/weather/atmosphere-metrics',
      },
    ],
  },
  {
    title: 'Water',
    href: '/water',
    subItems: [
      {
        title: 'Stations',
        categoryKey: 'water/stations',
        href: '/water/stations',
      },
      {
        title: 'Records',
        categoryKey: 'water/records',
        href: '/water/records',
      },
      {
        title: 'Parameters',
        categoryKey: 'water/parameters',
        href: '/water/parameters',
      },
    ],
  },
  { title: 'About Us', href: '/about' },
  { title: 'Dashboard', href: '/dashboard' },
];
