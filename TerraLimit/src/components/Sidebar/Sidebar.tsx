const Sidebar = () => {
  return (
    <div className="h-full w-1/4 absolute top-0 right-0 z-1 p-2 bg-gray-200">
      <h2 className="text-lg font-bold mb-4">Location Details</h2>
      <p className="text-muted-foreground">City: </p>
      <p className="text-foreground font-medium">New York</p>
      <p className="text-muted-foreground">Region: </p>
      <p className="text-foreground font-medium">Manhattan</p>
      <p className="text-muted-foreground">Country: </p>
      <p className="text-foreground font-medium">USA</p>
    </div>
  );
};

export default Sidebar;
