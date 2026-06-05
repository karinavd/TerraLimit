import NavButton from './NavButton';
import arrowLeft from '../../assets/arrow.png';
import dotBg from '../../assets/dot_bg.png';
import 'swiper/css';
import 'swiper/css/navigation';
import { Swiper, SwiperSlide } from 'swiper/react';
import { Navigation, Keyboard } from 'swiper/modules';
import { teamMembers } from './teamMembers';
import './AboutUs.css';
import linkedin from '../../assets/linkedin.png';
import github from '../../assets/github.png';
const AboutUs = () => {
  return (
    <div className="flex flex-col  z-1 w-full h-full items-center justify-between  px-4">
      <div className="relative w-full mx-0 my-auto py-10">
        <NavButton
          imgSrc={arrowLeft}
          imgAlt="Prev"
          imgClassName=" custom-prev z-1 left-0 -scale-x-100"
        />
        <NavButton
          imgSrc={arrowLeft}
          imgAlt="Next"
          imgClassName="custom-next right-0 "
        />

        <Swiper
          modules={[Navigation, Keyboard]}
          spaceBetween={30}
          slidesPerView={3}
          centeredSlides={true}
          initialSlide={1}
          loop={true}
          navigation={{
            prevEl: '.custom-prev',
            nextEl: '.custom-next',
          }}
          keyboard={{
            enabled: true,
          }}
          breakpoints={{
            320: { slidesPerView: 1 },
            768: { slidesPerView: 2 },
            1024: { slidesPerView: 3 },
          }}
          className="team-swiper"
        >
          {[...teamMembers, ...teamMembers].map((member, index) => (
            <SwiperSlide key={index}>
              <div className="flex flex-col items-center">
                <div
                  className={`rounded-2xl overflow-hidden flex items-end justify-center w-full cursor-pointer aspect-[4/3] ${member.color}`}
                  style={{
                    backgroundImage: `url(${dotBg})`,
                    backgroundSize: 'cover',
                    backgroundPosition: 'center',
                    backgroundRepeat: 'no-repeat',
                  }}
                >
                  <img
                    src={member.image}
                    alt={member.name}
                    className="object-cover h-[93%] w-auto grayscale"
                  />
                </div>
                <div className="mt-6 text-center member-info transition-all duration-300">
                  <h3 className="text-[27px] font-serif font-bold uppercase">
                    {member.name}
                  </h3>
                  <p className="text-gray-600 text-[17px] mt-1">
                    {member.role}
                  </p>
                  <div className="social-links flex justify-center gap-4 mt-3 opacity-0">
                    {member.github && (
                      <div className="flex items-center gap-2">
                        {' '}
                        <img
                          src={github}
                          alt="GitHub"
                          className="w-5 h-5"
                        />{' '}
                        <a target="_blank" href={member.github}>
                          GitHub
                        </a>
                      </div>
                    )}
                    {member.linkedin && (
                      <div className="flex items-center gap-2">
                        {' '}
                        <img
                          src={linkedin}
                          alt="LinkedIn"
                          className="w-5 h-5"
                        />{' '}
                        <a target="_blank" href={member.linkedin}>
                          LinkedIn
                        </a>
                      </div>
                    )}
                  </div>
                </div>
              </div>
            </SwiperSlide>
          ))}
        </Swiper>
      </div>
    </div>
  );
};

export default AboutUs;
