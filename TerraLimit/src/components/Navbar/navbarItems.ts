export const navbarItems = [
  {
    title: 'Weather',
    categoryKey: 'weather/locations',
    href: '/',
    subItems: [
      // {
      //   title: 'Locations',
      //   categoryKey: 'weather/locations',
      //   href: '/weather/locations',
      // },
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
    categoryKey: 'water/stations',
    href: '/',
    subItems: [
      {
        title: 'Records',
        categoryKey: 'water/records',
        href: '/water/records',
      },
    ],
  },
  { title: 'About Us', href: '/about' },
  { title: 'Dashboard', href: '/dashboard' },
];
