import React from 'react';
import { Link } from 'react-router-dom';
import { Sidenav, Nav} from 'rsuite';
import GearCircleIcon from '@rsuite/icons/legacy/GearCircle';
import GroupIcon from '@rsuite/icons/legacy/Group';
import DashboardIcon from '@rsuite/icons/legacy/Dashboard';

const SideBarComponent: React.FC=({ appearance, openKeys, expanded, onOpenChange, onExpand, ...navProps })=>{
    const styles = {
        width: 'auto',
        display: 'inline-table',
      };

      const sidenavStyles = {
        backgroundColor: '#4e6b8b',
        color: '#ecf0f1',
        height: 'auto',
        
    };

    const itemAppsStyles = {
        backgroundColor: '#821ba1',
        marginTop: '2px',
        marginLeft: '10px',
        padding: '0px 10px 0 10px',

    };
    const itemProfileStyles = {
        backgroundColor: '#9710c9',
        marginTop: '2px',
        marginLeft: '10px',
        padding: '0px 10px 0 10px',

    };

    const titleStyles = {
        color: '#ecf0f1',
        padding: '0px 0px'
    };

    const linkStyles = {
        color: '#ecf0f1',    
        fontWeight: 'bold',
        textDecoration: 'none',
        
    };

    const iconStyles = {
        marginLeft: '5px',
        fontSize: '36px',
        color:'#11d0ed'
    };
    const iconItemStyles = {
        marginLeft: '10px',
        fontSize: '36px',
        color: '#11d0ed'
    };
      return(
        <div style={styles}>
            <Sidenav 
                appearance={appearance}
                expanded={expanded}
                openKeys={openKeys}
                onOpenChange={onOpenChange}
                style={{backgroundColor: '#4e6b8b'}}           
            >
                <Sidenav.Body >
                    <Nav {...navProps}>
                          <Nav.Menu     eventKey="1"   icon={<GearCircleIcon style={iconStyles}     />} title="Settings" style={titleStyles}>
                              <Nav.Item eventKey="1-1" icon={<DashboardIcon  style={iconItemStyles} />} style={itemAppsStyles}>
                                  <Link to="/settings" style={linkStyles}>
                                      <span style={{marginLeft:'10px'} }>
                                          Apps
                                      </span>
                                  </Link>
                              </Nav.Item>
                              <Nav.Item eventKey="1-2" icon={<GroupIcon      style={iconItemStyles} />} style={itemProfileStyles}>
                                  <Link to="/profile" style={linkStyles }>
                                      <span style={{ marginLeft: '10px' }}>
                                          Profile
                                      </span>
                                  </Link>
                              </Nav.Item>
                        </Nav.Menu>                     
                    </Nav>
                  </Sidenav.Body>
                  <Sidenav.Toggle onToggle={onExpand} style={{ background: '#1E90FF', marginTop: '-5px', height:'20px' }}>
                </Sidenav.Toggle>
            </Sidenav>
        </div>
    )
};

export default SideBarComponent;