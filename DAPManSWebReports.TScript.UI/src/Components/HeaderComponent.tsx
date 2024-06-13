import React from 'react';
import { Navbar, Nav, Dropdown, Header} from 'rsuite';
import { Link } from 'react-router-dom';
import CharacterAuthorizeIcon from '@rsuite/icons/CharacterAuthorize';
import '../App.css';

interface HeaderProps {
  className: string;
  props: any;
  activeKey: any;
  handlePageClick: (eventKey: any) => void;
  handleDropdownSelect: (eventKey: any) => void;
  handleItemClick: (itemId: any) => void;
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
  updateBreadcrumbs,
  handleDataViewClick,
  onSelect,
  items,
  logo
}) => {
  return (
    <Header className={className}>
      <Navbar {...props} appearance="subtle">
        <Navbar.Brand className="navbar-brand logo" href="#">
          <img
            src={logo}
            alt="logo"
            style={{ padding: '0px 0px 15px 5px', height: '30px' }}
          />
        </Navbar.Brand>
        <Nav onSelect={onSelect} activeKey={activeKey}>
          <Nav.Item onClick={handlePageClick} className="custom-nav-item">
            <Link to="/">Main</Link>
          </Nav.Item>
          <Nav.Item
            onClick={handlePageClick}
            className="custom-nav-item"
          >
            <Link to="/home">Home</Link>
          </Nav.Item>
          <Nav.Item
            onClick={handlePageClick}
            className="custom-nav-item"
          >
            <Link to="/about">About</Link>
          </Nav.Item>
          <Dropdown
            title={"Select Plant"}
            onSelect={handleDropdownSelect}
            className="custom-dropdown-title"
          >
            <Dropdown.Item className="custom-dropdown-item">
              Plants name:
            </Dropdown.Item>
            {items?.childFolders.map((childFolder: any) => (
              <Dropdown.Item
                key={childFolder.id}
                eventKey={childFolder.id}
                onSelect={() =>
                  {
                    handleItemClick(childFolder.id);
                    updateBreadcrumbs([childFolder.name]);
                  }
                }
                className="custom-dropdown-item"
              >
                {childFolder.name}
              </Dropdown.Item>
            ))}
            <Dropdown.Item className="custom-dropdown-item">
              Reports:
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
        </Nav>
        <Nav pullRight>
          <Nav.Item
            onClick={handlePageClick}
            className="custom-nav-item"
            icon={<CharacterAuthorizeIcon color="#1E90FF" />}
          >
            <Link to="/login" className="nav-link">
              <span style={{ padding: '0 0 0 5px' }}>LogIn</span>
            </Link>
          </Nav.Item>
        </Nav>
      </Navbar>
    </Header>
  );
};

export default HeaderComponent;