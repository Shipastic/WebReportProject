import React from 'react';
import { useNavigate } from 'react-router-dom';
import { Navbar, Nav, Dropdown, Header} from 'rsuite';
import { Link                         } from 'react-router-dom';
import UserChangeIcon                   from '@rsuite/icons/UserChange';
import {useUser                       } from '../UserContext/UserContext';
import './Header.css';
import BurgerMenuComponent from '../BurgerMenuComponent/BurgerMenuComponent';

interface HeaderProps 
{
  className: string;
  props: any;
  activeKey: any;
  handlePageClick: (eventKey: any) => void;
  handleDropdownSelect: (eventKey: any) => void;
  handleItemClick: (itemId: any) => void;
  handleItemClickName: (itemName: any) => void;
  updateBreadcrumbs: (breadcrumbs: string[]) => void;
  handleDataViewClick: (dataViewId: any) => void;
  onSelect: (eventKey: any) => void;
  items: any;
  logo: string;
}

const HeaderComponent: React.FC<HeaderProps> = ({
  className,
  props,
  activeKey,
  handlePageClick,
  handleDropdownSelect,
  handleItemClick,
  handleItemClickName,
  updateBreadcrumbs,
  handleDataViewClick,
  onSelect,
  items,
  logo
}) => {
  const { user , logout} =  useUser() ?? {};
  
  const navigate = useNavigate();
  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <Header className={className}>
      <Navbar {...props} appearance="subtle">
      <div style={{ display: 'flex', alignItems: 'center'}}>
      <BurgerMenuComponent/>
        <Navbar.Brand  href="https://omk-job.ru/" style={{marginBottom:'15px'}}>
          <img
            src={logo}
            alt="logo"
            className='imgHeader'
          />
        </Navbar.Brand>
        <Nav onSelect={onSelect} activeKey={activeKey} style={{marginLeft:'15px'}}>
          <Nav.Item onClick={handlePageClick} className="custom-nav-item">
            <Link to="/">Главная</Link>
          </Nav.Item>
          <Nav.Item
            onClick={handlePageClick}
            className="custom-nav-item"
          >
            <Link to="/home">Домашняя</Link>
          </Nav.Item>
          <Nav.Item
            onClick={handlePageClick}
            className="custom-nav-item"
          >
            <Link to="/about">О портале</Link>
          </Nav.Item>
          {user && user.role === 'admin'? (
          <Dropdown
            title={"Выбор цеха"}
            onSelect={handleDropdownSelect}
            className="custom-dropdown-title"
          >
            <Dropdown.Item className="custom-dropdown-item-static" style={{backgroundColor:"black", color:"white"}}>
              Название Цеха:
            </Dropdown.Item> 
            {items?.childFolders.map((childFolder: any) => (
              <Dropdown.Item
                key={childFolder.id}
                eventKey={childFolder.id}
                onSelect={() =>
                  {
                    handleItemClick(childFolder.id);
                    handleItemClickName(childFolder.name);
                    updateBreadcrumbs([childFolder.name]);
                  }
                }
                className="custom-dropdown-item"
              >
                {childFolder.name}
              </Dropdown.Item>
            ))}
            <Dropdown.Item className="custom-dropdown-item-static" style={{backgroundColor:"black", color:"white"}}>
              Избранные Отчеты:
            </Dropdown.Item>
            {items?.dataviews.map((dataview: any) => (
              <Dropdown.Item
                key={dataview.id}
                onClick={() => handleDataViewClick(dataview.id)}
                className="custom-dropdown-item"
              >
                {dataview.name}
              </Dropdown.Item>
            ))}
          </Dropdown>
            ) :(<></>)}
        </Nav>
        <Nav pullRight>
        {user ? (
        <Dropdown title={<><UserChangeIcon /> {user.username}</>} className="custom-dropdown-title">
          <Dropdown.Item onClick={handlePageClick} className="custom-dropdown-item-login">Профиль</Dropdown.Item>
          <Dropdown.Item onClick={handleLogout} className="custom-dropdown-item-login">
            Выйти
          </Dropdown.Item>
        </Dropdown>
      ) : (
        <Nav.Item onClick={handlePageClick} className="custom-nav-item">
          <Link to="/login" className="nav-link">
            Войти
          </Link>
        </Nav.Item>
      )}
          </Nav>
        </div>
      </Navbar>
    </Header>
  );
};

export default HeaderComponent;
