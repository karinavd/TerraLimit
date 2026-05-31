export const getAllLocations = async (
  endpoint: string = 'weather/locations'
) => {
  try {
    const baseURL = 'http://localhost:5088';
    const response = await fetch(`${baseURL}/${endpoint}?limit=10000`);
    if (!response.ok) {
      throw new Error(`Failed to fetch locations: ${response.statusText}`);
    }
    return await response.json();
  } catch (err) {
    console.error(err);
  }
};
