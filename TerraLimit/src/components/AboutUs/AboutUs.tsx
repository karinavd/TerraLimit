import NavButton from './NavButton';
import arrowLeft from '../../assets/arrow.png';
import dotBg from '../../assets/dot_bg.png';
import 'swiper/css';
import 'swiper/css/navigation';
import { Swiper, SwiperSlide } from 'swiper/react';
import { Navigation } from 'swiper/modules';
import { teamMembers } from './teamMembers';
import './AboutUs.css';
const AboutUs = () => {
  return (
    <div className="relative w-full mx-auto py-10">
      <NavButton
        imgSrc={arrowLeft}
        imgAlt="Prev"
        imgClassName="custom-prev left-0"
      />
      <NavButton
        imgSrc={arrowLeft}
        imgAlt="Next"
        imgClassName="custom-next right-0 rotate-180"
      />

      <Swiper
        modules={[Navigation]}
        spaceBetween={30}
        slidesPerView={3}
        centeredSlides={true}
        initialSlide={1}
        loop={true}
        navigation={{
          prevEl: '.custom-prev',
          nextEl: '.custom-next',
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
                className={`rounded-2xl overflow-hidden flex items-end justify-center w-full aspect-[4/3] ${member.color}`}
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
                <h3 className="text-2xl font-serif font-bold uppercase">
                  {member.name}
                </h3>
                <p className="text-gray-600 text-sm mt-1">{member.role}</p>
                <div className="social-links flex justify-center gap-4 mt-3 opacity-0">
                  {member.github && <a href={member.github}>GitHub</a>}
                  {member.linkedin && <a href={member.linkedin}>LinkedIn</a>}
                </div>
              </div>
            </div>
          </SwiperSlide>
        ))}
      </Swiper>
    </div>
  );
};

export default AboutUs;
