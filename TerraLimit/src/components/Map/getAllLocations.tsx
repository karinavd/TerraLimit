export const getAllLocations = async () => {
  try {
    const baseURL = 'http://localhost:5088';
    const response = await fetch(`${baseURL}/weather/locations`);
    if (!response.ok) {
      throw new Error(`Failed to fetch locations: ${response.statusText}`);
    }
    return await response.json();
  } catch (err) {
    console.error(err);
  }
};
