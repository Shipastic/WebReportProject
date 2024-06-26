import React, {useState} from 'react';
import { slide as Menu } from 'react-burger-menu';
import { IconButton    } from 'rsuite';
import MenuIcon          from '@rsuite/icons/Menu'
import './BurgerMenu.css';

const BurgerMenuComponent: React.FC = () => {
  const [isOpen, setIsOpen] = useState(false);

  const handleStateChange = (state) => 
    {
      setIsOpen(state.isOpen);
    };

  const toggleMenu = () => 
    {
      setIsOpen(!isOpen);
    };

  return (
    <div>
        <IconButton
          icon={<MenuIcon />}
          onClick={toggleMenu}
          appearance="ghost"
        />
        <Menu
          isOpen={isOpen}
          onStateChange={(state) => handleStateChange(state)}
          width={'auto'} // Ширина по ширине содержимого
          customBurgerIcon={false}
          customCrossIcon={false}  
        >
          <a className="bm-item " href="/settings" onClick={toggleMenu}>
            Settings
          </a>
          <a className="bm-item " href="/about" onClick={toggleMenu}>
            Apps
          </a>
          <a className="bm-item " href="/home" onClick={toggleMenu}>
            Profile
          </a>
        </Menu>
    </div>
  );
};

export default BurgerMenuComponent;
