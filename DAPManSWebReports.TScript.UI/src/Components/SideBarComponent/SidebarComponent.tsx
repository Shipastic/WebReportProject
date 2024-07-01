import React from 'react';
import { Link } from 'react-router-dom';
import { Sidenav, Nav} from 'rsuite';
import GearCircleIcon from '@rsuite/icons/legacy/GearCircle';
import GroupIcon from '@rsuite/icons/legacy/Group';
import DashboardIcon from '@rsuite/icons/legacy/Dashboard';

interface Props{
    openKeys: string[],
    expanded: boolean,
    onOpenChange: () => void,
    onExpand: () => void
}
const SideBarComponent: React.FC<Props>=({ openKeys, expanded, onOpenChange, onExpand, ...navProps })=>{

    const styles = {
      width:180,
      display: 'inline-table',
      marginRight: 10
    };
      const sidenavStyles = {
        background: 'black',
        color: '#ecf0f1',
        height: 'auto',    
    };

    const titleStyles = {
        color: '#ecf0f1',
        padding: '0px 2px 0px 2px',
        margin:'5px',
        background: 'black',
    };
    const linkStylesApps = {
        color: '#ecf0f1',    
        fontWeight: 'bold',
        fontSize:'16px',
        textDecoration: 'none',
        backgroundColor: 'black',
        marginLeft:'-25px'
    };
    const linkStylesProfile = {
        color: '#ecf0f1',    
        fontWeight: 'bold',
        fontSize:'16px',
        textDecoration: 'none',
        backgroundColor: 'black',
        marginLeft:'-12px'
    };
    const iconStyles = {
        color:'#11d0ed',
        fontSize: '32px',
        marginLeft: '5px',
        marginTop: '2px',
    };
    const iconItemStyles = {
       marginLeft: '5px', 
       marginTop: '3px', 
       color:'#11d0ed',
       fontSize: '26px'
    };
      return(
        <div style={styles}>
            <Sidenav 
                appearance={'subtle'}
                expanded={expanded}
                openKeys={openKeys}
                onOpenChange={onOpenChange}  
                style={sidenavStyles}          
            >
                <Sidenav.Body >
                    <Nav {...navProps}>
                          <Nav.Menu     eventKey="1"   icon={<GearCircleIcon style={iconItemStyles}/>} title="Настройки" style={titleStyles}>
                              <Nav.Item eventKey="1-1">
                                <div style={{marginLeft:'0px'}}>
                                <DashboardIcon style={iconStyles}/>
                                </div>
                                <Link to="/settings" style={linkStylesApps}>Приложение</Link>
                              </Nav.Item>
                              <Nav.Item eventKey="1-2">
                              <div style={{marginLeft:'0px'}}>
                                <GroupIcon style={iconStyles}/>
                                </div>
                                <Link to="/profile" style={linkStylesProfile }>Профиль</Link>
                              </Nav.Item>
                        </Nav.Menu>                     
                    </Nav>
                  </Sidenav.Body>
                  <Sidenav.Toggle onToggle={onExpand} style={{ background: '#6747dd', marginTop: '-8px', height:'1px' }} title='Свернуть' >
                </Sidenav.Toggle>
            </Sidenav>
        </div>
    )
};

export default SideBarComponent;