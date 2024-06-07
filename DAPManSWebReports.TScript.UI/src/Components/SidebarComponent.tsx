import React, { useState } from 'react';
import ReactDOM from 'react-dom';
import { Link } from 'react-router-dom';
import { Sidenav, Nav} from 'rsuite';
import GearCircleIcon from '@rsuite/icons/legacy/GearCircle';
import GroupIcon from '@rsuite/icons/legacy/Group';
import DashboardIcon from '@rsuite/icons/legacy/Dashboard';

const SideBarComponent: React.FC=({ appearance, openKeys, expanded, onOpenChange, onExpand, ...navProps })=>{
    const styles = {
        width: 60,
        display: 'inline-table',
      };

      const sidenavStyles = {
        backgroundColor: '#4e6b8b',
        color: '#ecf0f1',
        height: '80vh',
        
    };

    const itemStyles = {
        padding: '0px 0px',
        backgroundColor: '#4e6b8b',
        color: '#ecf0f1',

    };

    const titleStyles = {
        color: '#ecf0f1',
        padding: '10px 0px',

    };

    const linkStyles = {
        color: '#FFF0F5',
        fontWeight: 'bold',
        textDecoration: 'none',
        
    };

    const iconStyles = {
        fontSize: '32px',
        color:'#1E90FF'
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
                        <Nav.Item eventKey="1"  active icon={<GearCircleIcon  style={iconStyles}/>}>
                            <Link to="/settings">
                                <span style={itemStyles}>
                                    Settings
                                 </span>
                            </Link>
                        </Nav.Item>
                        <Nav.Item eventKey="2" icon={<DashboardIcon  style={iconStyles}/>}>
                            <Link to="/settings">
                                <span style={itemStyles}>
                                    Applications
                                </span>
                            </Link>
                        </Nav.Item>
                        <Nav.Item eventKey="3" icon={<GroupIcon  style={iconStyles}/>}>
                            <Link to="/profile">
                                <span style={itemStyles}>
                                    Profile
                                </span>
                            </Link>
                        </Nav.Item>
                    </Nav>
                </Sidenav.Body>
                <Sidenav.Toggle onToggle={onExpand} style={{background:'#1E90FF'}} >
                </Sidenav.Toggle>
            </Sidenav>
        </div>
    )
};

export default SideBarComponent;