const NavButton = ({
  imgSrc,
  imgAlt,
  imgClassName,
}: {
  imgSrc: string;
  imgAlt: string;
  imgClassName: string;
}) => {
  return (
    <button
      className={`${imgClassName} absolute top-1/2 z-50 -translate-y-1/2 cursor-pointer`}
    >
      {<img src={imgSrc} alt={imgAlt} className="w-8 h-8" />}
    </button>
  );
};

export default NavButton;
