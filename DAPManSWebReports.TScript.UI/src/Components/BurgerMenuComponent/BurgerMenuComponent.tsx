import React, {useState} from 'react';
import { slide as Menu } from 'react-burger-menu';
import { useUser } from '../UserContext/UserContext';
import { IconButton    } from 'rsuite';
import MenuIcon          from '@rsuite/icons/Menu';
import './BurgerMenu.css';

const BurgerMenuComponent: React.FC = () => {
  const [isOpen, setIsOpen] = useState(false);
  const { user } = useUser();
  const handleStateChange = (state) => { setIsOpen(state.isOpen);};
  const toggleMenu = () => { setIsOpen(!isOpen); };

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
          <a className="menu-item " href="/settings"    onClick={toggleMenu}>
            Настройки
          </a>
          <a className="menu-item " href="/about"       onClick={toggleMenu}>
            Приложение
          </a>
          <a className="menu-item--small " href="/home" onClick={toggleMenu}>
            Профиль
          </a>
          {user && user.role ==='admin' && (
          <a className="menu-item" href="/admin"        onClick={toggleMenu}>
            Админ Панель
          </a>
          )}
          {user && user.role === 'user' && (
          <a className="menu-item" href="/user"         onClick={toggleMenu}>
            Дашборд
          </a>
          )}
        </Menu>
        </div>
    </div>
  );
};

export default BurgerMenuComponent;
