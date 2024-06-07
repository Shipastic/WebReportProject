import React, { useState, useEffect } from 'react';
import CogIcon from '@rsuite/icons/legacy/Cog';
import CharacterAuthorizeIcon from '@rsuite/icons/CharacterAuthorize';
import { Container, Header, Content, Navbar, Nav, Dropdown, Sidebar, Footer} from 'rsuite';
import './App.css';
//import './App.scss';
//import 'rsuite/dist/rsuite.css';
import ChildFolderComponent from './Components/ChildFolderComponent';
import QueryViewComponent from './Components/QueryViewComponent';
import SideBarComponent from './Components/SidebarComponent';
import Breadcrumbs  from './Models/Breadcrumbs';
import { FolderDetail } from './Models/FolderDetail';
import { Outlet, BrowserRouter, Routes, Route, Link } from 'react-router-dom';
import Home         from './pages/Home';
import About from './pages/About';
import FooterPage from './pages/FooterPage';
import Settings from './pages/Settings';
import LogIn from './pages/LogIn';
import NotFound from './pages/NotFound';
import logo from './assets/css/logo.jpg';
import ExcelIcon from './assets/icons-excel-48.png';
import FolderIcon from './assets/icons-folder-32.png';

interface HomePageProps {
    onSelect?: (eventKey: any) => void;
    activeKey?: string;
    [key: string]: any;
}

const App: React.FC<HomePageProps> = ({ onSelect, activeKey, ...props }) =>
{
    const [activeFolderId, setActiveFolderId] = useState<number | null>(null);
    const [items, setItems] = useState<FolderDetail | null>(null);
    const [isFolderComponentVisible, setFolderComponentVisible] = useState<boolean>(true);
    const [selectedDataViewId, setSelectedDataViewId] = useState<number | null>(null);

    const [breadcrumbs, setBreadcrumbs] = useState<string[]>(['Main']);

    const [activeKeySideBar, setActiveKey] = useState('1');
    const [openKeys, setOpenKeys] = useState(['1', '2']);
    const [expanded, setExpand] = useState(true);

    useEffect(() => {
        fetch('https://localhost:7263/api/menu/parents')
            .then(response => {
                if (response.ok) {
                    return response.json();
                }
                throw new Error('Network response was not ok.');
            })
            .then(data => setItems(data))
            .catch(error => console.error("There was an error fetching the reports: ", error));
    }, []);
   
    const handleDropdownSelect = () => {
        setActiveFolderId(null);  
        setBreadcrumbs(['Main']);
    };

    const handleItemClick = (id: number) => {
        setActiveFolderId(id);     
    };
    const handleDataViewClick = (dataviewid: number) => {
        setSelectedDataViewId(dataviewid);
    };

    const updateBreadcrumbs = (newBreadcrumb: string[]) => {
        setBreadcrumbs(newBreadcrumb);
    };
    const handlePageClick = () => {
        setBreadcrumbs(['Main']);
    };
   
    return (
        <BrowserRouter>     
         <div id="root">                     
            <Container className='container'>          
                    <Header className="custom-header">
                        <Navbar {...props} appearance="subtle">
                                <Navbar.Brand className="navbar-brand logo" href="#" >
                                    <img
                                        src={logo}
                                        alt="logo"
                                        style={{ padding: '0px 0px 15px 5px', height: '30px' }} />
                                </Navbar.Brand>                           
                                <Nav onSelect={onSelect} activeKey={activeKey} >
                                    <Nav.Item onClick={handlePageClick} className="custom-nav-item">
                                        <Link to="/">
                                        Main
                                        </Link>
                                    </Nav.Item>
                                    <Nav.Item onClick={handlePageClick} className="custom-nav-item"><Link to="/home" >Home</Link></Nav.Item> 
                                    <Nav.Item onClick={handlePageClick} className="custom-nav-item"><Link to="/about">About</Link></Nav.Item>
                                    <Dropdown title={"Select Plant"} onSelect={handleDropdownSelect} className='custom-dropdown-title'>                                   
                                        <Dropdown.Item className="custom-dropdown-item-static" disabled style={{ pointerEvents: 'none' }}>
                                            Plants name:
                                        </Dropdown.Item>
                                    {items?.childFolders.map(childFolder => (
                                        <Dropdown.Item
                                            key={childFolder.id}
                                            eventKey={childFolder.id}
                                            onSelect={() => {handleItemClick(childFolder.id), updateBreadcrumbs([childFolder.name])}}
                                            className="custom-dropdown-item"
                                        >                                           
                                        {childFolder.name}
                                    </Dropdown.Item>
                                ))}
                                    <Dropdown.Item className="custom-dropdown-item-static" disabled style={{ pointerEvents: 'none' }}>
                                        Reports:
                                     </Dropdown.Item>
                                    {items?.dataviews.map(dataview => (
                                        <Dropdown.Item
                                            key={dataview.id}
                                            onClick={() => {handleDataViewClick(dataview.id),updateBreadcrumbs(['Main',dataview.name])}}
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
                                            icon={<CharacterAuthorizeIcon color='#1E90FF'/>} className="custom-nav-item">
                                                <Link to="/login" className="nav-link">
                                                    <span style={{padding:'0 0 0 4px', marginRight:'20px'}}>
                                                        LogIn
                                                    </span>
                                                </Link>
                                    </Nav.Item>                                   
                                </Nav> 
                        </Navbar >                      
                    </Header>
                    <Container className='container'>
                        <Sidebar className='sidebar' style={{flex:'0 1'}}>
                            <SideBarComponent                            
                                  activeKeySideBar={setActiveKey}
                                  openKeys={openKeys}
                                  onSelect={setActiveKey}
                                  onOpenChange={setOpenKeys}
                                  expanded={expanded}
                                  onExpand={setExpand}
                                  appearance="subtle"/>
                        </Sidebar>
                        <Content className="content" >
                        <Breadcrumbs breadcrumbs={breadcrumbs} />
                        {selectedDataViewId !== null && (
                            <div className="query-view-container">
                                <QueryViewComponent dataviewid={selectedDataViewId} path={'/'} updateBreadcrumbs={updateBreadcrumbs} breadcrumbs={breadcrumbs} />
                            </div>
                        )}
                        <Routes>
                            <Route path="/home" element={<Home
                                                            path=""
                                                            updateBreadcrumbs={updateBreadcrumbs}
                                                            breadcrumbs={breadcrumbs}            />} />
                            <Route path="/login" element={<LogIn
                                                            path="/login"
                                                            updateBreadcrumbs={updateBreadcrumbs}
                                                            breadcrumbs={breadcrumbs}            />} />     
                            <Route path="/about" element={<About
                                                            path="/about"
                                                            updateBreadcrumbs={updateBreadcrumbs}
                                                            breadcrumbs={breadcrumbs}            />} />
                            <Route path="*"      element={<NotFound
                                                            path="NotFoundPAge"
                                                            updateBreadcrumbs={updateBreadcrumbs}
                                                            breadcrumbs={breadcrumbs}            />} />
                            <Route path="/"      element={activeFolderId !== null && isFolderComponentVisible && (
                                                          <ChildFolderComponent
                                                            parentid={activeFolderId}
                                                            path={'/activeFolderId'}
                                                            updateBreadcrumbs={updateBreadcrumbs}
                                                            breadcrumbs={breadcrumbs} />)} />
                            <Route path="/settings" element={<Settings
                                                                path=""
                                                                updateBreadcrumbs={updateBreadcrumbs}
                                                                breadcrumbs={breadcrumbs}        />} />
                        </Routes>    
                        <Outlet />
                        </Content>
                    </Container>
                    <Footer className='footer'>
                        <FooterPage/>
                    </Footer>                   
            </Container> 
            </div>                             
        </BrowserRouter>
    );
}
export default App;