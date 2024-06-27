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
    <div >
        <IconButton
          icon={<MenuIcon  style={{color:'white', fontSize:'22px'}}/>}
          onClick={toggleMenu}
          appearance="subtle"
        />
        <div>
        <Menu
          isOpen={isOpen}
          onStateChange={(state) => handleStateChange(state)}
          width='auto'
          customBurgerIcon={false}
          customCrossIcon={false}  
        >
          <a className="menu-item " href="/settings" onClick={toggleMenu}>
            Settings
          </a>
          <a className="menu-item " href="/about" onClick={toggleMenu}>
            Apps
          </a>
          <a className="menu-item--small " href="/home" onClick={toggleMenu}>
            Profile
          </a>
        </Menu>
        </div>
    </div>
  );
};

export default BurgerMenuComponent;
