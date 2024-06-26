import React, { useEffect } from 'react';
import defaultImg from '..//../assets/main2.png';
import tpcImg from '..//../assets/TPC.jpg';
import tesc2Img from '..//../assets/TESC-2.jpg';
import tesc1Img from '..//../assets/defaultImg.jpg';

const ContentComponent = ({ selectedItem, updateBackgroundImage }) => {
    useEffect(() => {
        const getImage = () => {
          switch (selectedItem) {
            case 'ТЭСЦ-2':
              return tesc2Img;
            case 'ТПЦ':
              return tpcImg;
            case 'ТЭСЦ-1 Test':
              return tesc1Img;
            case 'ТЭСЦ-2 Test':
              return tesc2Img;
            default:
              return defaultImg;
          }
        };

        const image = getImage();
        updateBackgroundImage(image);
      }, [selectedItem, updateBackgroundImage]);

      return null;
};

export default ContentComponent;